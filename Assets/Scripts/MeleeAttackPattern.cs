using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MeleeAttackPattern : MonoBehaviour
{
        public UnityEvent<GameObject> onAttackLand;
        public float attackTime;
        float attackCounter;
        bool attackFinished = false;
        void Start()
        {
                attackCounter = attackTime;
        }

        void FixedUpdate()
        {
                attackCounter -= Time.fixedDeltaTime;
                if (attackCounter <= 0f)
                {
                        EndAttack();
                }
        }

        void EndAttack()
        {
                Destroy(gameObject);
        }

        void OnTriggerEnter(Collider other)
        {
                if (!attackFinished)
                {
                        onAttackLand.Invoke(other.gameObject);
                        attackFinished = true;
                }
        }
}
