using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGemItem : GemItem
{
        [SerializeField] float healAmount;
        public override void OnUse(Player player)
        {
                player.health.Heal(healAmount);
                var slot = player.inventorySlots.GetEquipedItemSlot(this.gameObject);
                if (slot != null)
                {
                        player.inventorySlots.RemoveItem((int)slot);
                } else
                {
                        Destroy(gameObject);
                }

        }

        public override void OnUseCancel(Player player)
        {

        }
}
