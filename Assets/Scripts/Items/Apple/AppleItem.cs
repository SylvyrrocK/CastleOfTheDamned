using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleItem : Item3D
{
        [SerializeField] AppleItemData appleItemData;
        [SerializeField] float transformRange;
        [SerializeField] float transformChance;
        [SerializeField] float eatHeal;
        [SerializeField] float eatDamage;
        bool transformInRadius = false;
        public override ItemData Data
        {
                get { return appleItemData; }
                set
                {
                        appleItemData = value is AppleItemData ? (AppleItemData)value : throw new System.Exception("Trying to set ItemData of a wrong type");
                }
        }
        public override void OnUse(Player player)
        {
                EatApple(player);
        }
        public override void OnUseCancel(Player player)
        {

        }

        public override void FixedUpdate()
        {
                base.FixedUpdate();

                var slot = GameManager.Instance.player.inventorySlots.GetEquipedItemSlot(this.gameObject);
                if (slot == null)
                {
                        if ((GameManager.Instance.player.transform.position - transform.position).magnitude <= transformRange)
                        {
                                if (!transformInRadius)
                                {
                                        float randomFloat = Random.Range(0f, 1f);
                                        if (randomFloat <= transformChance)
                                        {
                                                TransformApple();
                                        }
                                }
                                transformInRadius = true;
                        }
                        else
                        {
                                transformInRadius = false;
                        }
                }
        }

        public void TransformApple()
        {
                Debug.Log("Transforming Apple");
                var slot = GameManager.Instance.player.inventorySlots.GetEquipedItemSlot(this.gameObject);
                if (slot != null)
                {
                        GameManager.Instance.player.inventorySlots.RemoveItem((int)slot);
                }
                else
                {
                        Destroy(gameObject);
                }
        }

        void EatApple(Player player)
        {
                float randomFloat = Random.Range(0f, 1f);
                if (randomFloat <= transformChance)
                {
                        player.health.Damage(eatDamage);
                        TransformApple();
                }
                else
                {
                        player.health.Heal(eatHeal);
                }

                var slot = GameManager.Instance.player.inventorySlots.GetEquipedItemSlot(this.gameObject);
                if (slot != null)
                {
                        appleItemData.uses -= 1;
                        if (appleItemData.uses <= 0)
                        {
                                player.inventorySlots.RemoveItem((int)slot);
                        }
                }
        }
}
