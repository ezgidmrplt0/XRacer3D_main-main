using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OyunYoneticisi : MonoBehaviour
{
    public void OyunuYenidenBaslat()
    {
        Time.timeScale = 1f; // Zaman� normale d�nd�r
        SceneManager.LoadScene("GameScene"); // Oyun sahnesini yeniden y�kle
    }

    public void AnaMenuyeDon()
    {
        Time.timeScale = 1f; // Zaman� normale d�nd�r
        SceneManager.LoadScene("AnaMenu"); // Ana men� sahnesine d�n
    }
}
