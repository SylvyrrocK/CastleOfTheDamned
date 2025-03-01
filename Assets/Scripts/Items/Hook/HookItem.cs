using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookItem : Item3D
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
        [SerializeField] Hook hook;

        public override void OnUse(Player player)
        {
                if (hook != null)
                {
                        hook.CastHook(transform.rotation * -Vector3.right);
                }
        }

        public override void OnUseCancel(Player player)
        {
                if (hook != null)
                {
                        hook.CancelHook();
                }
        }
}
