using System.Collections.Generic;
using UnityEngine;

public class BombUI : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab;

    private readonly List<GameObject> bombs = new();

    public void InitBombs(int bombCount)
    {
        ClearBombs();

        for (int i = 0; i < bombCount; i++)
        {
            GameObject bomb = Instantiate(bombPrefab, transform);
            bombs.Add(bomb);
        }
    }

    public void DestroyBomb()
    {
        if (bombs.Count <= 0) return;

        GameObject target = bombs[bombs.Count - 1];
        bombs.Remove(target);
        Destroy(target);
    }

    private void ClearBombs()
    {
        for (int i = bombs.Count - 1; i >= 0; i--)
        {
            if (bombs[i] != null)
            {
                Destroy(bombs[i]);
            }
        }

        bombs.Clear();
    }
}