using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kameratakip : MonoBehaviour
{
    public Transform oyuncu; // Oyuncunun Transform bile�eni
    public Vector3 kameraMesafe = new Vector3(0f, 2f, -5f); // Kameran�n oyuncuya olan mesafesi
    public float takipHizi = 5f; // Kameran�n takip etme h�z�

    private void LateUpdate()
    {
        if (oyuncu != null)
        {
            // Kameran�n hedef pozisyonu
            Vector3 hedefPozisyon = oyuncu.position + kameraMesafe;

            // Kameray� yumu�ak bir �ekilde hareket ettir
            transform.position = Vector3.Lerp(transform.position, hedefPozisyon, takipHizi * Time.deltaTime);

            // Kameran�n oyuncuya bakmas�n� sa�la
            transform.LookAt(oyuncu);
        }
    }
}
