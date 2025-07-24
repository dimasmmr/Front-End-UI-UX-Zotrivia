using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrlInstagram : MonoBehaviour
{
    // Start is called before the first frame update
    public void OpenurlIg()
    {
        Application.OpenURL("https://www.instagram.com/zootrivia_official/?utm_source=ig_web_button_share_sheet");
        Debug.Log("Masuk Link");
    }
}
