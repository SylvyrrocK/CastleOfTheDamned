using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnEnable : MonoBehaviour
{
        public float destroyDelay;
        float destroyDelayCurrent;
        bool startCountdown = false;
        void Awake()
        {
                destroyDelayCurrent = destroyDelay;
        }
        void FixedUpdate()
        {
                if (startCountdown)
                {
                        destroyDelayCurrent -= Time.fixedDeltaTime;
                }
                if (destroyDelayCurrent <= 0f)
                {
                        Destroy(gameObject);
                }
        }

        void OnEnable()
        {
                startCountdown = true;
        }
}
