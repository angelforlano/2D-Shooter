using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject impactPrefab;
    public int lifeTime = 5;
    [Range(2, 12)] public float speed = 6;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Environment") || other.gameObject.CompareTag("Ground"))
        {
            GameObject impact = Instantiate(impactPrefab, transform.position, transform.rotation);
            
            Destroy(impact, 0.25f);
            Destroy(gameObject);
        }    
    }
}