using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! this script stores player functions
public class PlayerScript : MonoBehaviour
{
    public static PlayerScript me;
    [Header("BASICs")]
    public int atk;
    [Header("SLASHes")]
    public GameObject endOfPath;
    public Vector3 slashTargetPos;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime;
    public bool slashing;
    public bool slashIntiated;
    public GameObject slashPath;
    [Header("HIT STUNs")]
    public float hitStun_duration;
    private float hitStun_timer;
    public bool hitStunned;
    [Header("VISUALs")] 
    public GameObject imagePlayer;
    public GameObject shadowPlayer;
    public Vector3 rotSpd;
    public Vector3 rotSpd_current;
    private Vector3 rotVelocity;
    public float warmUp_smoothTime;
    [Header("HITs")]
    public float hitStop_duration;
    private GameObject enemyHit;
    [Header("HURTs")]
    public Collider2D hitBox;
    public float flashDuration;
    private Material ogMat;
    public Material hurtMat;
    public float hurt_bulletTime_scale;
    public float hurt_bulletTime_duration;
    public GameObject ps_blood;
    [Header("FOR PLAYTESTs")]
    public bool hitBulletTime;
    public bool hitStop;

    private void Awake()
    {
        me = this;
    }
    private void Start()
    {
        ogMat = imagePlayer.GetComponent<SpriteRenderer>().material;
        hitStunned = false;
    }
    private void Update()
    {
        if (!slashIntiated)
        {
            slashTargetPos = transform.position;
        }
        transform.position = Vector3.SmoothDamp(transform.position, slashTargetPos, ref velocity, smoothTime);
        slashing = Vector3.Distance(transform.position, slashTargetPos) > 0.1f;
        if (slashing)
        {
            SlashRotate();
        }
        else
        {
            slashIntiated = false;
            if (InteractionScript.me.dragging)
            {
                WarmUp();
            }
            else
            {
                rotSpd_current = new(0, 0, 0);
                ReturnRotate();
            }
        }
        // hit stun cd
        if (hitStun_timer > 0)
        {
            hitStun_timer -= Time.deltaTime;
            hitStunned = true;
        }
        else
        {
            hitStunned = false;
        }
    }
    private void SlashRotate()
    {
        imagePlayer.transform.Rotate(rotSpd * Time.deltaTime);
        shadowPlayer.transform.Rotate(rotSpd * Time.deltaTime);
    }
    private void ReturnRotate()
    {
        Quaternion target = Quaternion.Euler(0, 0, 45);
        imagePlayer.transform.rotation = Quaternion.RotateTowards(imagePlayer.transform.rotation, target, Time.deltaTime * -500f);
        shadowPlayer.transform.rotation = Quaternion.RotateTowards(shadowPlayer.transform.rotation, target, Time.deltaTime * -500f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (slashing)
        {
            if ((collision.CompareTag("Bullet") ||
                collision.CompareTag("Enemy")) &&
                hitBox.IsTouching(collision))
            {
                collision.GetComponent<EnemyScript>().GetHit(atk);
                enemyHit = collision.gameObject; 
                if (hitStop)
                {
                    StartCoroutine(HitStop());
                }
            }
        }
        else // not slashing
        {
            if (collision.CompareTag("Enemy") &&  // got hit by an enemy
                collision.GetComponent<EnemyScript>().myEnemyType != EnemyScript.EnemyType.score &&
                hitBox.IsTouching(collision)) // check if the actual hit box got hit and not other colliders
            {
                GotHit();
            }
            else if (collision.CompareTag("Bullet") &&
                hitBox.IsTouching(collision))
            {
                GotHit();
                Destroy(collision.gameObject);
            }
        }
    }
    private void GotHit()
    {
        GameManager.me.playerHp--;
        KnockBackAreaScript.me.KnockBackEnemies();
        StartCoroutine(HitFlash());
        CameraScript.me.CamShake_PlayerHit();
        StartCoroutine(Hurt_BulletTime());
        GameObject psBlood = Instantiate(ps_blood);
        psBlood.transform.position = transform.position;
        hitStun_timer = hitStun_duration;
        if (GameManager.me.playerHp <= 0)
        {
            transform.position = new Vector3(CamHolderScript.me.transform.position.x, CamHolderScript.me.transform.position.y, 0);
        }
    }
    #region HIT VFXs
    IEnumerator HitFlash()
    {
        imagePlayer.GetComponent<SpriteRenderer>().material = hurtMat;
        yield return new WaitForSecondsRealtime(flashDuration);
        imagePlayer.GetComponent<SpriteRenderer>().material = ogMat;
    }
    IEnumerator Hurt_BulletTime()
    {
        Time.timeScale = hurt_bulletTime_scale;
        yield return new WaitForSecondsRealtime(hurt_bulletTime_duration);
        Time.timeScale = 1;
    }
    #endregion
    #region ATK VFXs
    public void WarmUp()
    {
        if (rotSpd_current.magnitude < rotSpd.magnitude)
        {
            rotSpd_current = Vector3.SmoothDamp(rotSpd_current, rotSpd, ref rotVelocity, warmUp_smoothTime);
        }
        imagePlayer.transform.Rotate(rotSpd_current * Time.deltaTime);
        shadowPlayer.transform.Rotate(rotSpd_current * Time.deltaTime);
    }
    IEnumerator HitStop()
    {
        Vector3 ogTargetPos = slashTargetPos;
        slashTargetPos = enemyHit.transform.position;
        rotSpd = new(0, 0, 0);
        yield return new WaitForSecondsRealtime(hitStop_duration);
        rotSpd = new(0, 0, 5);
        slashTargetPos = ogTargetPos;
    }
    #endregion
}
