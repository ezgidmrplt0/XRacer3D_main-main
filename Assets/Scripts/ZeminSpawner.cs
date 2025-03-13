using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeminSpawner : MonoBehaviour
{
    public GameObject zeminPrefab;    // Zemin prefab'ý (yol parçalarý)
    public float spawnAraligi = 5f;   // Spawn aralýðý (5 saniye)
    public float spawnMesafesi = 100f; // Her 5 saniyede z ekseninde artacak mesafe (100 birim)
    private float sonSpawnZamani;     // Son spawn zamaný
    private float zeminPozisyonu;     // Zeminlerin baþlangýç pozisyonunu takip edeceðiz
    private GameObject mevcutZemin;   // Þu anki zemin parçasý
    private float spawnZamani;        // Zeminin ne zaman silineceðini takip eden zamanlayýcý
    private float silmeZamani = 180f; // 3 dakika sonra zemin silinecek

    void Start()
    {
        zeminPozisyonu = transform.position.z; // Baþlangýç pozisyonunu kaydediyoruz
    }

    void Update()
    {
        sonSpawnZamani += Time.deltaTime;

        // Eðer spawn aralýðý geçtiyse yeni zemin oluþtur
        if (sonSpawnZamani >= spawnAraligi)
        {
            YeniZeminOlustur();
            sonSpawnZamani = 0f;
        }

        // Silme zamanýný azaltýyoruz
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
        // Yeni zemin parçasý oluþturuluyor ve yeni zemin pozisyonu güncelleniyor
        mevcutZemin = Instantiate(zeminPrefab, new Vector3(transform.position.x, transform.position.y, zeminPozisyonu), Quaternion.identity);

        // Zemin pozisyonunu 100 birim artýrýyoruz
        zeminPozisyonu += spawnMesafesi;

        // Zemin için silme zamanýný sýfýrlýyoruz
        spawnZamani = silmeZamani;  // 3 dakika sonra silme iþlemi yapýlacak
    }

    void SilZemin()
    {
        if (mevcutZemin != null)
        {
            // Zemin siliniyor
            Destroy(mevcutZemin);
        }

        // Silme zamanýný sýfýrlýyoruz, yeni zemin oluþturulmadan önce
        spawnZamani = silmeZamani;
    }
}
