using UnityEngine;
using UnityEngine.UI;

public class asker_konum_karesi : MonoBehaviour
{
    public RectTransform panel; // Panel referansı
    public GameObject konumKaresiPrefab; // Prefab referansı
    public int prefabCount = 500; // Başlangıç prefab sayısı

    private void Start()
    {
        CreateAndArrangePrefabs();
    }

    private void CreateAndArrangePrefabs()
    {
        // Panel boyutlarını al
        float panelWidth = panel.rect.width;
        float panelHeight = panel.rect.height;

        // Prefab sayısını satır ve sütun olarak hesapla
        int rowCount = Mathf.CeilToInt(Mathf.Sqrt(prefabCount * (panelHeight / panelWidth)));
        int colCount = Mathf.CeilToInt((float)prefabCount / rowCount);

        // Prefab sayısını paneli tam dolduracak şekilde ayarla
        while ((rowCount * colCount) < prefabCount || (panelWidth / colCount * panelHeight / rowCount) < prefabCount)
        {
            prefabCount++;
            rowCount = Mathf.CeilToInt(Mathf.Sqrt(prefabCount * (panelHeight / panelWidth)));
            colCount = Mathf.CeilToInt((float)prefabCount / rowCount);
        }

        // Prefab boyutlarını hesapla
        float prefabWidth = panelWidth / colCount;
        float prefabHeight = panelHeight / rowCount;

        // Prefabları oluştur ve yerleştir
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {
                if (i * colCount + j >= prefabCount) break;

                // Prefab oluştur
                GameObject newPrefab = Instantiate(konumKaresiPrefab, panel);

                // Prefab boyutunu ayarla
                RectTransform rt = newPrefab.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(prefabWidth, prefabHeight);

                // Prefab konumunu ayarla
                rt.anchorMin = new Vector2(j / (float)colCount, 1 - (i + 1) / (float)rowCount);
                rt.anchorMax = new Vector2((j + 1) / (float)colCount, 1 - i / (float)rowCount);
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;

                // Prefab adını ayarla
                newPrefab.name = "KonumKaresi_" + (i * colCount + j + 1);

                // Prefab üzerine sayı ekle
                Text textComponent = newPrefab.GetComponentInChildren<Text>();
                if (textComponent != null)
                {
                    textComponent.text = (i * colCount + j + 1).ToString();
                }
                else
                {
                    Debug.LogWarning("Prefab içinde Text bileşeni bulunamadı!");
                }
            }
        }
    }
}
