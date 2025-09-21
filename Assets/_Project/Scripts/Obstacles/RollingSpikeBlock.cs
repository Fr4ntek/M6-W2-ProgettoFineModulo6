using UnityEngine;

public class RollingSpikeBlock : MonoBehaviour
{
    [SerializeField] private Transform _respawnPoint; // punto in alto sulla rampa
    [SerializeField] private int _damage = 50;
    [SerializeField] private float _damageCooldown = 1f;

    private float _lastHitTime;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpikeBlockResetPoint"))
        {
            // Reset posizione e velocità
            transform.position = _respawnPoint.position;
            transform.rotation = _respawnPoint.rotation;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - _lastHitTime >= _damageCooldown)
            {
                collision.gameObject.GetComponent<LifeController>().TakeDamage(_damage);
                _lastHitTime = Time.time;
            }
        }
    }
}
