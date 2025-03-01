using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item3D : Item, IPickable
{
        public bool isPickable;
        public float pickupTimer = 0f;
        public SpriteRotate spriteRotation;
        public virtual void AttemptPickup(Picker picker)
        {
                if (isPickable && pickupTimer <= 0f)
                {
                        if (picker.PickItem(this))
                        {
                                Destroy(this.gameObject);
                        }
                }
        }

        public virtual void FixedUpdate()
        {
                if (pickupTimer > 0f)
                {
                        pickupTimer -= Time.fixedDeltaTime;
                }
        }
}
