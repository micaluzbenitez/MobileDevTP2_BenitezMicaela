using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectFactory : MonoBehaviour, ISpawner
{
    [Header("Fruits")]
    public GameObject[] fruitPrefabs = null;

    [Header("Bomb")]
    public GameObject bomb = null;
    [Range(0f, 1f)] public float bombChance = 0.05f;

    [Header("Difficult")]
    public bool increaseDifficult = false;
    public float timePerChange = 0f;
    [Range(0f, 1f)] public float maxBombChance = 0f;
    public float increaseBombChanceValue = 0f;

    private float difficultTimer = 0f;

    private void Update()
    {
        IncreaseGameDifficult();
    }

    private void IncreaseGameDifficult()
    {
        if (increaseDifficult && (bombChance < maxBombChance))
        {
            difficultTimer += Time.deltaTime;

            if (difficultTimer > timePerChange)
            {
                bombChance += increaseBombChanceValue;
                if (bombChance > maxBombChance) bombChance = maxBombChance;
                difficultTimer = 0f;
            }
        }
    }
    public void SetBombSpawner(float bombChance, bool increaseDifficult, float timePerChange, float maxBombChance, float increaseBombChanceValue)
    {
        this.bombChance = bombChance;
        this.increaseDifficult = increaseDifficult;
        this.timePerChange = timePerChange;
        this.maxBombChance = maxBombChance;
        this.increaseBombChanceValue = increaseBombChanceValue;
    }

    public IObject GetObject()
    {
        GameObject prefab = null;

        if (Random.value < bombChance) prefab = bomb;
        else prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];

        return prefab.GetComponent<IObject>();
    }
}