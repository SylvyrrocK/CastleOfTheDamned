using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


[Serializable]
public abstract class ItemData
{
        public string itemName;

        public bool isEmpty()
        {
                if (itemName == "" || itemName == null)
                {
                        return true;
                } else
                {
                        return false;
                }
        }
}
