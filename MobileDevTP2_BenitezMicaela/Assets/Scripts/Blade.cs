using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public Vector3 direction { get; private set; }
    public float minSliceVelocity = 0.01f;

    private Camera mainCamera = null;
    private Collider bladeCollider = null;
    private bool slicing = false;

    private void Awake()
    {
        mainCamera = Camera.main;
        bladeCollider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        StopSlicing();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) StartSlicing();
        else if (Input.GetMouseButtonUp(0)) StopSlicing();
        else if (slicing) ContinueSlicing();
    }

    private void OnDisable()
    {
        StopSlicing();
    }

    private void StartSlicing()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;
        transform.position = newPosition;

        slicing = true;
        bladeCollider.enabled = true;
    }

    private void StopSlicing()
    {
        slicing = false;
        bladeCollider.enabled = false;
    }

    private void ContinueSlicing()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;

        direction = newPosition - transform.position;

        float velocity = direction.magnitude / Time.deltaTime;
        bladeCollider.enabled = velocity > minSliceVelocity;

        transform.position = newPosition;
    }
}