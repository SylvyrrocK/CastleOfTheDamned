using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
        [SerializeField] GameObject hookHeadPrefab;
        [SerializeField] GameObject hookLinkPrefab;
        GameObject hookHead;
        GameObject hookLink;
        [SerializeField] LayerMask hookObstructiontLayers;
        [SerializeField] Material hookLinkMaterial;
        MeshRenderer hookLinkMeshRenderer;
        [Tooltip("Default vector 0f, 1f, 0f")]
        [SerializeField] Vector3 rotationCorrection = new Vector3(0f, 1f, 0f);
        [SerializeField] bool rotationRelative = false;

        void Update()
        {
                if (hookHead != null)
                {
                        Vector3 difference = transform.position - hookHead.transform.position;

                        bool hit = Physics.Raycast(transform.position, -difference.normalized, difference.magnitude, hookObstructiontLayers);
                        if (hit)
                        {
                                CancelHook();
                        }
                        else
                        {
                                Vector3 rotCor;
                                if (rotationRelative)
                                {
                                        rotCor = Vector3.Scale(rotationCorrection, transform.lossyScale);
                                }
                                else
                                {
                                        rotCor = rotationCorrection;
                                }
                                Quaternion rotation = Quaternion.LookRotation(difference.normalized, rotCor);

                                hookLink.transform.position = (transform.position + hookHead.transform.position) * 0.5f;
                                hookLink.transform.rotation = rotation;

                                hookLink.transform.localScale = new Vector3(hookLink.transform.localScale.x, hookLink.transform.localScale.y, difference.magnitude);

                                hookLinkMeshRenderer.material.mainTextureScale = new Vector2(1f, difference.magnitude);
                        }
                }
        }

        public void CastHook(Vector3 direction)
        {
                if (hookHead != null)
                {
                        return;
                }

                Vector3 rotCor;
                if (rotationRelative)
                {
                        rotCor = Vector3.Scale(rotationCorrection, transform.lossyScale);
                }
                else
                {
                        rotCor = rotationCorrection;
                }
                Quaternion rotation = Quaternion.LookRotation(direction, rotCor);

                hookLink = Instantiate(hookLinkPrefab, transform.position, Quaternion.identity);
                hookLinkMeshRenderer = hookLink.GetComponentInChildren<MeshRenderer>();
                hookLinkMeshRenderer.material = hookLinkMaterial;

                hookHead = Instantiate(hookHeadPrefab, transform.position + direction * 0.001f, rotation);
                var headScript = hookHead.GetComponent<HookHead>();
                headScript.owner = transform;
                headScript.hookLink = hookLink;
                headScript.onHookEnd.AddListener(CancelHook);
        }

        public void CancelHook()
        {
                Destroy(hookHead);
                Destroy(hookLink);
        }
}
