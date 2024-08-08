using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class TileChunking : MonoBehaviour
{
    public Tilemap mainTilemap;
    public Vector2Int chunkSize = new Vector2Int(10, 10);
    private List<Vector2Int> chunkCenters;
    private Vector3 lastCameraPosition;
    private float updateThreshold = 5.0f; 
    private Vector2Int previousChunkIndex;
    [SerializeField] Color customColor = Color.white;

    void Start()
    {
        BoundsInt bounds = mainTilemap.cellBounds;
        int rows = Mathf.CeilToInt((float)bounds.size.y / chunkSize.y);
        int cols = Mathf.CeilToInt((float)bounds.size.x / chunkSize.x);
        chunkCenters = new List<Vector2Int>();
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector2Int chunkCenter = new Vector2Int(
                    bounds.xMin + col * chunkSize.x + chunkSize.x / 2,
                    bounds.yMin + row * chunkSize.y + chunkSize.y / 2
                );
                chunkCenters.Add(chunkCenter);
            }
        }
        lastCameraPosition = Camera.main.transform.position;
        previousChunkIndex = GetChunkIndex(lastCameraPosition);
        UpdateChunksVisibility(previousChunkIndex, Vector2Int.zero).Forget();
    }

    void Update()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;
        Vector3 cameraPos = mainCamera.transform.position;
        if (Vector3.Distance(cameraPos, lastCameraPosition) > updateThreshold)
        {
            Vector2Int currentChunkIndex = GetChunkIndex(cameraPos);
            Vector2Int movementDirection = currentChunkIndex - previousChunkIndex;
            UpdateChunksVisibility(currentChunkIndex, movementDirection).Forget();
            lastCameraPosition = cameraPos;
            previousChunkIndex = currentChunkIndex;
        }
    }

    Vector2Int GetChunkIndex(Vector3 position)
    {
        Vector3 localPos = mainTilemap.WorldToCell(position);
        int col = Mathf.FloorToInt((localPos.x - mainTilemap.cellBounds.xMin) / (float)chunkSize.x);
        int row = Mathf.FloorToInt((localPos.y - mainTilemap.cellBounds.yMin) / (float)chunkSize.y);
        return new Vector2Int(col, row);
    }

    private async UniTaskVoid UpdateChunksVisibility(Vector2Int currentChunkIndex, Vector2Int movementDirection)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;
        Vector3 cameraPos = mainCamera.transform.position;
        float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect * 2;
        float cameraHalfHeight = mainCamera.orthographicSize * 2;
        float buffer = 8f; 
        Rect cameraRect = new Rect(
            cameraPos.x - cameraHalfWidth - buffer,
            cameraPos.y - cameraHalfHeight - buffer,
            (cameraHalfWidth + buffer) * 2,
            (cameraHalfHeight + buffer) * 2
        );
        for (int i = 0; i < chunkCenters.Count; i++)
        {
            Vector2Int chunkCenter = chunkCenters[i];
            Vector3 chunkWorldPos = mainTilemap.CellToWorld((Vector3Int)chunkCenter);
            Rect chunkRect = new Rect(
                chunkWorldPos.x - chunkSize.x * mainTilemap.cellSize.x / 2,
                chunkWorldPos.y - chunkSize.y * mainTilemap.cellSize.y / 2,
                chunkSize.x * mainTilemap.cellSize.x,
                chunkSize.y * mainTilemap.cellSize.y
            );
            if (movementDirection == Vector2Int.zero || IsChunkAffected(currentChunkIndex, chunkCenter, movementDirection))
            {
                bool inView = cameraRect.Overlaps(chunkRect);
                for (int x = 0; x < chunkSize.x; x++)
                {
                    for (int y = 0; y < chunkSize.y; y++)
                    {
                        Vector3Int tilePos = new Vector3Int(
                            chunkCenter.x - chunkSize.x / 2 + x,
                            chunkCenter.y - chunkSize.y / 2 + y,
                            0
                        );
                        mainTilemap.SetTileFlags(tilePos, TileFlags.None);
                        mainTilemap.SetColor(tilePos, inView ? customColor : Color.clear);
                    }
                }
            }
            await UniTask.Yield();
        }
    }

    bool IsChunkAffected(Vector2Int currentChunkIndex, Vector2Int chunkCenter, Vector2Int movementDirection)
    {
        Vector2Int chunkIndex = GetChunkIndex(mainTilemap.CellToWorld((Vector3Int)chunkCenter));
        return chunkIndex == currentChunkIndex || chunkIndex == currentChunkIndex + movementDirection;
    }
}

