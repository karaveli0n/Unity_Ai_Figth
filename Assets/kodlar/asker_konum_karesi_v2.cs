using UnityEngine; // Unity motoru ile ilgili sınıfları içerir
using UnityEngine.EventSystems; // Olay sistemi ile ilgili sınıfları içerir

public class asker_konum_karesi_v2 : MonoBehaviour, IPointerDownHandler // IPointerDownHandler arayüzünü uygulayan bir MonoBehaviour sınıfı tanımlar
{
    public RectTransform panel; // Panel referansı, UI paneli
    public GameObject konumKaresiPrefab; // Konum karesi prefabı referansı
    public GameObject askerPrefab_0, askerPrefab_1, secilen_asker; // Asker prefabı referansı
    public Transform terrain; // Terrain objesi referansı
    public float asker_cesidi, konumKaresiSizeRatio = 0.01f; // Panelin genişliğine göre konum karesinin boyut oranı (0.01 = %1)

    void Start() // Script ilk çalıştığında çağrılan metot
    {
        // Gerekli bileşenlerin atandığından emin ol
        if (panel == null || konumKaresiPrefab == null || askerPrefab_0 == null || askerPrefab_1 == null || terrain == null)
        {
            Debug.LogError("Gerekli referanslar atanmadı!"); // Referanslar atanmadıysa hata mesajı göster
        }
    }

    public void OnPointerDown(PointerEventData eventData) // Mouse tıklaması algılandığında çağrılan metot
    {
        // Tıklanılan noktayı belirle
        Vector2 localPoint;
        // Tıklanılan ekran noktasını panelin lokal noktasına çevir
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panel, eventData.position, eventData.pressEventCamera, out localPoint);

        // Tıklanan noktada daha önce konum karesi olup olmadığını kontrol et
        foreach (Transform child in panel)
        {
            RectTransform childRect = child.GetComponent<RectTransform>(); // Çocuğun RectTransform bileşenini al
            if (childRect != null && RectTransformUtility.RectangleContainsScreenPoint(childRect, eventData.position, eventData.pressEventCamera))
            {
                return; // Tıklanan noktada zaten bir konum karesi var, metottan çık
            }
        }

        // Yeni bir konum karesi prefabı oluştur
        GameObject newKonumKaresi = Instantiate(konumKaresiPrefab, panel); // Yeni konum karesi oluştur ve panelin çocuğu yap
        RectTransform konumKaresiRect = newKonumKaresi.GetComponent<RectTransform>(); // Yeni konum karesinin RectTransform bileşenini al

        // Konum karesinin boyutunu ayarla
        float size = Mathf.Min(panel.rect.width, panel.rect.height) * konumKaresiSizeRatio; // Panelin boyutuna göre konum karesi boyutunu hesapla
        konumKaresiRect.sizeDelta = new Vector2(size, size); // Konum karesinin boyutunu ayarla

        // Prefabı tıklanılan noktaya taşı
        konumKaresiRect.anchoredPosition = localPoint; // Konum karesini tıklanılan noktaya taşı

        // Prefabın konumuna göre asker spawn et
        SpawnAsker(localPoint); // Tıklanılan noktaya göre asker spawn et
    }

    public void asker_tipi_secimi_0()
    {
        secilen_asker = askerPrefab_0;
    }
    public void asker_tipi_secimi_1()
    {
        secilen_asker = askerPrefab_1;
    }

    private void SpawnAsker(Vector2 konumKaresiPosition) // Askeri spawn eden metot
    {
        // Panel üzerindeki konumu dünya konumuna çevir
        Vector3 panelSize = panel.rect.size; // Panelin boyutlarını al
        Vector3 terrainSize = terrain.GetComponent<Collider>().bounds.size; // Terrain boyutlarını al
        Vector3 terrainPosition = terrain.position; // Terrain pozisyonunu al

        // Panel üzerindeki konumu terrain üzerindeki konuma ölçekle
        float xRatio = (konumKaresiPosition.x / panelSize.x) + 0.5f; // Panel konumunu normalize et [0,1] aralığında
        float yRatio = (konumKaresiPosition.y / panelSize.y) + 0.5f; // Panel konumunu normalize et [0,1] aralığında

        Vector3 worldPosition = new Vector3( // Dünya pozisyonunu hesapla
            terrainPosition.x + (xRatio * terrainSize.x) - (terrainSize.x / 2), // X pozisyonu
            terrainPosition.y, // Y pozisyonu
            terrainPosition.z + (yRatio * terrainSize.z) - (terrainSize.z / 2) // Z pozisyonu
        );

        // Askeri spawn et, terrain sınırları içinde olduğundan emin ol
        if (worldPosition.x >= terrainPosition.x - terrainSize.x / 2 && // Dünya pozisyonunun terrain sınırları içinde olduğunu kontrol et
            worldPosition.x <= terrainPosition.x + terrainSize.x / 2 &&
            worldPosition.z >= terrainPosition.z - terrainSize.z / 2 &&
            worldPosition.z <= terrainPosition.z + terrainSize.z / 2)
        {
            Instantiate(secilen_asker, worldPosition, Quaternion.identity); // Asker prefabını dünya pozisyonunda oluştur
        }
        else
        {
            Debug.LogWarning("Terrain sınırları dışında bir konum tespit edildi."); // Sınır dışı pozisyon için uyarı göster
        }
    }
}