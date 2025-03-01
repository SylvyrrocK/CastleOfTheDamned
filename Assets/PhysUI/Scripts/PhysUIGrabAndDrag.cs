using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhysUIGrabAndDrag : MonoBehaviour
{
        public Player player;
        public Rigidbody2D draggedObjectRb;
        float draggedObjectRotation;
        public LayerMask physUILayer;
        public PlayerController playerController; // THIS IS NO NEEDED> CAN BE OPTIMISED. FIXTHIS
        public float dragSpeed;
        public float rotateSpeed;
        public float rotateStep;
        public float breakForce;
        Camera cam;
        Vector2 mouseWorldPos;
        PhysUIDraggableSlot draggableSlot;
        public float dragDistanceToPick;
        Vector2 draggableSlotDragStartPos;
        void Start()
        {
                cam = GetComponent<Camera>();
        }

        void FixedUpdate()
        {
                if (!playerController.isFreeCursor)
                {
                        ReleaseDraggedObjects();
                }

                if (draggableSlot)
                {
                        Vector2 distance = mouseWorldPos - new Vector2(draggableSlotDragStartPos.x, draggableSlotDragStartPos.y);
                        if (distance.magnitude > dragDistanceToPick)
                        {
                                int? slot = draggableSlot.slotOwner.GetDraggableSlot(draggableSlot);
                                if (slot != null)
                                {
                                        ItemData itemData = draggableSlot.slotOwner.RetrieveItem((int)slot);
                                        if (itemData != null)
                                        {
                                                GameObject item = ItemFactory.SpawnItem2D(itemData, mouseWorldPos, Quaternion.identity, transform.parent);
                                                var rb = item.GetComponent<Rigidbody2D>();
                                                draggedObjectRb = rb;
                                                draggedObjectRotation = rb.rotation;
                                                if (TutorialManager.Instance != null)
                                                {
                                                        TutorialManager.Instance.ShowInventoryUseTutorial(true);
                                                        TutorialManager.Instance.ShowSpinUseTutorial(true);
                                                        TutorialManager.Instance.ShowBackpackTutorial(true);
                                                }
                                        }
                                        draggableSlot = null;
                                }
                        }
                }

                if (draggedObjectRb)
                {
                        Vector2 velocity = mouseWorldPos - draggedObjectRb.position;
                        if (velocity.magnitude > breakForce)
                        {
                                ReleaseDraggedObjects();
                        }
                        else
                        {
                                draggedObjectRb.velocity = velocity * dragSpeed;
                                draggedObjectRb.angularVelocity = (draggedObjectRotation - draggedObjectRb.rotation) * rotateSpeed;
                        }
                }
        }

        public void OnMousePosition(Vector2 mouseScreenPos)
        {
                mouseWorldPos = cam.ScreenToWorldPoint(mouseScreenPos);
        }

        public void OnPrimary(bool isDown, bool isFreeCursor)
        {
                if (isDown)
                {
                        if (!isFreeCursor)
                        {
                                return;
                        }

                        var collider = Physics2D.OverlapPoint(mouseWorldPos, physUILayer);

                        if (collider == null)
                        {
                                return;
                        }

                        var rb = collider.gameObject.GetComponent<Rigidbody2D>();
                        if (rb != null)
                        {
                                draggedObjectRb = rb;
                                draggedObjectRotation = rb.rotation;
                                return;
                        }

                        var dS = collider.gameObject.GetComponent<PhysUIDraggableSlot>();
                        if (dS != null)
                        {
                                draggableSlot = dS;
                                draggableSlotDragStartPos = mouseWorldPos;
                        }
                }
                else
                {
                        ReleaseDraggedObjects();
                }
        }

        public void OnSecondary(bool isDown, bool isFreeCursor)
        {
                if (draggedObjectRb == null)
                {
                        if (isDown)
                        {
                                var collider = Physics2D.OverlapPoint(mouseWorldPos, physUILayer);
                                if (collider != null)
                                {
                                        var freeitem = collider.gameObject.GetComponent<Item>();
                                        if (freeitem != null)
                                        {
                                                freeitem.OnUse(player);
                                                freeitem.OnUseCancel(player);
                                        }
                                        return;
                                }
                        }
                        return;
                }
                var item = draggedObjectRb.gameObject.GetComponent<Item>();
                if (item == null)
                {
                        return;
                }
                if (isDown)
                {
                        item.OnUse(player);
                }
                else
                {
                        item.OnUseCancel(player);
                }
        }

        public void OnMouseWheel(float input)
        {
                if (input > 0f)
                {
                        draggedObjectRotation += rotateStep;
                }
                else if (input < 0f)
                {
                        draggedObjectRotation -= rotateStep;
                }
        }

        void ReleaseDraggedObjects()
        {
                if (draggedObjectRb != null)
                {
                        draggedObjectRb = null;
                }
                if (draggableSlot != null)
                {
                        draggableSlot = null;
                }
        }
}
