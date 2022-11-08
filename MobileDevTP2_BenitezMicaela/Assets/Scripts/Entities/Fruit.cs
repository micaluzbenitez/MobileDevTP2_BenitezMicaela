using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

namespace Entities
{
    public class Fruit : MonoBehaviour, IObject
    {
        [Header("Fruit components")]
        public GameObject whole = null;
        public GameObject sliced = null;
        public ParticleSystem juice = null;
        public ParticleSystem splash = null;

        [Header("Fruit data")]
        public int points = 1;

        private Rigidbody fruitRigidbody = null;
        private Collider fruitCollider = null;

        private void Awake()
        {
            fruitRigidbody = GetComponent<Rigidbody>();
            fruitCollider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Blade blade = other.GetComponent<Blade>();
                if (blade) Slice(blade.direction, blade.transform.position, blade.sliceForce);
            }
        }

        private void OnDestroy()
        {
            if (whole.activeSelf) GameManager.Instance.LoseLife();
        }

        private void Slice(Vector3 direction, Vector3 position, float force)
        {
            GameManager.Instance.UpdateScore(points);

            whole.SetActive(false);
            sliced.SetActive(true);

            fruitCollider.enabled = false;
            juice.Play();
            splash.Play();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody slice in slices)
            {
                slice.velocity = fruitRigidbody.velocity;
                slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
            }
        }

        public GameObject GetGO()
        {
            return gameObject;
        }
    }
}