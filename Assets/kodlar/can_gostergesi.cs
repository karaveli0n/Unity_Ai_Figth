using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class can_gostergesi : MonoBehaviour
{
    public Image can_bari;
    public GameObject cam;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        cam=GameObject.Find("Main Camera");
    }

    public void can_bar_metodu(float can, float tam_can)
    {
        can_bari.fillAmount = can / tam_can;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
