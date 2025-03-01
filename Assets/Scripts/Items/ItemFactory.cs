using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
        [SerializeField] public float defaultPickupTimer = 2f;
        [SerializeField] public Transform defaultSpriteRotator;
        public static ItemFactory Instance;
        ItemFactory()
        {
                Instance = this;
        }

        [SerializeField] ItemType[] itemTypesList;

        [Serializable]
        public class ItemType
        {
                [SerializeField] public string itemType;
                [SerializeField] public ItemPrefab[] itemsList;
        }

        [Serializable]
        public class ItemPrefab
        {
                [SerializeField] public string itemName;
                [SerializeField] public GameObject prefab;
                [SerializeField] public GameObject prefab2D;
                [SerializeField] public Texture2D inventoryIcon;
        }

        public static ItemPrefab GetItem(string itemName)
        {
                if (Instance == null)
                {
                        throw new Exception("No ItemFactory in scene!");
                }
                if (itemName == "")
                {
                        throw new Exception("ItemPrefab: GetItem: Empty argument");
                }
                foreach (var type in Instance.itemTypesList)
                {
                        foreach (var itemPrefab in type.itemsList)
                        {
                                if (itemPrefab.itemName == itemName)
                                {
                                        return itemPrefab;
                                }
                        }
                }
                throw new Exception("Could not find item " + itemName);
        }

        public static GameObject SpawnItem(ItemData item, Vector3 position, Quaternion rotation, Transform parent)
        {
                ItemPrefab itemEntry = GetItem(item.itemName);
                var spawnedObject = Instantiate(itemEntry.prefab, position, rotation, parent);
                spawnedObject.GetComponent<Item>().Data = item;
                return spawnedObject;
        }

        public static GameObject SpawnItem(ItemData item, Vector3 position, Quaternion rotation)
        {
                ItemPrefab itemEntry = GetItem(item.itemName);
                var spawnedObject = Instantiate(itemEntry.prefab, position, rotation);
                spawnedObject.GetComponent<Item>().Data = item;
                return spawnedObject;
        }

        public static GameObject SpawnItem2D(ItemData item, Vector3 position, Quaternion rotation)
        {
                ItemPrefab itemEntry = GetItem(item.itemName);
                var spawnedObject = Instantiate(itemEntry.prefab2D, position, rotation);
                spawnedObject.GetComponent<Item>().Data = item;
                return spawnedObject;
        }

        public static GameObject SpawnItem2D(ItemData item, Vector3 position, Quaternion rotation, Transform parent)
        {
                ItemPrefab itemEntry = GetItem(item.itemName);
                var spawnedObject = Instantiate(itemEntry.prefab2D, position, rotation, parent);
                spawnedObject.GetComponent<Item>().Data = item;
                return spawnedObject;
        }

        public static GameObject SpawnNewItem(string itemName, Vector3 position, Quaternion rotation)
        {
                ItemPrefab itemEntry = GetItem(itemName);
                var spawnedObject = Instantiate(itemEntry.prefab, position, rotation);
                return spawnedObject;
        }

}
