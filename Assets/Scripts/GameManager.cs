using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
        public Player player;
        public static GameManager Instance;
        public bool hasAllRelics = false;
        public bool hadAllRelics = false;
        public GameObject physUI;
        GameManager()
        {
                Instance = this;
        }

        void FixedUpdate()
        {
                var relics = physUI.GetComponentsInChildren<Relic>();
                if (relics.Length >= 3)
                {
                        hasAllRelics = true;
                } else
                {
                        hasAllRelics = false;
                }

                if (hasAllRelics && !hadAllRelics)
                {
                        hadAllRelics = true;
                        Debug.Log("I dont feel safe");
                }
        }
}
