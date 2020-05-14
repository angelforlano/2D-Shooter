using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform gunPoint;
    public AudioSource shootEffect;
    [Range(2, 8)] public float speed = 4;
    [Range(10, 20)] public float jumpForce = 3;
    [Range(0, 100)] public int bulletsCount = 100;
    [Range(0.005f, 0.5f)] public float fireRate = 0.5f;
    public ForceMode2D forceMode;
    
    bool isRunning;
    int jumpCount;
    float lastShootTime;
    SpriteRenderer renderer;
    Rigidbody2D rigidbody;
    Animator animator;

    void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();

        GameHUD.Instance.UpdateHUD(this);
    }

    void Update()
    {
        // <Update Input's>
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.Return))
        {
            Move(1);
        }
        
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.Return))
        {
            Move(-1);
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            Jump();
        }
        
        if (Input.GetKey(KeyCode.Return) && Time.time > fireRate + lastShootTime && bulletsCount > 0)
        {
            Shoot();
        }

        // <Update Animator>
        animator.SetBool("IsRunning", isRunning);

        if (isRunning)
        {
            isRunning = false;
        }
    }

    void Move(int movement)
    {
        isRunning = true;
        renderer.flipX = movement != 1;
        var movementVector = new Vector3(movement * speed * Time.deltaTime, 0, 0);
        transform.Translate(movementVector);
    }

    void Jump()
    {
        jumpCount++;
        rigidbody.AddForce(new Vector3(0, jumpForce, 0), forceMode);
        animator.SetTrigger("Jump");
    }

    void Shoot()
    {
        bulletsCount--;
        animator.SetTrigger("Shoot");
        shootEffect.pitch = Random.Range(1.5f, 1.95f);
        shootEffect.Play();

        lastShootTime = Time.time;
        
        Quaternion rotation = transform.rotation;
        Vector3 position = gunPoint.position;

        if (renderer.flipX)
        {
            rotation = Quaternion.Euler(0, 0, 180);
            position = new Vector3(transform.position.x + (gunPoint.localPosition.x * -1), gunPoint.position.y, gunPoint.position.z);
        }
        
        Instantiate(bulletPrefab, position, rotation);

        GameHUD.Instance.UpdateHUD(this);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PowerUp"))
        {
            bulletsCount += 75;
            GameHUD.Instance.UpdateHUD(this);
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }
}