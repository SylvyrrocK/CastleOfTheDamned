using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem2D : SimpleItem
{
        [SerializeField] float floatDuration;
        [SerializeField] float floatDurationDev;
        float floatDurationCurrent;
        [SerializeField] float floatInterval;
        [SerializeField] float floatIntervalDev;
        float floatIntervalCurrent;
        bool isFloating = false;
        Rigidbody2D rb;
        [SerializeField] Vector2 randomVelocity;
        [SerializeField] float floatForce;
        void Start()
        {
                floatIntervalCurrent = Mathf.Clamp(floatInterval + Random.Range(-floatIntervalDev, floatIntervalDev), 0f, Mathf.Infinity);
                rb = gameObject.GetComponent<Rigidbody2D>();
        }

        public override void FixedUpdate()
        {
                base.FixedUpdate();

                floatIntervalCurrent -= Time.fixedDeltaTime;
                floatDurationCurrent -= Time.fixedDeltaTime;
                if (!isFloating)
                {
                        if (floatIntervalCurrent <= 0f)
                        {
                                isFloating = true;
                                floatDurationCurrent = Mathf.Clamp(floatDuration + Random.Range(-floatDurationDev, floatDurationDev), 0f, Mathf.Infinity);
                        }
                        rb.gravityScale = 1f;
                }
                else
                {
                        if (floatDurationCurrent <= 0f)
                        {
                                isFloating = false;
                                floatIntervalCurrent = Mathf.Clamp(floatInterval + Random.Range(-floatIntervalDev, floatIntervalDev), 0f, Mathf.Infinity);
                        }
                        rb.gravityScale = 0f;
                        var randomVector = new Vector2(Random.Range(-randomVelocity.x, randomVelocity.x), Random.Range(-randomVelocity.y, randomVelocity.y));
                        rb.AddForce(randomVector * Time.fixedDeltaTime);
                        rb.AddForce(new Vector2(0f, floatForce) * Time.fixedDeltaTime);
                }
        }
}
