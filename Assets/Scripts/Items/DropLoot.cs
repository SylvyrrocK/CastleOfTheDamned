using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour
{
        [SerializeField] float lootChance;
        [SerializeField] string[] lootNames;
        public void Drop()
        {
                float rand = Random.Range(0f, 1f);
                if (rand <= lootChance)
                {
                        int randomSlot = Random.Range(0, lootNames.Length);
                        ItemFactory.SpawnNewItem(lootNames[randomSlot], transform.position, Quaternion.identity);
                }
        }
}
