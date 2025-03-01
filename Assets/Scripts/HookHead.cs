using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HookHead : MonoBehaviour
{
        public Transform owner;

        [SerializeField] float travelDistance;
        [SerializeField] float traveled = 0f;
        [SerializeField] float travelSpeed;
        [SerializeField] Vector3 travelDirection;
        [SerializeField] LayerMask canHookLayers;
        [SerializeField] LayerMask obstructionLayers;
        public UnityEvent onHookEnd;
        Transform hookedObject;
        public GameObject hookLink;

        void FixedUpdate()
        {
                if (owner == null)
                {
                        Destroy(hookLink);
                        Destroy(gameObject);
                        return;
                }
                if (traveled < travelDistance)
                {
                        Vector3 movement = transform.rotation * travelDirection.normalized * travelSpeed * Time.fixedDeltaTime;
                        transform.position = transform.position + movement;
                        traveled += movement.magnitude;
                }
                else
                {
                        Vector3 movement = (owner.position - transform.position).normalized * travelSpeed * Time.fixedDeltaTime;
                        transform.position = transform.position + movement;

                        if ((owner.position - transform.position).magnitude <= travelSpeed * Time.fixedDeltaTime)
                        {
                                transform.position = owner.position;

                                MoveHookedObject();

                                ReleaseObject();

                                onHookEnd.Invoke();
                        }
                }

                MoveHookedObject();
        }

        void HookObject(Transform target)
        {
                if (hookedObject == null)
                {
                        var slot = GameManager.Instance.player.inventorySlots.GetEquipedItemSlot(target.gameObject);
                        if (slot == null)
                        {
                                hookedObject = target;
                                traveled = Mathf.Infinity;
                        }
                }
        }

        void MoveHookedObject()
        {
                if (hookedObject == null)
                {
                        return;
                }

                hookedObject.position = transform.position;
        }

        void ReleaseObject()
        {
                if (hookedObject != null)
                {
                        hookedObject = null;
                }
        }
        void OnTriggerEnter(Collider other)
        {
                if (LayerCompare.IsInLayerMask(other.gameObject, canHookLayers))
                {
                        HookObject(other.transform);
                }
                else if (LayerCompare.IsInLayerMask(other.gameObject, obstructionLayers))
                {
                        traveled = Mathf.Infinity;
                        ReleaseObject();
                }
        }
}
