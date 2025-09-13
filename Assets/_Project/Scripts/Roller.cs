using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 90f;
    [SerializeField] private float _pushForce = 5f;

    void Update()
    {
        transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.rigidbody;
            if (rb == null) return;

            Vector3 centerToPlayer = collision.transform.position   - transform.position;
            Vector3 tangent = Vector3.Cross(Vector3.down, centerToPlayer).normalized;

            rb.velocity += tangent * _pushForce * Time.deltaTime;
        }
    }
}
