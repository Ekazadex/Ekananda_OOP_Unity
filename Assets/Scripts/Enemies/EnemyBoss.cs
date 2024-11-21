using UnityEngine;
using UnityEngine.Pool;

public class EnemyBoss : EnemyHorizontal
{
    public Bullet bullet;
    public Transform bulletSpawnPoint;
    public float shootInterval = 2f;

    private float shootTimer;

    [Header("Bullet Pool")]
    private IObjectPool<Bullet> objectPool;

    private readonly bool collectionCheck = false;
    private readonly int defaultCapacity = 30;
    private readonly int maxSize = 100;

    public override void Awake()
    {
        base.Awake();
        // Menginisialisasi ObjectPool
        objectPool = new ObjectPool<Bullet>(
            CreateBullet,
            OnGetBullet,
            OnReleaseBullet,
            OnDestroyBullet,
            collectionCheck,
            defaultCapacity,
            maxSize
        );

        // Mencari BulletSpawnPoint
        if (bulletSpawnPoint == null)
        {
            bulletSpawnPoint = transform.Find("BulletSpawnPoint");

            if (bulletSpawnPoint == null)
            {
                Debug.LogWarning(this + " tidak menemukan BulletSpawnPoint");
            }
            else
            {
                bulletSpawnPoint.position = new Vector3(0, 1, 0); // Memberikan offset pada BulletSpawnPoint
            }
        }
    }

    new void Start()
    {
        base.Start();
        shootTimer = 0; // Menyiapkan timer untuk menembak
    }

    public override void Move()
    {
        base.Move();

        // Menembak Bullet setiap interval waktu
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            Shoot();
            shootTimer = 0;
        }
    }

    private Bullet CreateBullet()
    {
        Bullet newBullet = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        newBullet.SetObjectPool(objectPool);
        return newBullet;
    }

    private void OnGetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
        bullet.transform.position = bulletSpawnPoint.position;
        bullet.transform.rotation = bulletSpawnPoint.rotation;
    }

    private void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
    
    public Bullet Shoot()
    {
        if (objectPool != null && bulletSpawnPoint != null)
        {
            Bullet bulletInstance = objectPool.Get();
            return bulletInstance;
        }
        return null;
    }
}
