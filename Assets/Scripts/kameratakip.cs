using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kameratakip : MonoBehaviour
{
    public Transform oyuncu; // Oyuncunun Transform bileþeni
    public Vector3 kameraMesafe = new Vector3(0f, 2f, -5f); // Kameranýn oyuncuya olan mesafesi
    public float takipHizi = 5f; // Kameranýn takip etme hýzý

    private void LateUpdate()
    {
        if (oyuncu != null)
        {
            // Kameranýn hedef pozisyonu
            Vector3 hedefPozisyon = oyuncu.position + kameraMesafe;

            // Kamerayý yumuþak bir þekilde hareket ettir
            transform.position = Vector3.Lerp(transform.position, hedefPozisyon, takipHizi * Time.deltaTime);

            // Kameranýn oyuncuya bakmasýný saðla
            transform.LookAt(oyuncu);
        }
    }
}
