using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleItem : Item3D
{
        [SerializeField] protected SimpleItemData simpleItemData;
                public override ItemData Data
        {
                get { return simpleItemData; }
                set
                {
                        simpleItemData = value is SimpleItemData ? (SimpleItemData)value : throw new System.Exception("Trying to set ItemData of a wrong type");
                }
        }

        public override void OnUse(Player player)
        {

        }

        public override void OnUseCancel(Player player)
        {

        }
}
