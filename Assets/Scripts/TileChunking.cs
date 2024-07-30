using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChunking : MonoBehaviour
{
    public Tilemap mainTilemap; 
    public Vector2Int chunkSize = new Vector2Int(10, 10); 
    private Tilemap[,] chunks; 

    void Start()
    {
        BoundsInt bounds = mainTilemap.cellBounds;
        int rows = Mathf.CeilToInt((float)bounds.size.y / chunkSize.y);
        int cols = Mathf.CeilToInt((float)bounds.size.x / chunkSize.x);
        chunks = new Tilemap[rows, cols];
        for (int row = 0; row < rows; row++){
            for (int col = 0; col < cols; col++){
                // Create a new GameObject for the chunk
                GameObject chunkGO = new GameObject($"Chunk_{row}_{col}");
                chunkGO.transform.SetParent(transform);

                // Add Tilemap and TilemapRenderer components
                Tilemap chunkTilemap = chunkGO.AddComponent<Tilemap>();
                chunkGO.AddComponent<TilemapRenderer>();

                // Set the chunk's position in world space
                Vector3Int chunkOrigin = new Vector3Int(
                    bounds.x + col * chunkSize.x,
                    bounds.y + row * chunkSize.y,
                    0
                );
                chunkGO.transform.position = mainTilemap.CellToWorld(chunkOrigin);

                // Copy tiles from the main Tilemap to the chunk Tilemap
                for (int x = 0; x < chunkSize.x; x++)
                {
                    for (int y = 0; y < chunkSize.y; y++)
                    {
                        // Tile position in main Tilemap
                        Vector3Int tilePos = new Vector3Int(chunkOrigin.x + x, chunkOrigin.y + y, 0);

                        // Check if tile exists at this position
                        TileBase tile = mainTilemap.GetTile(tilePos);
                        if (tile != null)
                        {
                            // Set tile in the chunk Tilemap
                            Vector3Int localTilePos = new Vector3Int(x, y, 0);
                            chunkTilemap.SetTile(localTilePos, tile);
                        }
                    }
                }
                chunks[row, col] = chunkTilemap;
            }
        }
        mainTilemap.GetComponent<TilemapRenderer>().enabled = false;
    }

    void Update(){
        UpdateChunksVisibility();
    }

    void UpdateChunksVisibility(){
        Camera mainCamera = Camera.main;
        Vector3 cameraPos = mainCamera.transform.position;
        float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        float cameraHalfHeight = mainCamera.orthographicSize;
        Rect cameraRect = new Rect(
            cameraPos.x - cameraHalfWidth,
            cameraPos.y - cameraHalfHeight,
            cameraHalfWidth * 2,
            cameraHalfHeight * 2
        );
        foreach (var chunk in chunks){
            if (chunk != null){
                Vector2 chunkWorldPos = chunk.transform.position;
                Rect chunkRect = new Rect(
                    chunkWorldPos.x,
                    chunkWorldPos.y,
                    chunkSize.x * mainTilemap.cellSize.x,
                    chunkSize.y * mainTilemap.cellSize.y
                );
                bool inView = cameraRect.Overlaps(chunkRect);
                chunk.gameObject.SetActive(inView);
            }
        }
    }
}
