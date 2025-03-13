using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] engelPrefablar; // Engellerin prefablarý
    public Transform spawnNoktasi; // Engellerin spawnlanacaðý nokta
    public float spawnAraligi = 1.5f; // Kaç saniyede bir spawn olacak
    public float hareketHizi = 5f; // Z ekseninde hareket hýzý
    public float zArttirmaAraligi = 5f; // Z ekseninin her 5 saniyede bir artacaðý aralýk
    public float zArttirmaMiktari = 10f; // Z eksenindeki artýþ miktarý

    private float minX, maxX, minZ, maxZ;
    private float spawnZDegeri; // Baþlangýçta spawn noktasýnýn z deðeri
    private List<Vector3> spawnNoktalari = new List<Vector3>(); // Önceden kullanýlan spawn noktalarý

    void Start()
    {
        spawnZDegeri = spawnNoktasi.position.z; // Baþlangýç Z deðeri
        minX = -50f; // X ekseni için daha geniþ aralýk
        maxX = 50f;
        minZ = 0f; // Z ekseni için daha geniþ aralýk
        maxZ = 100f;

        StartCoroutine(EngelleriSpawnla());
        StartCoroutine(ZArttir());
    }

    // Spawn noktasý Z deðerini arttýran coroutine
    IEnumerator ZArttir()
    {
        while (true)
        {
            yield return new WaitForSeconds(zArttirmaAraligi); // 5 saniye bekle
            spawnZDegeri += zArttirmaMiktari; // Z'yi 10 arttýr
        }
    }

    IEnumerator EngelleriSpawnla()
    {
        while (true)
        {
            yield return StartCoroutine(EngelSirasiSpawnla());
            yield return new WaitForSeconds(spawnAraligi);
        }
    }

    // Engel sýrasýyla spawn yapma
    IEnumerator EngelSirasiSpawnla()
    {
        foreach (GameObject engelPrefab in engelPrefablar)
        {
            Vector3 yeniSpawnNoktasi = GetYeniSpawnNoktasi();
            GameObject yeniEngel = Instantiate(engelPrefab, yeniSpawnNoktasi, Quaternion.identity);

            yeniEngel.AddComponent<EngelHareket>(); // Engel hareketini ekleyelim

            yield return new WaitForSeconds(Random.Range(0f, 1.5f));
        }
    }

    // Yeni spawn noktasý bulma
    Vector3 GetYeniSpawnNoktasi()
    {
        Vector3 yeniSpawnNoktasi;
        bool noktaUygun = false;

        do
        {
            float rastgeleX = Random.Range(minX, maxX);
            float rastgeleZ = Random.Range(minZ, maxZ);

            yeniSpawnNoktasi = new Vector3(rastgeleX, -1.59f, spawnZDegeri);

            // Önceki spawn noktalarýna çok yakýn deðilse uygun kabul et
            noktaUygun = true;
            foreach (Vector3 eskiNokta in spawnNoktalari)
            {
                if (Vector3.Distance(eskiNokta, yeniSpawnNoktasi) < 5f) // 5 birimden daha yakýnsa, uygun deðil
                {
                    noktaUygun = false;
                    break;
                }
            }

        } while (!noktaUygun);

        spawnNoktalari.Add(yeniSpawnNoktasi); // Yeni spawn noktasýný listeye ekle
        return yeniSpawnNoktasi;
    }
}

// Engel hareketi için ayrý bir script
public class EngelHareket : MonoBehaviour
{
    public float hareketHizi = 5f;

    void Update()
    {
        transform.Translate(Vector3.back * hareketHizi * Time.deltaTime);
    }
}
