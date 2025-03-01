using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
        public float maxHealth;
        public float health;
        public float regeneration;
        public UnityEvent<float> onDamage;
        public UnityEvent<float> onHeal;
        public UnityEvent onDeath;
        bool isDead = false;
        protected virtual void Awake()
        {
                health = maxHealth;
        }
        protected virtual void FixedUpdate() 
        {
                health = Mathf.Clamp(health + regeneration * Time.fixedDeltaTime, 0f, maxHealth);
        }

        public virtual void Damage(float damage)
        {
                if (damage <= 0f)
                {
                        return;
                }
                health -= damage;
                onDamage.Invoke(damage);
                if (health <= 0f)
                {
                        if (!isDead)
                        {
                                isDead = true;
                                Die();
                        }
                }
        }

        public virtual void Heal(float heal)
        {
                if (heal <= 0f)
                {
                        return;
                }
                health = Mathf.Clamp(health + heal, 0f, maxHealth);
                onHeal.Invoke(heal);
        }

        public virtual void Die()
        {
                onDeath.Invoke();
                Destroy(gameObject);
        }
}
