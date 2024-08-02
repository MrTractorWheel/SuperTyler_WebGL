using System.Collections.Generic;
using UnityEngine;

public class CoinAnimatorManager : MonoBehaviour
{
    private List<Transform> coins = new List<Transform>();
    private List<Animator> animators = new List<Animator>();
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        var coinGameObjects = GameObject.FindGameObjectsWithTag("Coin");

        foreach (var coin in coinGameObjects)
        {
            coins.Add(coin.transform);
            var animator = coin.GetComponent<Animator>();

            if (animator != null)
            {
                animators.Add(animator);
            }
            else
            {
                Debug.LogWarning($"Coin at {coin.transform.position} does not have an Animator component.");
            }
        }
    }

    void Update()
    {
        for (int i = coins.Count - 1; i >= 0; i--)
        {
            if (coins[i] == null)
            {
                coins.RemoveAt(i);
                animators.RemoveAt(i);
            }
            else
            {
                if (IsInView(mainCamera, coins[i].position))
                {
                    animators[i].enabled = true;
                }
                else
                {
                    animators[i].enabled = false;
                }
            }
        }
    }

    private bool IsInView(Camera cam, Vector3 worldPosition)
    {
        Vector3 viewportPosition = cam.WorldToViewportPoint(worldPosition);
        return viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1 && viewportPosition.z >= 0;
    }
}
