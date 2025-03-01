using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpriteRotate : MonoBehaviour
{
        public Transform cam;
        public float rotation;
        public bool disable;
        void Start()
        {
                cam = ItemFactory.Instance.defaultSpriteRotator;
        }
        void Update()
        {
                if (!disable)
                {
                        Vector2 direction = new Vector2(cam.position.x - transform.position.x, cam.position.z - transform.position.z).normalized;
                        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, rotation, direction.y), Vector3.up);
                }
                else
                {
                        transform.localRotation = Quaternion.LookRotation(new Vector3(0f, rotation, 0f), Vector3.up);
                }

        }
}
