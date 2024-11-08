using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector2 maxSpeed;
    [SerializeField] private Vector2 timeToFullSpeed;
    [SerializeField] private Vector2 timeToStop;
    [SerializeField] private Vector2 stopClamp;

    private Vector2 moveDirection;
    private Vector2 moveVelocity;
    private Vector2 moveFriction;
    private Vector2 stopFriction;
    private Rigidbody2D rb;

    // Mendefinisikan batas layar untuk pergerakan
    private float minX, maxX, minY, maxY;
    [SerializeField] private float boundaryMarginX = 0.5f; // Margin batas kiri dan kanan
    [SerializeField] private float boundaryMarginTop = 0.5f; // Margin batas atas
    [SerializeField] private float boundaryMarginBottom = 0.2f; // Margin batas bawah agar lebih mendekati tepi

    private void Start()
    {
    // Mendapatkan komponen Rigidbody2D yang terpasang pada GameObject Player
    rb = GetComponent<Rigidbody2D>();

    // Menghitung nilai awal untuk moveVelocity, moveFriction, dan stopFriction
    moveVelocity = 2 * maxSpeed / timeToFullSpeed;
    moveFriction = -2 * maxSpeed / (timeToFullSpeed * timeToFullSpeed);
    stopFriction = -2 * maxSpeed / (timeToStop * timeToStop);

    // Menghitung batas layar berdasarkan tampilan kamera dan menambahkan margin
    Camera camera = Camera.main;
    Vector3 screenBounds = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camera.transform.position.z));
    minX = -screenBounds.x + boundaryMarginX;
    maxX = screenBounds.x - boundaryMarginX;
    minY = -screenBounds.y + boundaryMarginBottom; // Menggunakan margin bawah khusus
    maxY = screenBounds.y - boundaryMarginTop; // Menggunakan margin atas khusus
    }

    private void FixedUpdate()
    {
        // Memanggil fungsi Move untuk mengatur pergerakan pemain
        Move();

        // Membatasi posisi pemain agar tetap di dalam batas layar
        ClampPosition();
    }

    public void Move()
    {
        // Mendapatkan arah input dari pemain
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        if (moveDirection != Vector2.zero)
        {
            // Mempercepat dengan halus menggunakan moveVelocity dan menerapkan arah gerakan
            Vector2 targetVelocity = moveDirection * maxSpeed;
            Vector2 velocityChange = moveDirection * moveVelocity * Time.fixedDeltaTime;

            // Membatasi kecepatan hingga kecepatan maksimum
            rb.velocity = Vector2.ClampMagnitude(rb.velocity + velocityChange, maxSpeed.magnitude);
        }
        else
        {
            // Menerapkan gesekan untuk memperlambat pemain
            Vector2 frictionForce = GetFriction() * Time.fixedDeltaTime;
            rb.velocity += frictionForce;

            // Menghentikan pemain jika kecepatan di bawah batas stop clamp
            if (rb.velocity.magnitude < stopClamp.magnitude)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void ClampPosition()
    {
    // Membatasi posisi pemain dalam batas layar dengan margin
    Vector3 clampedPosition = transform.position;
    clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
    clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
    transform.position = clampedPosition;
    }

    private Vector2 GetFriction()
    {
        // Memeriksa apakah kecepatan lebih besar dari nol
        if (rb.velocity.sqrMagnitude > 0)
        {
            // Mengembalikan gesekan gerakan
            return moveFriction * rb.velocity.normalized;
        }
        else
        {
            // Mengembalikan gesekan berhenti
            return stopFriction;
        }
    }

    public bool IsMoving()
    {
        // Mengembalikan true jika pemain bergerak, false jika tidak
        return rb.velocity.magnitude > 0.1f;
    }
}
