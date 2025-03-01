using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeItem : Item3D
{
        [SerializeField] SimpleItemData simpleItemData;
        public override ItemData Data
        {
                get { return simpleItemData; }
                set
                {
                        simpleItemData = value is SimpleItemData ? (SimpleItemData)value : throw new System.Exception("Trying to set ItemData of a wrong type");
                }
        }
        [SerializeField] Melee melee;

        public override void OnUse(Player player)
        {
                if (melee != null)
                {
                        melee.OnMelee(transform.rotation * -Vector3.right);
                }
        }

        public override void OnUseCancel(Player player)
        {

        }
}
