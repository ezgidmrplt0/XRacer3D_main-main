using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OyunYoneticisi : MonoBehaviour
{
    public void OyunuYenidenBaslat()
    {
        Time.timeScale = 1f; // Zamaný normale döndür
        SceneManager.LoadScene("GameScene"); // Oyun sahnesini yeniden yükle
    }

    public void AnaMenuyeDon()
    {
        Time.timeScale = 1f; // Zamaný normale döndür
        SceneManager.LoadScene("AnaMenu"); // Ana menü sahnesine dön
    }
}
