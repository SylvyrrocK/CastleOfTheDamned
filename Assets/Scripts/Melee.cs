using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Melee : MonoBehaviour
{
        public GameObject attackPatternGameObject;
        GameObject currentMeleeAttack;
        public float damage;
        public float attackDelay;
        float attackDelayCounter;
        public bool parentAttackPattern = false;

        public void OnMelee(Vector3 direction)
        {
                if (attackDelayCounter >= 0f)
                {
                        return;
                }

                quaternion rotation = Quaternion.LookRotation(direction, new Vector3(0f, 1f, 0f));

                Transform parent = null;
                if (parentAttackPattern)
                {
                        parent = transform;
                }

                currentMeleeAttack = Instantiate(attackPatternGameObject, transform.position + direction, rotation, parent);
                currentMeleeAttack.GetComponent<MeleeAttackPattern>().onAttackLand.AddListener(OnSuccessfulAttack);

                attackDelayCounter = attackDelay;
        }

        public void OnSuccessfulAttack(GameObject targetObj)
        {
                var targetHealth = targetObj.GetComponent<Health>();
                if (targetHealth != null)
                {
                        Debug.Log(targetObj.name);
                        targetHealth.Damage(damage);
                }
        }

        void FixedUpdate()
        {
                attackDelayCounter -= Time.fixedDeltaTime;
        }
}
