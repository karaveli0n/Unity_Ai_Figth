using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mermi : MonoBehaviour
{
    public int mermi_hasari;
    private float zamanSayaci = 0f;
    public string ad;
    void Start()
    {
        // Mermi oluşturulduktan 7 saniye sonra yok olmasını sağla
        Destroy(this.gameObject, 7f);
    }
   
    void Update()
    {
        zamanSayaci += Time.deltaTime;
    }
    public void veri_al(string firlatan_adi)
    {
        ad = firlatan_adi;
    }

    void OnTriggerEnter(Collider other)
    {
        if(zamanSayaci>=0.1f&&other.gameObject.name != ad && (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy"))
        {
            other.gameObject.GetComponent<HW_Yapay_Zeka>().hasar_al(mermi_hasari);
            Debug.Log( ad + "Bu hedefi vurdu" + other.gameObject.name );
            Destroy(this.gameObject);
        }
    }
}
