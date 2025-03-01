using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysUIDraggableSlot : MonoBehaviour
{
        public InventorySlots slotOwner;

        void OnTriggerEnter2D(Collider2D other)
        {
                var item2d = other.gameObject.GetComponent<Item>();
                if (item2d == null)
                {
                        return;
                }

                var slotn = slotOwner.GetDraggableSlot(this);
                if (slotn != null)
                {
                        var slot = (int)slotn;
                        var itemData = slotOwner.GetItem(slot);
                        if (itemData == null)
                        {
                                slotOwner.AddItem(item2d.Data, slot);
                                Destroy(other.gameObject);
                        }
                        else
                        {
                                return;
                        }
                }
        }
}
