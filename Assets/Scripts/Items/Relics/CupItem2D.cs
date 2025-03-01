using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupItem2D : SimpleItem
{
        [SerializeField] float punchStrength;
        [SerializeField] float punchStrengthDev;
        [SerializeField] float punchDelay;
        [SerializeField] float punchDelayCurrent;
        [SerializeField] float punchDelayDev;

        void Start()
        {
                ResetCounter();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
                if (punchDelayCurrent > 0f)
                {
                        return;
                }
                Vector3 impulse = collision.relativeVelocity;
                var rb = collision.collider.gameObject.GetComponent<Rigidbody2D>();
                if (rb)
                {
                        float force = Mathf.Clamp(punchStrength + Random.Range(-punchStrengthDev, punchStrengthDev), 0f, Mathf.Infinity);
                        rb.AddForce(-impulse.normalized * force);
                        ResetCounter();
                }
        }

        public override void FixedUpdate()
        {
                base.FixedUpdate();
                punchDelayCurrent -= Time.fixedDeltaTime;
        }

        void ResetCounter()
        {
                punchDelayCurrent = Mathf.Clamp(punchDelay + Random.Range(-punchDelayDev, punchDelayDev), 0f, Mathf.Infinity);
        }
}
