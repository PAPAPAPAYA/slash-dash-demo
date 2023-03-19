using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashPathCollisionScript : MonoBehaviour
{
    public bool validity;
    public Material matValid;
    public Material matInvalid;
    private List<GameObject> enemiesInMe;
    #region SINGLETON
    public static SlashPathCollisionScript me;
    private void Awake()
    {
        me = this;
    }
    #endregion
    private void Start()
    {
        enemiesInMe = new();
    }
    private void Update()
    {
        validity = enemiesInMe.Count > 0;
        GetComponent<SpriteRenderer>().material = validity ? matValid : matInvalid;
    }
    private void OnTriggerEnter2D(Collider2D collision) // record slashables inside the slash path
    {
        if (collision.CompareTag("Enemy") ||
            collision.CompareTag("Bullet"))
        {
            enemiesInMe.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) // take out enemies that exits the slash path
    {
        if (collision.CompareTag("Enemy") ||
            collision.CompareTag("Bullet"))
        {
            enemiesInMe.Remove(collision.gameObject);
        }
    }
    public int HowManyEnemiesIHit() // return enemies inside the slash path
    {
        return enemiesInMe.Count;
    }
}
