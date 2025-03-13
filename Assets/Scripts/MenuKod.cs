using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuKod : MonoBehaviour
{
    public GameObject marketPanel; // Market Panelini buraya atayaca��z

    public void PlayGame()
    {
        // Oyun sahnesini ba�lat
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
        Debug.Log("Oyun kapand�! (�al��t�r�rken Unity Edit�r'de kapanmaz.)");
    }
}
