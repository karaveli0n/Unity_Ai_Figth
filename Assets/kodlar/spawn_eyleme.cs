using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_eyleme : MonoBehaviour
{
    public GameObject objectPrefab; // Spawn edilecek objenin prefabı
    public float spawnInterval = 1f; // Spawn aralığı (saniye)
    public int maxObjects = 10; // Maksimum spawn edilecek obje sayısı
    public Terrain terrain; // Terrain referansı

    private int spawnedObjects = 0; // Spawn edilen obje sayısı

    private void Start()
    {
        // Belirli aralıklarla spawn işlemini başlatan coroutine
        StartCoroutine(SpawnObjectsRoutine());
    }

    private IEnumerator SpawnObjectsRoutine()
    {
        while (spawnedObjects < maxObjects)
        {
            // Belirli bir süre bekleyelim
            yield return new WaitForSeconds(spawnInterval);

            // Rastgele bir pozisyon seçelim
            Vector3 spawnPosition = GetRandomPosition();

            // Objeyi spawn edelim
            Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

            // Spawn edilen obje sayısını bir artıralım
            spawnedObjects++;
        }
    }

    private Vector3 GetRandomPosition()
    {
        // Terrain boyutunu alalım
        Vector3 terrainSize = terrain.terrainData.size;

        // Terrain'in dünya konumunu alalım
        Vector3 terrainPosition = terrain.transform.position;

        // Rastgele bir x ve z koordinatı seçelim
        float randomX = Random.Range(terrainPosition.x, terrainPosition.x + terrainSize.x);
        float randomZ = Random.Range(terrainPosition.z, terrainPosition.z + terrainSize.z);

        // Y koordinatını terrain üzerindeki yüksekliği alarak ayarlayalım
        float y = Terrain.activeTerrain.SampleHeight(new Vector3(randomX, 0f, randomZ));

        // Spawn pozisyonunu döndürelim
        return new Vector3(randomX, y, randomZ);
    }
}
