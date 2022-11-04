using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Data", menuName = "Game/Game Data")]
public class GameData : ScriptableObject
{
    [Serializable]
    public class Levels
    {
        public string tag = "";
        public int level = 0;

        [Header("Bomb")]
        [Range(0f, 1f)] public float bombChance;

        [Header("Spawner")]
        public float maxSpawnDelay = 1f;
        public float minSpawnDelay = 0.25f;

        [Header("Difficult")]
        public bool increaseDifficult = false;
        public float timePerChange = 0f;
        [Range(0f, 1f)] public float maxBombChance = 0f;
        public float increaseBombChanceValue = 0f;
    }

    public List<Levels> levels;
}