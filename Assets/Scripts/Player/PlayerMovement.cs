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

    private void Start()
    {
        // Mendapatkan komponen Rigidbody2D yang terpasang pada GameObject Player
        rb = GetComponent<Rigidbody2D>();

        // Menghitung nilai awal untuk moveVelocity, moveFriction, dan stopFriction
        moveVelocity = 2 * maxSpeed / timeToFullSpeed;
        moveFriction = -2 * maxSpeed / (timeToFullSpeed * timeToFullSpeed);
        stopFriction = -2 * maxSpeed / (timeToStop * timeToStop);
    }

    private void FixedUpdate()
    {
        // Memanggil fungsi Move untuk mengatur pergerakan pemain
        Move();
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