using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleFollowAI : EnemyAI    
{
        public Transform target;
        public float distanceToTarget = 2f;
        NavMeshAgent agent;
        void Start()
        {
                agent = GetComponent<NavMeshAgent>();
        }
        void FixedUpdate()
        {
                agent.destination = target.position;
                if ((target.position - transform.position).magnitude < distanceToTarget)
                {
                        agent.destination = transform.position;
                }
        }
}
