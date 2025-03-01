using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAI : MonoBehaviour
{
        public EnemyHealth health;
        public NavMeshAgent nmAgent;
        protected Player player;
        public LayerMask sightLayers;
}
