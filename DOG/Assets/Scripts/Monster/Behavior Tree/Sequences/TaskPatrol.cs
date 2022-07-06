using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskPatrol : Node
{
    private Transform _transform = null;
    private Transform[] _waypoints = null;

    private int currentWaypointIndex = 0;

    private bool waiting = false;
    private float waitCounter = 0.0f;

    public float waitTime = 3.0f;

    public TaskPatrol(Transform transform, Transform[] waypoints)
    {
        _transform = transform;
        _waypoints = waypoints;
    }

    public override NodeState Evaluate()
    {
        if (waiting)
        {
            waitCounter += Time.deltaTime;
            if (waitCounter > waitTime)
            {
                waiting = false;
            }
        }

        Transform wp = _waypoints[currentWaypointIndex];
        if(Vector2.SqrMagnitude(_transform.position - wp.position) < 0.01f)
        {
            Debug.Log($"{wp.name} µµÂø");
            _transform.position = wp.position;
            waitCounter = 0.0f;
            waiting = true;

            currentWaypointIndex = (currentWaypointIndex + 1) % _waypoints.Length;
        }
        else
        {
            _transform.position = Vector3.MoveTowards(_transform.position, wp.position, Monsters_BT.moveSpeed * Time.fixedDeltaTime);
            _transform.LookAt(wp.position);
        }

        state = NodeState.RUNNING;
        return state;
    }
}
