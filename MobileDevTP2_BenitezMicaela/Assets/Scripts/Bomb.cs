using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class Bomb : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) GameManager.Instance.Explode();
    }
}