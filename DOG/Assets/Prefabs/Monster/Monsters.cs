using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monsters : MonoBehaviour
{
    private NavMeshAgent navigator = null;
    public Transform player = null;

    public enum CurrentState { idle, traverse, track, attack, dead };
    public CurrentState status = CurrentState.idle;

    private bool isDead = false;

    public float healthPoint = 100.0f;
    public int strength = 5;

    private void Awake()
    {
        navigator = GetComponent<NavMeshAgent>();
    }

    IEnumerator CheckStatus()
    {
        while (!isDead)
        {
            if (status == CurrentState.idle)
            {
                navigator.isStopped = true;
                //idle animation
            }
            else if (status == CurrentState.traverse)
            {

            }
            else if (status == CurrentState.track)
            {
                navigator.isStopped = false;
                //move animation
                navigator.destination = player.transform.position;

            }
            else if (status == CurrentState.attack)
            {

            }
            else if (status == CurrentState.dead)
            {

            }

            yield return null;
        }
    }
    
}
