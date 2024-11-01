using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public PlayerMovement playerMovement;
    public Animator animator;

    private void Awake()
    {
        // Pola Singleton untuk memastikan hanya ada satu instance dari Player
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }
    }

    private void Start()
    {
        // Mendapatkan komponen PlayerMovement yang terpasang pada GameObject Player
        playerMovement = GetComponent<PlayerMovement>();
        // Mendapatkan komponen Animator dari GameObject anak
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        // Memanggil fungsi Move dari PlayerMovement untuk menggerakkan pemain
        playerMovement.Move();
    }

    private void LateUpdate()
    {
        // Mengatur parameter IsMoving pada animator berdasarkan apakah pemain sedang bergerak
        animator.SetBool("IsMoving", playerMovement.IsMoving());
    }
}