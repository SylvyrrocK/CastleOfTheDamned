using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public string tooltipMessage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ToastManager.Instance.ShowToast(tooltipMessage); 
        }
        //if (other.CompareTag("Player"))
        //{
        //    ToastManager.Instance.ShowImportantToast(tooltipMessage);
        //}
    }
}
