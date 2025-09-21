using UnityEngine;

public class RollingSpikeBlock : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint; // punto in alto sulla rampa
    [SerializeField] private int _damage = 50;
    [SerializeField] private float damageCooldown = 1f;

    private float lastHitTime;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpikeBlockResetPoint"))
        {
            // Reset posizione e velocità
            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastHitTime >= damageCooldown)
            {
                collision.gameObject.GetComponent<LifeController>().TakeDamage(_damage);
                lastHitTime = Time.time;
            }
        }
    }
}
