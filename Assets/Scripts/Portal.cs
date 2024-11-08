using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;

    private Vector2 newPosition;

    private void Start()
    {
        ChangePosition();
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, newPosition) < 0.5f)
        {
            ChangePosition();
        }

        // Cek apakah pemain memiliki senjata
        if (PlayerHasWeapon())
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Load scene Main
            LevelManager.Instance.LoadScene("Main");
        }
    }

    private void ChangePosition()
    {
        // Generate posisi baru secara random
        newPosition = Random.insideUnitCircle * 5; // Sesuaikan radius sesuai kebutuhan
        newPosition += (Vector2)transform.position;
    }

    private bool PlayerHasWeapon()
    {
        // Implementasi logika untuk mengecek apakah pemain memiliki senjata
        // Contoh:
        // GameObject player = GameObject.FindGameObjectWithTag("Player");
        // PlayerController playerController = player.GetComponent<PlayerController>();
        // return playerController.hasWeapon;
        return true; // Ganti dengan logika yang sesuai
    }
}
