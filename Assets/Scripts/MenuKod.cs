using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuKod : MonoBehaviour
{
    public GameObject marketPanel; // Market Panelini buraya atayacaðýz

    public void PlayGame()
    {
        // Oyun sahnesini baþlat
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void OpenMarket()
    {
        // Market panelini aktif yap
        marketPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Oyun kapandý! (Çalýþtýrýrken Unity Editör'de kapanmaz.)");
    }
}
