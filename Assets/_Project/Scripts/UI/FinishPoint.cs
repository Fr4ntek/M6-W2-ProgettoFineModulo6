using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIController uiController = other.GetComponent<UIController>();
            if (uiController != null) uiController.CheckCoins();
        }
    }
}
