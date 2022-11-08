using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

namespace Entities
{
    public class Bomb : MonoBehaviour, IObject
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) GameManager.Instance.Lose();
        }

        public GameObject GetGO()
        {
            return gameObject;
        }
    }
}