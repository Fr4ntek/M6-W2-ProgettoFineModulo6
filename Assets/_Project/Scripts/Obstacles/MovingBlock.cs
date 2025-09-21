using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class MovingBlock : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private AIState currentState;

    [Header("Common Settings")]
    [SerializeField] private Transform _head;
    [SerializeField] private float _viewAngle = 45f;
    [SerializeField] private float _sightDistance = 15f;
    [SerializeField] private LayerMask _whatIsObstacle;
    [SerializeField] private int _subdivisions = 12;

    private Vector3 _startPosition;
    private int _currentWayPointIndex = 0;
    private Transform _target;
    private NavMeshAgent _agent;
    private LineRenderer _lineRenderer;

    void Start()
    {
        _startPosition = transform.position;

        if (_target == null)
        {
            _target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        _agent = GetComponent<NavMeshAgent>();
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        if (_lineRenderer != null)
        {
            _lineRenderer.startColor = Color.white;
            _lineRenderer.endColor = Color.white;
        }

        EvaluateConeOfView(_subdivisions);
        ChangeState(AIState.Idle);
    }
    void Update()
    {
        switch (currentState)
        {
            case AIState.Idle:
                IdleState();
                break;
            case AIState.Chasing:
                ChaseState();
                break;
            case AIState.ReturningToPost:
                ReturnToPostState();
                break;
        }

        if (currentState != AIState.Chasing && CanSeePlayer())
        {
            _lineRenderer.startColor = Color.red;
            _lineRenderer.endColor = Color.red;
            ChangeState(AIState.Chasing);
        }
    }

    private void ChangeState(AIState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case AIState.Idle:
                PerformPatrol();
                break;

            case AIState.Chasing:
                _agent.isStopped = false;
                break;

            case AIState.ReturningToPost:
                _agent.isStopped = false;
                _agent.SetDestination(_startPosition);
                break;
        }
    }

    private void IdleState()
    {
       PerformPatrol();
    }

    private void ChaseState()
    {
        _agent.SetDestination(_target.position);

        if (!CanSeePlayer())
        {
            _lineRenderer.startColor = Color.white;
            _lineRenderer.endColor = Color.white;
            ChangeState(AIState.ReturningToPost);
        }
    }

    private void ReturnToPostState()
    {
        if (!_agent.pathPending && _agent.remainingDistance < 0.2f)
        {
            ChangeState(AIState.Idle);
        }
    }

    private void PerformPatrol()
    {
        if (_waypoints.Length == 0) return;

        if (!_agent.pathPending && _agent.remainingDistance < 0.2f)
        {
            _currentWayPointIndex = (_currentWayPointIndex + 1) % _waypoints.Length;
            _agent.SetDestination(_waypoints[_currentWayPointIndex].position);
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 toTarget = _target.position - transform.position;
        float sqrDistance = toTarget.sqrMagnitude;

        if (sqrDistance > _sightDistance * _sightDistance)
            return false;

        float distance = Mathf.Sqrt(sqrDistance);
        toTarget /= distance;

        if (Vector3.Dot(transform.forward, toTarget) < Mathf.Cos(_viewAngle * Mathf.Deg2Rad))
            return false;

        if (Physics.Linecast(_head.position, _target.position, _whatIsObstacle))
            return false;

        return true;
    }

    public void EvaluateConeOfView(int subdivisions)
    {
        float startAngle = (90 - _viewAngle) * Mathf.Deg2Rad;

        int totalPoints = subdivisions + 1;
        _lineRenderer.positionCount = totalPoints;

        Vector3[] arcPoints = new Vector3[totalPoints];

        float stepAngle = (2 * _viewAngle / subdivisions) * Mathf.Deg2Rad;

        for (int i = 0; i < subdivisions; i++)
        {
            float currentAngle = startAngle + i * stepAngle;

            arcPoints[i].x = Mathf.Cos(currentAngle) * _sightDistance;
            arcPoints[i].z = Mathf.Sin(currentAngle) * _sightDistance;
        }

        arcPoints[subdivisions] = Vector3.zero;

        _lineRenderer.SetPositions(arcPoints);
    }
}
