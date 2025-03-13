    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    public class UcakHareket : MonoBehaviour
    {
        public GameObject yenidenBaslaPanel; // Inspector’dan bağlayacağın panel
        public float hareketHizi = 5f;  // X ekseninde sağa/sola hareket hızı
        public float yukseklikHizi = 5f; // Y ekseninde yukarı/aşağı hareket hızı
        public float maxYukseklik = 4f;
        public float minYukseklik = 0f;
        public float hizArtisKademesi = 10f; // Belirli mesafelerde hız artışı
        public float egimHizi = 2f;
        public float maxEgilmeAcisi = 20f;

        private float yPozisyonu;
        private float previousZPosition = 0f;
        private float mevcutEgilmeX = 0f;
        private float mevcutEgilmeZ = 0f;
        private float baslangicZ;
        private float oncekiAltinMesafesi; // 💰 Altın kazanma için önceki mesafe

        public TextMeshProUGUI mesafeText;
        public TextMeshProUGUI altinText; // 💰 UI için Altın Text 

        private int altin = 0; // 💰 Altın miktarı
        private float katEdilenMesafe = 0f; // Mesafe, her saniye 1 metre artacak şekilde güncellenecek

        void Start()
        {
            yPozisyonu = transform.position.y;
            previousZPosition = transform.position.z;
            baslangicZ = transform.position.z;
            oncekiAltinMesafesi = baslangicZ; // 💰 Altın mesafesini başlangıca ayarla

            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            GuncelleUI(); // 🎮 UI'yı başlat
        }

        void Update()
        {
            // Oyuncu inputlarını al
            float horizontal = Input.GetAxis("Horizontal"); // Sağ-Sol (A-D)
            float vertical = Input.GetAxis("Vertical");     // Yukarı-Aşağı (W-S)

            // Yukarı-aşağı hareket
            yPozisyonu += vertical * yukseklikHizi * Time.deltaTime;
            yPozisyonu = Mathf.Clamp(yPozisyonu, minYukseklik, maxYukseklik);

            // Belirli mesafelerde hızı artır
            if (transform.position.z - previousZPosition >= 100f)
            {
                hareketHizi += hizArtisKademesi;
                previousZPosition = transform.position.z;
            }

            // Uçağın sadece sağa/sola hareketini uygula (Z ekseni sabit kalacak)
            Vector3 hareket = new Vector3(horizontal, 0f, 0f); // Z eksenini kaldırdık
            Vector3 yeniPozisyon = transform.position + hareket * hareketHizi * Time.deltaTime;
            yeniPozisyon.y = yPozisyonu;
            transform.position = yeniPozisyon;

            // Uçağın eğilme açısını hesapla
            float hedefEgilmeX = horizontal * maxEgilmeAcisi;
            float hedefEgilmeZ = -vertical * maxEgilmeAcisi; // Yukarı çıkarken eğim değişsin

            mevcutEgilmeX = Mathf.Lerp(mevcutEgilmeX, hedefEgilmeX, Time.deltaTime * egimHizi);
            mevcutEgilmeZ = Mathf.Lerp(mevcutEgilmeZ, hedefEgilmeZ, Time.deltaTime * egimHizi);

            Quaternion hedefRotasyon = Quaternion.Euler(mevcutEgilmeX, 90f, mevcutEgilmeZ);
            transform.rotation = Quaternion.Lerp(transform.rotation, hedefRotasyon, Time.deltaTime * egimHizi);

            // 🚀 **Mesafeyi Güncelle ve UI'ye Yazdır**
            katEdilenMesafe += 1f * Time.deltaTime; // Her saniye 1 metre mesafe artacak
            mesafeText.text = katEdilenMesafe.ToString("F1") + " m";

            // 💰 **Altın Kazanımı**
            if (katEdilenMesafe - oncekiAltinMesafesi >= 10f)
            {
                altin += 10;
                oncekiAltinMesafesi = katEdilenMesafe;
                GuncelleUI();
            }
        }

        void GuncelleUI()
        {
            altinText.text = "Altın: " + altin;
            PlayerPrefs.SetInt("Altin", altin); // 💾 Altını kaydet
        }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Engel")) // Eğer "Engel" tag'ine sahip bir şeye çarptıysa
        {
            yenidenBaslaPanel.SetActive(true); // Paneli aç
            Time.timeScale = 0f; // Oyunu durdur
        }
    }
}
