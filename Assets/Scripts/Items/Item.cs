using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
        public abstract ItemData Data { get; set; }
        public abstract void OnUse(Player player);
        public abstract void OnUseCancel(Player player);
}
