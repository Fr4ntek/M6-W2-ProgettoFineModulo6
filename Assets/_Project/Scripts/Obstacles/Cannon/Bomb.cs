using UnityEngine;
using System.Collections;
using TMPro.EditorUtilities;

public class Bomb : MonoBehaviour
{
    [SerializeField] private int _explodeTimeMin = 1;
    [SerializeField] private int _explodeTimeMax = 10;
    [SerializeField] private ParticleSystem _explosionParticles;
    [SerializeField] private AudioSource _explosionSound;

    private BombPool _pool;
    private Rigidbody _rb;
    private Renderer _renderer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _explosionSound = GetComponent<AudioSource>();
        _explosionParticles = GetComponent<ParticleSystem>();
        _renderer = GetComponent<Renderer>();

    }

    public void Initialize(BombPool poolRef)
    {
        _pool = poolRef;
        StartCoroutine(ExplodeCoroutine());
    }

    private IEnumerator ExplodeCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(_explodeTimeMin, _explodeTimeMax));

        _explosionSound.Play();
        _explosionParticles.Play();
        
        // ho dovuto perche il main duration non matchava con la durata effettiva del particle
        // la bomba restava visibile anche dopo la fine del PS
        _renderer.enabled = false;

        // Aspetta durata particle system
        yield return new WaitForSeconds(_explosionParticles.main.duration);

        // Reset fisica
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        // Ritorna nel pool
        _pool.ReturnBomb(gameObject);
    }
}
