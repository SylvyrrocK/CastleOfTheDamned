using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleItem2D : Item2D
{
        [SerializeField] AppleItemData appleItemData;
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

        }
        public override void OnUseCancel(Player player)
        {
                
        }
}
