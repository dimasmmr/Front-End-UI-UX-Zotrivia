using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class SetObjek : MonoBehaviour
{
    // Start is called before the first frame update
    public void ActivateObject()
    {
        gameObject.SetActive(true);
        AudioManager.instance.PlaySFX("button");
    }

    // Menonaktifkan GameObject ini
    public void DeactivateObject()
    {
        gameObject.SetActive(false);
        AudioManager.instance.PlaySFX("button");
    }

    // Mengaktifkan/Menonaktifkan GameObject ini berdasarkan status saat ini
    public void ToggleObject()
    {
        bool isActive = gameObject.activeSelf;
        gameObject.SetActive(!isActive);
    }
}
