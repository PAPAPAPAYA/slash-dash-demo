using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashPathCollisionScript : MonoBehaviour
{
    public static SlashPathCollisionScript me;
    public bool validity;
    public Material matValid;
    public Material matInvalid;
    private List<GameObject> enemiesInMe;
    
    private void Awake()
    {
        me = this;
    }
    private void Start()
    {
        enemiesInMe = new();
    }
    private void Update()
    {
        validity = enemiesInMe.Count > 0;
        GetComponent<SpriteRenderer>().material = validity ? matValid : matInvalid;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") ||
            collision.CompareTag("Bullet"))
        {
            enemiesInMe.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") ||
            collision.CompareTag("Bullet"))
        {
            enemiesInMe.Remove(collision.gameObject);
        }
    }
    public int HowManyEnemiesIHit()
    {
        return enemiesInMe.Count;
    }
}
