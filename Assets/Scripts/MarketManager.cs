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
    public List<Material> skinler;   // Farklı skin'ler (materyaller)

    [HideInInspector] public bool satinAlindi; // Satın alma durumu
    [HideInInspector] public int seciliSkinIndex; // Hangi skin aktif
}

public class MarketManager : MonoBehaviour
{
    [Header("Market Ayarları")]
    public List<Ucak> ucaklar;                // Inspector’dan ayarla
    public List<Button> satinAlButonlari;     // Her uçağa ait butonlar (UI)
    public TextMeshProUGUI altinText;         // UI’daki altın yazısı
    public GameObject marketPanel;            // Market paneli

    [Header("Seçili Uçak Yerleşimi")]
    public Transform seciliUcakSpawnNoktasi;  // Seçilen uçağın sahnede doğacağı yer

    private int mevcutAltin;                  // Şu anki altın
    private int seciliUcakIndex;              // Hangi uçak seçili
    private GameObject aktifUcakInstance;     // Sahnedeki aktif uçak objesi

    void Start()
    {
        // Altın yükle ve UI
        mevcutAltin = PlayerPrefs.GetInt("Altin", 0);
        GuncelleAltinUI();

        // Uçakları yükle: satın alındı / seçili skin
        for (int i = 0; i < ucaklar.Count; i++)
        {
            int index = i; // closure için
            ucaklar[i].satinAlindi = PlayerPrefs.GetInt("Ucak_" + i, 0) == 1;
            ucaklar[i].seciliSkinIndex = PlayerPrefs.GetInt($"Ucak_{i}_Skin", 0);

            // Buton listener ekle
            if (i < satinAlButonlari.Count && satinAlButonlari[i] != null)
            {
                satinAlButonlari[i].onClick.RemoveAllListeners();
                satinAlButonlari[i].onClick.AddListener(() => UcakSatinAlOrSec(index));
            }

            ButonYazisiniGuncelle(i);
        }

        // Seçili uçağı yükle (eğer daha önce seçilmişse)
        seciliUcakIndex = PlayerPrefs.GetInt("SeciliUcak", 0);
        LoadSelectedUcak();
    }

    public void UcakSatinAlOrSec(int index)
    {
        Ucak secilenUcak = ucaklar[index];

        if (secilenUcak.satinAlindi)
        {
            // Zaten alınmışsa sadece seç
            seciliUcakIndex = index;
            PlayerPrefs.SetInt("SeciliUcak", index);
            PlayerPrefs.Save();
            Debug.Log(secilenUcak.isim + " seçildi.");
        }
        else
        {
            if (mevcutAltin >= secilenUcak.fiyat)
            {
                // Satın al ve seç
                mevcutAltin -= secilenUcak.fiyat;
                PlayerPrefs.SetInt("Altin", mevcutAltin);
                secilenUcak.satinAlindi = true;
                PlayerPrefs.SetInt("Ucak_" + index, 1);
                seciliUcakIndex = index;
                PlayerPrefs.SetInt("SeciliUcak", index);
                PlayerPrefs.Save();

                GuncelleAltinUI();
                Debug.Log(secilenUcak.isim + " satın alındı ve seçildi.");
            }
            else
            {
                Debug.Log("Yetersiz altın!");
                // Buraya kullanıcıya görsel uyarı vs koyabilirsin.
            }
        }

        ButonYazisiniGuncelle(index);
        LoadSelectedUcak();
    }

    // Seçili uçağı instantiate et ve aktif skin'i uygula
    private void LoadSelectedUcak()
    {
        // Önceki varsa yok et
        if (aktifUcakInstance != null)
            Destroy(aktifUcakInstance);

        if (seciliUcakIndex < 0 || seciliUcakIndex >= ucaklar.Count) return;

        Ucak secili = ucaklar[seciliUcakIndex];
        if (secili.ucakPrefab == null) return;

        // Instantiate
        if (seciliUcakSpawnNoktasi != null)
        {
            aktifUcakInstance = Instantiate(secili.ucakPrefab, seciliUcakSpawnNoktasi.position, seciliUcakSpawnNoktasi.rotation);
        }
        else
        {
            aktifUcakInstance = Instantiate(secili.ucakPrefab);
        }

        ApplySkin(secili, aktifUcakInstance);
    }

    // Skin değiştir (örneğin UI'dan çağrılır)
    public void SkinDegistir(int ucakIndex, int skinIndex)
    {
        if (ucakIndex < 0 || ucakIndex >= ucaklar.Count) return;
        Ucak u = ucaklar[ucakIndex];
        if (!u.satinAlindi) return; // satın alınmamışsa yapma

        skinIndex = Mathf.Clamp(skinIndex, 0, u.skinler.Count - 1);
        u.seciliSkinIndex = skinIndex;
        PlayerPrefs.SetInt($"Ucak_{ucakIndex}_Skin", skinIndex);
        PlayerPrefs.Save();

        // Eğer şu an seçili uçaksa, görünümü güncelle
        if (seciliUcakIndex == ucakIndex && aktifUcakInstance != null)
        {
            ApplySkin(u, aktifUcakInstance);
        }
    }

    // Verilen uçak için seçili skin'i instance'a uygular
    private void ApplySkin(Ucak u, GameObject instance)
    {
        if (u.skinler == null || u.skinler.Count == 0) return;

        int skinIndex = Mathf.Clamp(u.seciliSkinIndex, 0, u.skinler.Count - 1);
        Material mat = u.skinler[skinIndex];
        if (mat == null) return;

        Renderer[] rends = instance.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
        {
            // Dikkat: burada sharedMaterial değil material kullanırsan runtime'da instance'ı etkilersin
            r.material = mat;
        }
    }

    void ButonYazisiniGuncelle(int index)
    {
        if (index >= satinAlButonlari.Count) return;

        var btn = satinAlButonlari[index];
        if (btn == null) return;

        if (ucaklar[index].satinAlindi)
        {
            btn.GetComponentInChildren<TextMeshProUGUI>().text = "SEÇ";
        }
        else
        {
            btn.GetComponentInChildren<TextMeshProUGUI>().text = ucaklar[index].fiyat + " ALTIN";
        }
    }

    void GuncelleAltinUI()
    {
        altinText.text = "Altın: " + mevcutAltin;
    }

    public void GeriDon()
    {
        marketPanel.SetActive(false);
        Debug.Log("Ana menüye dönüldü");
    }

    // Yardımcı: şu anki seçili uçağın skin butonuna basıldığında çağrılabilir
    public void OnSkinButonunaBasildi(int skinIndex)
    {
        SkinDegistir(seciliUcakIndex, skinIndex);
    }
}
