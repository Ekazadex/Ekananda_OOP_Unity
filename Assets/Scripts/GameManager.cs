using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public LevelManager LevelManager { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Agar tidak hilang saat berganti scene
        }
        else
        {
            Destroy(gameObject);
        }

        LevelManager = GetComponent<LevelManager>();
    }
    
    public void ClearScene()
    {
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in objects)
        {
            if (obj.tag != "MainCamera" && obj.tag != "Player") // Hint: Menghapus semua kecuali Camera dan Player
            {
                Destroy(obj);
            }
        }
    }
}

