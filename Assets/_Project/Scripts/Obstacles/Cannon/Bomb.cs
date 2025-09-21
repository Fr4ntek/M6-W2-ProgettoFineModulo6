using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _explodeTime = 5f;
    [SerializeField] private ParticleSystem _explosionParticles;
    [SerializeField] private AudioSource _explosionSound;

    private BombPool _pool;
    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Initialize(BombPool poolRef)
    {
        _pool = poolRef;
        StartCoroutine(ExplodeCoroutine());
    }

    private IEnumerator ExplodeCoroutine()
    {
        yield return new WaitForSeconds(_explodeTime);

        // Particles + suono
        //if (_explosionParticles) _explosionParticles.Play();
        //if (_explosionSound) _explosionSound.Play();

        // Aspetta durata particle system
        //float waitTime = _explosionParticles ? _explosionParticles.main.duration : 0.5f;
        yield return new WaitForSeconds(2);

        // Reset fisica
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        // Ritorna nel pool
        _pool.ReturnBomb(gameObject);
    }
}
