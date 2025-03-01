using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picker : MonoBehaviour
{
        public bool canPickup;
        [SerializeField] LayerMask itemsLayer;
        public InventorySlots slots;
        void OnTriggerEnter(Collider other)
        {
                if (LayerCompare.IsInLayerMask(itemsLayer, other.gameObject.layer))
                {
                        IPickable item = other.gameObject.GetComponent<IPickable>();
                        if (item != null)
                        {
                                item.AttemptPickup(this);
                        }
                }
        }

        public bool PickItem(Item item)
        {
                return slots.AddItem(item.Data);
        }
}
