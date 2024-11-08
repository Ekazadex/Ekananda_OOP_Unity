using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] Weapon weaponHolder;
    Weapon weapon;

    void Awake()
    {
        // Buat instance baru dari weaponHolder dan sembunyikan
        weapon = Instantiate(weaponHolder);
        weapon.gameObject.SetActive(false);
    }

    void Start()
    {
        if (weapon != null)
        {
            TurnVisual(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Cek jika player memasuki area trigger
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Objek Player Memasuki trigger");

            // Set parent dari weapon menjadi player
            weapon.transform.SetParent(other.transform);

             // Atur posisi lokal senjata relatif terhadap pesawat agar tidak berpindah-pindah
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            
            // Aktifkan visual dari weapon
            TurnVisual(true);

            // Hancurkan objek WeaponPickup setelah diambil oleh player
            Destroy(gameObject);
        }
        else 
        {
            Debug.Log("Bukan Objek Player yang memasuki Trigger");
        }
    }

    // Mengatur tampilan dari weapon
    void TurnVisual(bool on)
    {
        weapon.gameObject.SetActive(on);
    }

    // Overload untuk mengatur tampilan dari weapon tertentu
    void TurnVisual(bool on, Weapon weapon)
    {
        weapon.gameObject.SetActive(on);
    }
}
