using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class Spawner : MonoBehaviour
    {
        [Header("Fruits and bomb")]
        public GameObject[] fruitPrefabs = null;
        public GameObject bomb = null;
        public float maxLifeTime = 5f;
        [Range(0f, 1f)] public float bombChance = 0.05f;

        [Header("Spawner")]
        public float startSpawnDelay = 2f;
        public float maxSpawnDelay = 1f;
        public float minSpawnDelay = 0.25f;
        public float maxAngle = 15f;
        public float minAngle = -15f;
        public float maxForce = 22f;
        public float minForce = 18f;

        [Header("Difficult")]
        public bool increaseDifficult = false;
        public float timePerChange = 0f;
        [Range(0f, 1f)] public float maxBombChance = 0f;
        public float increaseBombChanceValue = 0f;

        private Collider spawnArea = null;
        private float difficultTimer = 0f;

        private void Awake()
        {
            spawnArea = GetComponent<Collider>();
        }

        private void Update()
        {
            IncreaseGameDifficult();
        }

        private void OnEnable()
        {
            StartCoroutine(Spawn());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
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

        private IEnumerator Spawn()
        {
            yield return new WaitForSeconds(startSpawnDelay);

            while (enabled)
            {
                GameObject prefab = null;
                if (Random.value < bombChance) prefab = bomb;
                else prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];

                GameObject instantiate = Instantiate(prefab, CalculatePosition(), CalculateRotation());
                CalculateSpawnForce(instantiate);
                Destroy(instantiate, maxLifeTime);

                yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            }
        }

        private Vector3 CalculatePosition()
        {
            Vector3 position = new Vector3();
            position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
            position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);
            return position;
        }

        private Quaternion CalculateRotation()
        {
            return Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));
        }

        private void CalculateSpawnForce(GameObject objectSpawned)
        {
            float force = Random.Range(minForce, maxForce);
            objectSpawned.GetComponent<Rigidbody>().AddForce(objectSpawned.transform.up * force, ForceMode.Impulse);
        }

        public void SetSpawner(float bombChance, float maxSpawnDelay, float minSpawnDelay, bool increaseDifficult, float timePerChange, float maxBombChance, float increaseBombChanceValue)
        {
            this.bombChance = bombChance;
            this.maxSpawnDelay = maxSpawnDelay;
            this.minSpawnDelay = minSpawnDelay;
            this.increaseDifficult = increaseDifficult;
            this.timePerChange = timePerChange;
            this.maxBombChance = maxBombChance;
            this.increaseBombChanceValue = increaseBombChanceValue;
        }
    }
}