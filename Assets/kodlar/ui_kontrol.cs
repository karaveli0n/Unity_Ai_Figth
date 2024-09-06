using UnityEngine;
using UnityEngine.UI;

public class ui_kontrol : MonoBehaviour
{
    public GameObject panel; // Panel referansı
    public GameObject buton_01;
    public Text playerCountText; // Player sayısı gösterecek Text referansı
    public Text enemyCountText;  // Enemy sayısı gösterecek Text referansı

    private void Update()
    {
        int playerCount = GameObject.FindGameObjectsWithTag("Player").Length;
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        playerCountText.text = "Players: " + playerCount;
        enemyCountText.text = "Enemies: " + enemyCount;
    }

    // Paneli açan metot
    public void OpenPanel()
    {
        panel.SetActive(true);
        buton_01.SetActive(true);
        Debug.Log("Panel açıldı!");
    }

    // Paneli kapatan metot
    public void ClosePanel()
    {
        panel.SetActive(false);
        buton_01.SetActive(false);
        Debug.Log("Panel kapatıldı!");
    }
}
