using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
        public static TutorialManager Instance;
        TutorialManager()
        {
                Instance = this;
        }
        [SerializeField] GameObject tutorialDefaultObj;
        static bool itemDrag = false;
        [SerializeField] GameObject itemDragObj;
        static bool itemUse = false;
        [SerializeField] GameObject itemUseObj;
        static bool itemUseInventory = false;
        [SerializeField] GameObject itemUseInventoryObj;
        static bool itemSpinInventory = false;
        [SerializeField] GameObject itemSpinInventoryObj;
        static bool backpack = false;
        [SerializeField] GameObject backpackObj;
        public void ShowDefaultTutorial(bool show)
        {
                tutorialDefaultObj.SetActive(true);
        }

        public void ShowDragTutorial(bool show)
        {
                if (itemDragObj == null)
                {
                        return;
                }
                if (show)
                {
                        if (!itemDrag)
                        {
                                itemDragObj.SetActive(true);
                        }
                        itemDrag = true;
                }
                else
                {
                        itemDragObj.SetActive(false);
                }
        }

        public void ShowUseTutorial(bool show)
        {
                if (itemUseObj == null)
                {
                        return;
                }
                if (show)
                {
                        if (!itemUse)
                        {
                                itemUseObj.SetActive(true);
                        }
                        itemUse = true;
                }
                else
                {
                        itemUseObj.SetActive(false);
                }
        }

        public void ShowInventoryUseTutorial(bool show)
        {
                if (itemUseInventoryObj == null)
                {
                        return;
                }
                if (show)
                {
                        if (!itemUseInventory)
                        {
                                itemUseInventoryObj.SetActive(true);
                        }
                        itemUseInventory = true;
                }
                else
                {
                        itemUseInventoryObj.SetActive(false);
                }
        }

        public void ShowSpinUseTutorial(bool show)
        {
                if (itemSpinInventoryObj == null)
                {
                        return;
                }
                if (show)
                {
                        if (!itemSpinInventory)
                        {
                                itemSpinInventoryObj.SetActive(true);
                        }
                        itemSpinInventory = true;
                }
                else
                {
                        itemSpinInventoryObj.SetActive(false);
                }
        }

        public void ShowBackpackTutorial(bool show)
        {
                if (backpackObj == null)
                {
                        return;
                }
                if (show)
                {
                        if (!backpack)
                        {
                                backpackObj.SetActive(true);
                        }
                        backpack = true;
                }
                else
                {
                        backpackObj.SetActive(false);
                }
        }
}
