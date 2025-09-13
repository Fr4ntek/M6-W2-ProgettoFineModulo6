using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RotaryObstacle : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 90;
    [SerializeField] private float _pushForce = 10;
    [SerializeField] private float _upwardForce = 5f;

    void Update()
    {
        transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Rigidbody rb = collision.collider.attachedRigidbody;
            Vector3 toPlayer = collision.transform.position - transform.position;

            // Spinta laterale simulata dalla rotazione
            Vector3 tangent = Vector3.Cross(Vector3.up, toPlayer.normalized);
            Vector3 force = tangent.normalized * _pushForce + Vector3.up * _upwardForce;
            //rb.AddForce(force, ForceMode.Impulse);
            rb.velocity = force;
            // disabilito per mezzo secondo il movimento cosi da fargli avere la spinta altrimenti resettavo tutto al frame successivo
            collision.collider.GetComponent<PlayerController>().StartCoroutine("TemporarilyDisableMovement");
        }
    }
}
