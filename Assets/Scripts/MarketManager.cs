using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketManager : MonoBehaviour
{
    public GameObject marketPanel; // Market panelini buraya atayacağız
    public GameObject[] ucaklar;
    public Button[] satinAlButonlari;
    public TextMeshProUGUI altinText;

    private int altin;

    void Start()
    {
        altin = PlayerPrefs.GetInt("Altin", 0);
        GuncelleAltinUI();

        for (int i = 0; i < ucaklar.Length; i++)
        {
            int index = i;
            bool satinAlindi = PlayerPrefs.GetInt("Ucak" + index, 0) == 1;

            if (satinAlindi)
            {
                satinAlButonlari[i].GetComponentInChildren<TextMeshProUGUI>().text = "SEÇ";
            }

            satinAlButonlari[i].onClick.AddListener(() => UcakSatinAlOrSec(index));
        }
    }

    public void UcakSatinAlOrSec(int index)
    {
        bool satinAlindi = PlayerPrefs.GetInt("Ucak" + index, 0) == 1;

        if (satinAlindi)
        {
            PlayerPrefs.SetInt("SeciliUcak", index);
            Debug.Log("Uçak seçildi: Uçak" + index);
        }
        else
        {
            int fiyat = 100;
            if (altin >= fiyat)
            {
                altin -= fiyat;
                PlayerPrefs.SetInt("Ucak" + index, 1);
                PlayerPrefs.SetInt("SeciliUcak", index);
                GuncelleAltinUI();
                satinAlButonlari[index].GetComponentInChildren<TextMeshProUGUI>().text = "SEÇ";
                Debug.Log("Uçak satın alındı ve seçildi: Uçak" + index);
            }
            else
            {
                Debug.Log("Yeterli altın yok!");
            }
        }
    }

    void GuncelleAltinUI()
    {
        altinText.text = "Altın: " + altin;
        PlayerPrefs.SetInt("Altin", altin);
    }

    public void GeriDon()
    {
        // Market panelini kapat
        marketPanel.SetActive(false);
        Debug.Log("Ana menüye dönüldü");
    }
}
