using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFruitFactory : MonoBehaviour, ISpawner
{
    [Header("Fruits")]
    public GameObject[] fruitPrefabs = null;

    public IObject GetObject()
    {
        GameObject prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];
        return prefab.GetComponent<IObject>();
    }
}