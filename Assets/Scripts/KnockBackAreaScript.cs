using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackAreaScript : MonoBehaviour
{
    public float knockback_amount;
    public float knockBack_amount_weak;
    public List<GameObject> enemiesInArea;
    #region SINGLETON
    public static KnockBackAreaScript me;
    private void Awake()
    {
        me = this;
    }
    #endregion
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") ||
            collision.CompareTag("Bullet"))
        {
            enemiesInArea.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") ||
            collision.CompareTag("Bullet"))
        {
            if (enemiesInArea.Contains(collision.gameObject))
            {
                enemiesInArea.Remove(collision.gameObject);
            }
        }
    }
    public void KnockBackEnemies()
    {
        foreach (var enemy in enemiesInArea)
        {
            Vector3 knockBack_dir = (enemy.transform.position - transform.position).normalized;
            Vector3 knockBack_force = knockBack_dir * knockback_amount;
            if (enemy.GetComponent<Rigidbody2D>() &&
                enemy.GetComponent<EnemyScript>().myEnemyType != EnemyScript.EnemyType.score &&
                enemy.GetComponent<EnemyScript>().myEnemyType != EnemyScript.EnemyType.restart)
            {
                enemy.GetComponent<Rigidbody2D>().AddForce(knockBack_force, ForceMode2D.Impulse);
            }
            if (enemy.GetComponent<EnemyScript>().myEnemyType == EnemyScript.EnemyType.bullet)
            {
                enemy.GetComponent<EnemyScript>().GetHit(1);
            }
        }
    }
    public void KnockBackEnemies_Weak()
    {
        foreach (var enemy in enemiesInArea)
        {
            Vector3 knockBack_dir = (enemy.transform.position - transform.position).normalized;
            Vector3 knockBack_force = knockBack_dir * knockBack_amount_weak;
            if (enemy.GetComponent<Rigidbody2D>() &&
                enemy.GetComponent<EnemyScript>().myEnemyType != EnemyScript.EnemyType.score &&
                enemy.GetComponent<EnemyScript>().myEnemyType != EnemyScript.EnemyType.restart)
            {
                enemy.GetComponent<Rigidbody2D>().AddForce(knockBack_force, ForceMode2D.Impulse);
            }
            if (enemy.GetComponent<EnemyScript>().myEnemyType == EnemyScript.EnemyType.bullet)
            {
                enemy.GetComponent<EnemyScript>().GetHit(1);
            }
        }
    }
}
