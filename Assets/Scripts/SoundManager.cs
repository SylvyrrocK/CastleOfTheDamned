using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
        [SerializeField] GameObject soundPrefab3D;
        [SerializeField] GameObject soundPrefab2D;
        public static SoundManager Instance;
        SoundManager()
        {
                Instance = this;
        }
}
