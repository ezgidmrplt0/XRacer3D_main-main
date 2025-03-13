using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeminSpawner : MonoBehaviour
{
    public GameObject zeminPrefab;    // Zemin prefab'� (yol par�alar�)
    public float spawnAraligi = 5f;   // Spawn aral��� (5 saniye)
    public float spawnMesafesi = 100f; // Her 5 saniyede z ekseninde artacak mesafe (100 birim)
    private float sonSpawnZamani;     // Son spawn zaman�
    private float zeminPozisyonu;     // Zeminlerin ba�lang�� pozisyonunu takip edece�iz
    private GameObject mevcutZemin;   // �u anki zemin par�as�
    private float spawnZamani;        // Zeminin ne zaman silinece�ini takip eden zamanlay�c�
    private float silmeZamani = 180f; // 3 dakika sonra zemin silinecek

    void Start()
    {
        zeminPozisyonu = transform.position.z; // Ba�lang�� pozisyonunu kaydediyoruz
    }

    void Update()
    {
        sonSpawnZamani += Time.deltaTime;

        // E�er spawn aral��� ge�tiyse yeni zemin olu�tur
        if (sonSpawnZamani >= spawnAraligi)
        {
            YeniZeminOlustur();
            sonSpawnZamani = 0f;
        }

        // Silme zaman�n� azalt�yoruz
        if (spawnZamani > 0)
        {
            spawnZamani -= Time.deltaTime;
        }
        else
        {
            // 3 dakika sonra zemin silinir
            SilZemin();
        }
    }

    void YeniZeminOlustur()
    {
        // Yeni zemin par�as� olu�turuluyor ve yeni zemin pozisyonu g�ncelleniyor
        mevcutZemin = Instantiate(zeminPrefab, new Vector3(transform.position.x, transform.position.y, zeminPozisyonu), Quaternion.identity);

        // Zemin pozisyonunu 100 birim art�r�yoruz
        zeminPozisyonu += spawnMesafesi;

        // Zemin i�in silme zaman�n� s�f�rl�yoruz
        spawnZamani = silmeZamani;  // 3 dakika sonra silme i�lemi yap�lacak
    }

    void SilZemin()
    {
        if (mevcutZemin != null)
        {
            // Zemin siliniyor
            Destroy(mevcutZemin);
        }

        // Silme zaman�n� s�f�rl�yoruz, yeni zemin olu�turulmadan �nce
        spawnZamani = silmeZamani;
    }
}
