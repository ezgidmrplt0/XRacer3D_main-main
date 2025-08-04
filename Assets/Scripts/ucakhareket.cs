using TMPro;
using UnityEngine;

public class UcakHareket : MonoBehaviour
{
    public GameObject yenidenBaslaPanel;
    public float yatayHareketHizi = 5f; // sağa-sola
    public float yukseklikHizi = 5f;    // yukarı-aşağı
    public float maxYukseklik = 4f;
    public float minYukseklik = 0f;
    public float egimHizi = 2f;
    public float maxEgilmeAcisi = 20f;

    [Header("Mesafe / Altın")]
    public float mesafeHizi = 5f; // metre/saniye cinsinden artış
    public TextMeshProUGUI mesafeText;
    public TextMeshProUGUI altinText;

    private float yPozisyonu;
    private float mevcutEgilmeX = 0f;
    private float mevcutEgilmeZ = 0f;

    private float katEdilenMesafe = 0f;
    private float oncekiAltinMesafesi = 0f;
    private int altin = 0;

    void Start()
    {
        yPozisyonu = transform.position.y;
        altin = PlayerPrefs.GetInt("Altin", 0);
        oncekiAltinMesafesi = 0f;
        GuncelleUI();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Yükseklik kontrolü
        yPozisyonu += vertical * yukseklikHizi * Time.deltaTime;
        yPozisyonu = Mathf.Clamp(yPozisyonu, minYukseklik, maxYukseklik);

        // X-Y hareket
        Vector3 yeniPozisyon = transform.position;
        yeniPozisyon.x += horizontal * yatayHareketHizi * Time.deltaTime;
        yeniPozisyon.y = yPozisyonu;
        transform.position = yeniPozisyon;

        // Eğilme animasyonu (tilt)
        float hedefEgilmeX = -vertical * maxEgilmeAcisi; // pitch
        float hedefEgilmeZ = -horizontal * maxEgilmeAcisi; // roll

        mevcutEgilmeX = Mathf.Lerp(mevcutEgilmeX, hedefEgilmeX, Time.deltaTime * egimHizi);
        mevcutEgilmeZ = Mathf.Lerp(mevcutEgilmeZ, hedefEgilmeZ, Time.deltaTime * egimHizi);

        Quaternion hedefRotasyon = Quaternion.Euler(mevcutEgilmeX, 0f, mevcutEgilmeZ);
        transform.rotation = Quaternion.Lerp(transform.rotation, hedefRotasyon, Time.deltaTime * egimHizi);

        // Mesafe (zamana bağlı)
        katEdilenMesafe += mesafeHizi * Time.deltaTime;

        // Altın kazanma her 10m'de bir
        if (katEdilenMesafe - oncekiAltinMesafesi >= 10f)
        {
            altin += 10;
            oncekiAltinMesafesi += 10f; // tam doğru aralık için 10 ekle
            GuncelleUI();
        }

        // UI mesafe her frame güncellensin
        mesafeText.text = katEdilenMesafe.ToString("F1") + " m";
    }

    void GuncelleUI()
    {
        altinText.text = "Altın: " + altin;
        mesafeText.text = katEdilenMesafe.ToString("F1") + " m";
        PlayerPrefs.SetInt("Altin", altin);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Engel"))
        {
            yenidenBaslaPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (other.CompareTag("Altin"))
        {
            altin += 10;
            GuncelleUI();
            Destroy(other.gameObject);
        }
    }
}
