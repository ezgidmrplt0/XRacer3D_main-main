using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OyunBaslat : MonoBehaviour
{
    [Header("Prefab ve UI")]
    public List<GameObject> ucakPrefabListesi;
    public Transform spawnNoktasi;
    public TextMeshProUGUI altinText;
    public TextMeshProUGUI mesafeText;
    public GameObject yenidenBaslaPanel; // UcakHareket içindeki paneli buradan ver

    [Header("Diðer Scriptler")]
    public kameratakip kameraTakipScript; // Inspector’dan baðla

    void Start()
    {
        int seciliIndex = PlayerPrefs.GetInt("SeciliUcak", 0);
        if (seciliIndex < 0 || seciliIndex >= ucakPrefabListesi.Count)
            seciliIndex = 0;

        GameObject seciliPrefab = ucakPrefabListesi[seciliIndex];

        GameObject aktifUcak = Instantiate(seciliPrefab, spawnNoktasi.position, seciliPrefab.transform.rotation);
        aktifUcak.name = "AktifUcak";

        // Kamera takibi baþlat
        if (kameraTakipScript != null)
            kameraTakipScript.oyuncu = aktifUcak.transform;

        // UcakHareket scriptini al veya ekle
        UcakHareket hareketScript = aktifUcak.GetComponent<UcakHareket>();
        if (hareketScript == null)
        {
            hareketScript = aktifUcak.AddComponent<UcakHareket>();
        }

        // UI ve panel referanslarýný set et
        hareketScript.altinText = altinText;
        hareketScript.mesafeText = mesafeText;

        if (yenidenBaslaPanel != null)
            hareketScript.yenidenBaslaPanel = yenidenBaslaPanel;

        // Altýn UI baþlat (UcakHareket de kendi Start'ýnda PlayerPrefs'ten okuyorsa çakýþma olmaz ama burada senkronize edelim)
        int mevcutAltin = PlayerPrefs.GetInt("Altin", 0);
        if (altinText != null)
            altinText.text = "Altýn: " + mevcutAltin;
        if (mesafeText != null)
            mesafeText.text = "0.0 m";
    }
}
