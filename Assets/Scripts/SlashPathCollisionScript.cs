using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashPathCollisionScript : MonoBehaviour
{
    public bool valid;
    public Material matValid;
    public Material matInvalid;
    public List<GameObject> enemiesInMe;
    public GameObject endOfPath;
    private void Start()
    {
        enemiesInMe = new();
    }
    private void Update()
    {
        if (transform.parent.GetComponent<SlashPathHolderScript>().fatherPath_holder == null)
        {
            valid = enemiesInMe.Count > 0;
        }
        else
        {
            valid = true;
        }
        GetComponent<SpriteRenderer>().material = valid ? matValid : matInvalid;
    }
    
    private void OnTriggerEnter2D(Collider2D collision) // record slashables inside the slash path
    {
        if (collision.CompareTag("Enemy") ||
            collision.CompareTag("Bullet"))
        {
            {
                enemiesInMe.Add(collision.gameObject);
                ReflectionSlashScript.me.endOfPathes.Add(endOfPath);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision) // take out enemies that exits the slash path
    {
        if (collision.CompareTag("Enemy") ||
            collision.CompareTag("Bullet"))
        {
            enemiesInMe.Remove(collision.gameObject);
            if (ReflectionSlashScript.me.endOfPathes.Contains(endOfPath))
            {
                ReflectionSlashScript.me.endOfPathes.Remove(endOfPath);
            }
        }
    }
    public int HowManyEnemiesIHit() // return enemies inside the slash path
    {
        return enemiesInMe.Count;
    }
}
