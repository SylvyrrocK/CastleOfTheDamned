using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static PhysUIReposition;

public class PhysUIReposition : MonoBehaviour
{
    public GameObject createElementsFolder;
    private Camera cameraUI;
    [Serializable]
    public class ElementUI
    {
        public GameObject obj;
        public int depth = 1;
        public Vector2 pivot; // float in percent 0 -> 1
        public Vector2 offset; // float
    }
    [SerializeField]
    ElementUI[] spawnElementUIArray;
    public List<ElementUI> activeElementArray = new List<ElementUI>();
    private Vector2 resCameraUI = new Vector2(); // camera resolution



    void Start()
    {
        cameraUI = GetComponent<Camera>();
        resCameraUI.Set(cameraUI.pixelWidth, cameraUI.pixelHeight);
        //Debug.Log($"Camera: {resCameraUI}");
        CreateUI();
        UpdateUI();
    }
    private bool ResolutionChanged() // return true, if Resolution has been Changed
    {
        Vector2 newCamRes = new Vector2(cameraUI.pixelWidth, cameraUI.pixelHeight);
        if (!newCamRes.Equals(resCameraUI))
        {
            resCameraUI = newCamRes;
            //Debug.Log($"Camera new: {newCamRes}");
            return true;
        }
        return false;
    }
    private void CreateUI()
    {
        resCameraUI.Set(cameraUI.pixelWidth, cameraUI.pixelHeight); // camera resolution
        foreach (var _elementUI in spawnElementUIArray)
        {
            ElementUI tempElementUI = _elementUI; // element, to change the position of the created object in the UI
            Vector2 objScreenPos2 = tempElementUI.pivot * resCameraUI + tempElementUI.offset;// the position of the object
                                                                                       // in the camera's coordinate system
            Vector3 objScreenPos3 = new Vector3(objScreenPos2.x, objScreenPos2.y, tempElementUI.depth); // add depth
            tempElementUI.obj = Instantiate(tempElementUI.obj, cameraUI.ScreenToWorldPoint(objScreenPos3),
                Quaternion.identity, createElementsFolder.transform); // spawn objects
            activeElementArray.Add(tempElementUI);
            //Debug.Log($"objScreenPos3 in create: {objScreenPos3}");
        }
    }
    private void UpdateUI()
    {
        foreach (var _elementUI in activeElementArray)
        {
            Vector2 objScreenPos2 = _elementUI.pivot * resCameraUI + _elementUI.offset;// the position of the object
                                                                                     // in the camera's coordinate system
            Vector3 objScreenPos3 = new Vector3(objScreenPos2.x, objScreenPos2.y, _elementUI.depth); // add depth
            _elementUI.obj.transform.position = cameraUI.ScreenToWorldPoint(objScreenPos3);
            //Debug.Log($"objScreenPos3 in update: {objScreenPos3}");

        }
    }


    // Update is called once per frame
    void Update()
    {
        if (ResolutionChanged())
        {
            UpdateUI();
        }
    }
}
