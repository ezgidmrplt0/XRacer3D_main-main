using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Ucak
{
    public string isim;              // Uçak adı
    public int fiyat;                // Altın fiyatı
    public GameObject ucakPrefab;    // Oyun için model
    [HideInInspector] public bool satinAlindi; // Satın alma durumu (kaydedilir)
}

public class MarketManager : MonoBehaviour
{
    [Header("Market Ayarları")]
    public List<Ucak> ucaklar;               // Inspector’dan ayarla
    public List<Button> satinAlButonlari;   // Her uçağa ait butonlar (UI)
    public TextMeshProUGUI altinText;       // UI’daki altın yazısı
    public GameObject marketPanel;          // Market paneli

    private int mevcutAltin;                // Şu anki altın
    private int seciliUcakIndex;            // Hangi uçak seçili

    void Start()
    {
        // Altın bilgisini yükle
        mevcutAltin = PlayerPrefs.GetInt("Altin", 0);
        altinText.text = "Altın: " + mevcutAltin;

        // Her uçağın satın alma durumunu kontrol et
        for (int i = 0; i < ucaklar.Count; i++)
        {
            ucaklar[i].satinAlindi = PlayerPrefs.GetInt("Ucak_" + i, 0) == 1;
            ButonYazisiniGuncelle(i);
        }

        // Seçili uçağı yükle
        seciliUcakIndex = PlayerPrefs.GetInt("SeciliUcak", 0);
    }

    public void UcakSatinAlOrSec(int index)
    {
        Ucak secilenUcak = ucaklar[index];

        if (secilenUcak.satinAlindi)
        {
            // Zaten satın alındıysa, sadece seç
            seciliUcakIndex = index;
            PlayerPrefs.SetInt("SeciliUcak", index);
            Debug.Log(secilenUcak.isim + " seçildi.");
        }
        else
        {
            // Satın alacak kadar altını var mı?
            if (mevcutAltin >= secilenUcak.fiyat)
            {
                mevcutAltin -= secilenUcak.fiyat;
                PlayerPrefs.SetInt("Altin", mevcutAltin);
                secilenUcak.satinAlindi = true;
                PlayerPrefs.SetInt("Ucak_" + index, 1); // Kaydet
                PlayerPrefs.SetInt("SeciliUcak", index);
                seciliUcakIndex = index;

                altinText.text = "Altın: " + mevcutAltin;
                Debug.Log(secilenUcak.isim + " satın alındı ve seçildi.");
            }
            else
            {
                Debug.Log("Yetersiz altın!");
            }
        }

        // Buton yazısını güncelle
        ButonYazisiniGuncelle(index);
    }

    void ButonYazisiniGuncelle(int index)
    {
        if (ucaklar[index].satinAlindi)
        {
            satinAlButonlari[index].GetComponentInChildren<TextMeshProUGUI>().text = "SEÇ";
        }
        else
        {
            satinAlButonlari[index].GetComponentInChildren<TextMeshProUGUI>().text = ucaklar[index].fiyat + " ALTIN";
        }
    }

    public void GeriDon()
    {
        marketPanel.SetActive(false);
        Debug.Log("Ana menüye dönüldü");
    }
}
