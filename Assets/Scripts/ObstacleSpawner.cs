using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] engelPrefablar; // Engellerin prefablar�
    public Transform spawnNoktasi; // Engellerin spawnlanaca�� nokta
    public float spawnAraligi = 1.5f; // Ka� saniyede bir spawn olacak
    public float hareketHizi = 5f; // Z ekseninde hareket h�z�
    public float zArttirmaAraligi = 5f; // Z ekseninin her 5 saniyede bir artaca�� aral�k
    public float zArttirmaMiktari = 10f; // Z eksenindeki art�� miktar�

    private float minX, maxX, minZ, maxZ;
    private float spawnZDegeri; // Ba�lang��ta spawn noktas�n�n z de�eri
    private List<Vector3> spawnNoktalari = new List<Vector3>(); // �nceden kullan�lan spawn noktalar�

    void Start()
    {
        spawnZDegeri = spawnNoktasi.position.z; // Ba�lang�� Z de�eri
        minX = -50f; // X ekseni i�in daha geni� aral�k
        maxX = 50f;
        minZ = 0f; // Z ekseni i�in daha geni� aral�k
        maxZ = 100f;

        StartCoroutine(EngelleriSpawnla());
        StartCoroutine(ZArttir());
    }

    // Spawn noktas� Z de�erini artt�ran coroutine
    IEnumerator ZArttir()
    {
        while (true)
        {
            yield return new WaitForSeconds(zArttirmaAraligi); // 5 saniye bekle
            spawnZDegeri += zArttirmaMiktari; // Z'yi 10 artt�r
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

    // Engel s�ras�yla spawn yapma
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

    // Yeni spawn noktas� bulma
    Vector3 GetYeniSpawnNoktasi()
    {
        Vector3 yeniSpawnNoktasi;
        bool noktaUygun = false;

        do
        {
            float rastgeleX = Random.Range(minX, maxX);
            float rastgeleZ = Random.Range(minZ, maxZ);

            yeniSpawnNoktasi = new Vector3(rastgeleX, -1.59f, spawnZDegeri);

            // �nceki spawn noktalar�na �ok yak�n de�ilse uygun kabul et
            noktaUygun = true;
            foreach (Vector3 eskiNokta in spawnNoktalari)
            {
                if (Vector3.Distance(eskiNokta, yeniSpawnNoktasi) < 5f) // 5 birimden daha yak�nsa, uygun de�il
                {
                    noktaUygun = false;
                    break;
                }
            }

        } while (!noktaUygun);

        spawnNoktalari.Add(yeniSpawnNoktasi); // Yeni spawn noktas�n� listeye ekle
        return yeniSpawnNoktasi;
    }
}

// Engel hareketi i�in ayr� bir script
public class EngelHareket : MonoBehaviour
{
    public float hareketHizi = 5f;

    void Update()
    {
        transform.Translate(Vector3.back * hareketHizi * Time.deltaTime);
    }
}
