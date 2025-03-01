using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PhysUIKineticForce : MonoBehaviour
{
        // Start is called before the first frame update
        public bool enableShake = true;
        public Transform cameraTransorm;

        public bool enableShakeOfCameraRotation = true;
        public float delayCheckChildsPhysUI = 0;
        public float shakeStrength = 100;
        public float shakeStrengthXDueRotation = 1;
        public float shakeStrengthYDueRotation = 1;

        public bool enableShakeOfCameraMove = true;
        public float shakeStrengthXOfMove = 1;
        public float shakeStrengthYOfMove = 50;

        public bool enableShakeOfCameraSteps = true;
        public float multiplierShakeFrequency = 1;
        public float multiplierShakeStrength = 1;

        public bool gravityControlOfCameraRotation = true;
        public float StrengthYDueRotation = 1;

        private Rigidbody2D[] childrenRb;
        private float delay;
        private float cameraDeltaCounter = 0;
        private Quaternion camRotation;
        private Vector3 cameraPosition;
        private Vector3 cameraDeltaRotation;
        private Vector3 cameraDeltaPosition;
        private Vector3 cameraDeltaPositionXZ;


        Vector3 CameraDeltaRotation()
        {
                Quaternion camRotationNow = cameraTransorm.rotation;
                Quaternion camDelta = camRotationNow * Quaternion.Inverse(camRotation);
                camRotation = camRotationNow;
                return VectorUnfuck(camDelta.eulerAngles);
        }

        Vector3 CameraDeltaPosition()
        {
                Vector3 cameraPositionNow = cameraTransorm.position;
                Vector3 cameraDeltaPosition = cameraPositionNow - cameraPosition;
                cameraPosition = cameraPositionNow;
                //Debug.Log($"cameraDeltaPosition: {cameraDeltaPosition}, pos1:{cameraPosition}");
                return cameraDeltaPosition;
        }
        void CalcHorizontalMoveVecForPush() // MB in future. because fucking math
        {
                // levo -, pravo +, ot osi Zov
                float camRotVert = cameraTransorm.rotation.eulerAngles.y;
                if (camRotVert > 180) camRotVert -= 360;
                if (camRotVert < -180) camRotVert += 360;
                Debug.Log(camRotVert);
                //float sa = Mathf.Sin(Mathf.PI * 0.33f);
                //Debug.Log($"sin: {sa}");
        }

        Vector3 VectorUnfuck(Vector3 vec)
        {
                float x = vec.x;
                float y = vec.y;
                float z = vec.z;
                if (x > 180) x -= 360;
                if (y > 180) y -= 360;
                if (z > 180) z -= 360;
                if (x < -180) x += 360;
                if (y < -180) y += 360;
                if (z < -180) z += 360;
                return new Vector3(y, x, z);
        }
        Vector2 VectorUnfuck(Vector2 vec)
        {
                float x = vec.x;
                float y = vec.y;
                if (x > 180) x -= 360;
                if (y > 180) y -= 360;
                if (x < -180) x += 360;
                if (y < -180) y += 360;
                return new Vector3(y, x);
        }

        void PushAllRigidBodies()
        {
                float cameraRotationY = VectorUnfuck(cameraTransorm.rotation.eulerAngles).y;
                float fraction = 1 - Mathf.Abs(cameraRotationY) / 90;
                float sinPushWalk = Mathf.Clamp(Mathf.Sin(cameraDeltaCounter / multiplierShakeFrequency), 0f, 1f) * multiplierShakeStrength;
                cameraDeltaCounter += sinPushWalk;
                foreach (var rb in childrenRb)
                {

                        Vector2 tempV2 = new Vector2(-cameraDeltaRotation.x * shakeStrengthXDueRotation,
                            (cameraDeltaRotation.y - (cameraDeltaPosition.y * shakeStrengthYOfMove) + sinPushWalk) * shakeStrengthYDueRotation);
                        rb.AddForce(tempV2 * shakeStrength * Time.fixedDeltaTime);
                        rb.gravityScale = Mathf.Clamp(fraction, 0.5f, 1f);

                }
        }
        void PushAllRigidBodiesOfRotation()
        {
                foreach (var rb in childrenRb)
                {
                        Vector2 tempV2 = new Vector2(-cameraDeltaRotation.x * shakeStrengthXDueRotation,
                            cameraDeltaRotation.y * shakeStrengthYDueRotation);
                        rb.AddForce(tempV2 * shakeStrength * Time.fixedDeltaTime);
                }
        }
        void PushAllRigidBodiesOfSteps()
        {

                float sinPushWalk = Mathf.Clamp(Mathf.Sin(cameraDeltaCounter / multiplierShakeFrequency), 0f, 1f) * multiplierShakeStrength;
                cameraDeltaCounter += sinPushWalk;
                foreach (var rb in childrenRb)
                {
                        Vector2 tempV2 = new Vector2(0, sinPushWalk);
                        rb.AddForce(tempV2 * shakeStrength * Time.fixedDeltaTime);

                }
        }
        void PushAllRigidBodiesOfMove()
        {
                foreach (var rb in childrenRb)
                {
                        Vector2 tempV2 = new Vector2(0, -cameraDeltaPosition.y * shakeStrengthYOfMove);
                        rb.AddForce(tempV2 * shakeStrength * Time.fixedDeltaTime);
                }
        }


        void Start()
        {
                childrenRb = gameObject.GetComponentsInChildren<Rigidbody2D>();
                cameraPosition = cameraTransorm.position;
                camRotation = cameraTransorm.rotation;

                delay = delayCheckChildsPhysUI;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
                for (int i = 0; i < childrenRb.Length; i++)
                {
                        if (childrenRb[i] == null)
                        {
                                childrenRb = gameObject.GetComponentsInChildren<Rigidbody2D>();
                        }
                }
                if (!enableShake) { return; }
                cameraDeltaRotation = CameraDeltaRotation();
                cameraDeltaPosition = CameraDeltaPosition();

                cameraDeltaPositionXZ = new Vector3(cameraDeltaPosition.x, 0, cameraDeltaPosition.z);
                cameraDeltaCounter += cameraDeltaPositionXZ.magnitude;
                //Debug.Log($"delta: {cameraDelta}, now: {cameraTransorm.rotation}");


                delay -= Time.fixedDeltaTime;
                if (delay <= 0)
                {
                        delay = delayCheckChildsPhysUI;
                        childrenRb = gameObject.GetComponentsInChildren<Rigidbody2D>();
                }
                PushAllRigidBodiesOfRotation();
                PushAllRigidBodiesOfMove();
                PushAllRigidBodiesOfSteps();
                foreach (var rb in childrenRb)
                {
                        if (rb.gameObject.GetComponent<IgnorePhysUIGravity>() == null)
                        {
                                float fraction = 1 - Mathf.Abs(VectorUnfuck(cameraTransorm.rotation.eulerAngles).y) / 90;
                                rb.gravityScale = Mathf.Clamp(fraction, 0.5f, 1f);
                        }
                }
                //PushAllRigidBodies();
        }

}
