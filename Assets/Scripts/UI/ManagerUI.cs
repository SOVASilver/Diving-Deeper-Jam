using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject Inv;
   public void ToggleInventory()
    {
        if (Inv.activeSelf)
        {
            Inv.SetActive(false);
        }
        else
        {
            Inv.SetActive(true);
        }
    }
}