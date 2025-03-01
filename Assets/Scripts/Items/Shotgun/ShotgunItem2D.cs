using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunItem2D : Item2D
{
        public ShotgunItemData shotgunItemData;
        [SerializeField] Sprite loadedBarrelSprite;
        [SerializeField] Sprite unloadedBarrelSprite;
        [SerializeField] SpriteRenderer barrelRenderer;
        public override ItemData Data
        {
                get { return shotgunItemData; }
                set
                {
                        shotgunItemData = value is ShotgunItemData ? (ShotgunItemData)value : throw new System.Exception("Trying to set ItemData of a wrong type");
                }
        }

        public override void OnUse(Player player)
        {

        }

        public override void OnUseCancel(Player player)
        {

        }

        public void FixedUpdate()
        {
                if (barrelRenderer == null)
                {
                        return;
                }
                if (shotgunItemData.ammo > 0)
                {
                        barrelRenderer.sprite = loadedBarrelSprite;
                }
                else
                {
                        barrelRenderer.sprite = unloadedBarrelSprite;
                }
        }
}
