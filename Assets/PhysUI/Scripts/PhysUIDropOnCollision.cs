using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysUIDropOnCollision : MonoBehaviour
{
        public bool deleteNonItems = false;
        public Transform dropPosition;
        public Transform itemsHierarchyFolder;

        void OnCollisionEnter2D(Collision2D collision)
        {
                var item2d = collision.gameObject.GetComponent<Item>();
                if (item2d != null)
                {
                        if (item2d.Data != null)
                        {
                                GameObject newItemObj = ItemFactory.SpawnItem(item2d.Data, dropPosition.position, Quaternion.identity, itemsHierarchyFolder);
                                var item3d = newItemObj.GetComponent<Item3D>();
                                item3d.pickupTimer = ItemFactory.Instance.defaultPickupTimer;
                                Destroy(collision.gameObject);
                        }
                }

                if (deleteNonItems)
                {
                        Destroy(collision.gameObject);
                }
                return;
        }
}
