using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Transform[] _positions;
    
    private Vector3 _targetPos;
    private bool _isMoving = false;

    private void Start()
    {
        ChooseRandomPosition();
    }
    void FixedUpdate()
    {
        if (_positions == null) return;
        if(_targetPos == null) return;

        if (_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, _speed * Time.fixedDeltaTime);
        }
        // Appena arrivo scelgo un altro punto
        if (Vector3.Distance(transform.position, _targetPos) < 0.1f)
        {
            ChooseRandomPosition();
        }
    }

    private void ChooseRandomPosition()
    {
         _targetPos = _positions[Random.Range(0, _positions.Length)].position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _isMoving = true;
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.transform.SetParent(null);
    }
}
