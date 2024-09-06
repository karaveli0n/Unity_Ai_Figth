using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class kamera : MonoBehaviour
{
    
    public float moveSpeed = 10.0f; // Kamera hareket hızı
    public float lookSpeed = 2.0f; // Fare ile bakma hızı
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    public GameObject uiPanel; // Sağ tıklama ile aktif hale getirilecek panel
    private bool isCameraMode = false;
    public Terrain terrain;
    public GameObject objectToSpawn_0,objectToSpawn_1;
    public Slider positionSlider_0,positionSlider_1;
    public Text txt_0,txt_1;


void Update()
    {
        txt_0.text=positionSlider_0.value.ToString();
        txt_1.text=positionSlider_1.value.ToString();
        // Sol tıklama ile kamera modunu aç/kapat, ancak sadece mouse panel alanında değilse
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
        {
            isCameraMode = true;
            Cursor.lockState = CursorLockMode.Locked; // Mouse imlecini kilitle ve gizle
            Cursor.visible = false;
        }

        // Sağ tıklama ile kamera modunu kapat ve paneli aktif et
        if (Input.GetMouseButtonDown(1))
        {
            isCameraMode = false;
            Cursor.lockState = CursorLockMode.None; // Mouse imlecini serbest bırak ve göster
            Cursor.visible = true;

            if (uiPanel != null)
            {
                uiPanel.SetActive(true);
            }
        }

        // Kamera modunda yön tuşları ve fare hareketi ile ilgili olaylar
        if (isCameraMode)
        {
            // WASD tuşlarıyla hareket etme
            float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            transform.position += move;

            // Fare ile bakma
            yaw += lookSpeed * Input.GetAxis("Mouse X");
            pitch -= lookSpeed * Input.GetAxis("Mouse Y");
            pitch = Mathf.Clamp(pitch, -90f, 90f); // Bakma açısını sınırlamak için

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }
    }

    // Fare imlecinin UI üzerinde olup olmadığını kontrol eden fonksiyon
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void mavi_spawn(float value)
    {
        value = positionSlider_0.value;
        // Slider değerini kullanarak Terrain üzerinde pozisyon hesapla
        float terrainWidth = terrain.terrainData.size.x;
        float terrainHeight = terrain.terrainData.size.z;
        
        // Slider değerini Terrain genişliğiyle orantılı hale getir
        float spawnX = value / positionSlider_0.maxValue * terrainWidth;
        float spawnZ = value / positionSlider_0.maxValue * terrainHeight;

        // Yükseklik değeri hesapla
        float spawnY = terrain.SampleHeight(new Vector3(spawnX, 0, spawnZ));

        // Yeni nesneyi oluştur
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);
        for (var i = 0; i < value; i++)
        {
            Instantiate(objectToSpawn_0, spawnPosition, Quaternion.identity); 
        }
    }

    public void kirmizi_spawn(float value)
    {
        value = positionSlider_1.value;
        // Slider değerini kullanarak Terrain üzerinde pozisyon hesapla
        float terrainWidth = terrain.terrainData.size.x;
        float terrainHeight = terrain.terrainData.size.z;
        
        // Slider değerini Terrain genişliğiyle orantılı hale getir
        float spawnX = value / positionSlider_1.maxValue * terrainWidth;
        float spawnZ = value / positionSlider_1.maxValue * terrainHeight;

        // Yükseklik değeri hesapla
        float spawnY = terrain.SampleHeight(new Vector3(spawnX, 0, spawnZ));

        // Yeni nesneyi oluştur
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);
         for (var i = 0; i < value; i++)
        {
            Instantiate(objectToSpawn_1, spawnPosition, Quaternion.identity);
        }
    }
}