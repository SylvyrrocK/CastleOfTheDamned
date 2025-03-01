using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlots : MonoBehaviour
{
        public Player player;
        public ItemData[] slots;
        public PhysUIDraggableSlot[] draggableSlots;
        public SpriteRenderer[] slotTextures;
        public Transform[] handTransforms;
        GameObject[] equipedItems;
        [SerializeField] Material fistMaterial;
        [SerializeField] Material handMaterial;
        [SerializeField] MeshRenderer[] hands;
        void Awake()
        {
                slots = new ItemData[draggableSlots.Length];
                equipedItems = new GameObject[slots.Length];
        }
        void OnEnable()
        {
                UpdateSlots();
        }

        void UpdateSlots()
        {
                for (int i = 0; i < slots.Length; i++)
                {
                        if (slots[i] == null)
                        {
                                slotTextures[i].sprite = null;
                                DeEquipSlot(i);
                        }
                        else
                        {
                                Texture tex = ItemFactory.GetItem(slots[i].itemName).inventoryIcon;
                                Sprite sprite = Sprite.Create(ItemFactory.GetItem(slots[i].itemName).inventoryIcon, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 32);
                                slotTextures[i].sprite = sprite;
                                EquipSlot(i);
                        }
                }
        }

        void FixedUpdate()
        {
                for (int i = 0; i < equipedItems.Length; i++)
                {
                        if (equipedItems[i] != null && slots[i] != null)
                        {
                                slots[i] = equipedItems[i].GetComponent<Item>().Data;
                        }
                }
        }
        void EquipSlot(int slot)
        {
                DeEquipSlot(slot);
                equipedItems[slot] = ItemFactory.SpawnItem(slots[slot], handTransforms[slot].position, handTransforms[slot].rotation * Quaternion.Euler(-90f, 90f, 0f), handTransforms[slot]);
                Rigidbody rb = equipedItems[slot].GetComponent<Rigidbody>();
                Item3D item = rb.GetComponent<Item3D>();
                item.isPickable = false;
                item.spriteRotation.disable = true;
                rb.isKinematic = true;
                hands[slot].material = fistMaterial;
        }

        void DeEquipSlot(int slot)
        {
                Destroy(equipedItems[slot]);
                equipedItems[slot] = null;
                hands[slot].material = handMaterial;
        }

        public bool AddItem(ItemData itemData, int slot)
        {
                if (TutorialManager.Instance != null)
                {
                        TutorialManager.Instance.ShowDragTutorial(true);
                }
                if (TutorialManager.Instance != null)
                {
                        TutorialManager.Instance.ShowUseTutorial(true);
                }

                if (slots[slot] == null)
                {
                        slots[slot] = itemData;
                        UpdateSlots();
                        return true;
                }
                else
                {
                        return false;
                }
        }

        public bool AddItem(ItemData itemData)
        {
                if (TutorialManager.Instance != null)
                {
                        TutorialManager.Instance.ShowDragTutorial(true);
                }
                if (TutorialManager.Instance != null)
                {
                        TutorialManager.Instance.ShowUseTutorial(true);
                }

                for (int i = 0; i < slots.Length; i++)
                {
                        if (slots[i] == null)
                        {
                                slots[i] = itemData;
                                UpdateSlots();
                                return true;
                        }
                }
                return false;
        }

        public void RemoveItem(int slot)
        {
                if (TutorialManager.Instance != null)
                {
                        TutorialManager.Instance.ShowDragTutorial(false);
                }

                slots[slot] = null;
                UpdateSlots();
        }

        public ItemData RetrieveItem(int slot)
        {
                if (TutorialManager.Instance != null)
                {
                        TutorialManager.Instance.ShowDragTutorial(false);
                }

                ItemData itemData = slots[slot];

                if (itemData == null)
                {
                        //Debug.LogWarning("Trying to retrieve from an empty slot");
                }

                slots[slot] = null;

                UpdateSlots();
                return itemData;
        }

        public ItemData GetItem(int slot)
        {
                return slots[slot];
        }

        public PhysUIDraggableSlot GetDraggableSlot(int slot)
        {
                return draggableSlots[slot];
        }

        public int? GetDraggableSlot(PhysUIDraggableSlot draggableSlot)
        {
                for (int i = 0; i < draggableSlots.Length; i++)
                {
                        if (draggableSlots[i] == draggableSlot)
                        {
                                return i;
                        }
                }
                return null;
        }

        public GameObject GetEquipedItem(int slot)
        {
                return equipedItems[slot];
        }

        public int? GetEquipedItemSlot(GameObject item)
        {
                for (int i = 0; i < equipedItems.Length; i++)
                {
                        if (equipedItems[i] == item)
                        {
                                return i;
                        }
                }
                return null;
        }

        public void UseItem(int slot)
        {
                if (equipedItems[slot] != null)
                {
                        equipedItems[slot].GetComponent<Item>().OnUse(player);
                }
                if (TutorialManager.Instance != null)
                {
                        TutorialManager.Instance.ShowUseTutorial(false);
                }
        }

        public void CancelUseItem(int slot)
        {
                if (equipedItems[slot] != null)
                {
                        equipedItems[slot].GetComponent<Item>().OnUseCancel(player);
                }
        }

        public void OnPrimary(bool isDown, bool isFreeCursor)
        {
                if (isFreeCursor)
                {
                        return;
                }
                if (isDown)
                {
                        UseItem(0);
                }
                else
                {
                        CancelUseItem(0);
                }
        }

        public void OnSecondary(bool isDown, bool isFreeCursor)
        {
                if (isFreeCursor)
                {
                        return;
                }
                if (isDown)
                {
                        UseItem(1);
                }
                else
                {
                        CancelUseItem(1);
                }
        }
}
