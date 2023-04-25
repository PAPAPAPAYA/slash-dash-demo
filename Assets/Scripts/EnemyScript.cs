using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyScript : MonoBehaviour
{
    public enum EnemyType
    {
        brawler,
        slimer,
        shooter,
        bullet,
        score,
        restart
    };
    [Header("BASICs")]
    public int hp;
    public float moveSpd;
    public float hurt_stunDuration;
    private bool hurt_stunned;
    public float spawn_iFrame;
    public EnemyType myEnemyType;
    public bool shielded;
    public float rotSpd;
    [Header("SLIMERs")]
    public List<GameObject> slimees;
    public float slimee_spawnForce;
    [Header("SHOOTERs")]
    public float shoot_interval;
    private float shoot_timer;
    public float shooter_stopDis;
    public GameObject bulletPrefab;
    [Header("SHADOWs")]
    public GameObject myShadow;
    public float shadow_xOffset;
    public float shadow_yOffset;
    [Header("HP INDICATORs")]
    public GameObject prefab_indicatorHp;
    public List<GameObject> myHpIndicators;
    public float gap_indicatorHp;
    private int hpIndicators_Showed;
    [Header("HIT VFXs")]
    public GameObject myImg;
    private Material ogMat;
    public Material hurtMat;
    public float flashDuration;
    public GameObject PS_blood;
    public float hurt_bulletTime_scale;
    public float hurt_bulletTime_duration;
    [Header("FOR PLAYTESTs")]
    public bool undying;
    private void Start()
    {
        myHpIndicators = new();
        ogMat = myImg.GetComponent<SpriteRenderer>().material;
        shoot_timer = Random.Range(2, shoot_interval);
        //!MakeHpIndicators();
    }
    private void Update()
    {
        //!UpdateHpIndicator();
        MoveLogic();
        if (spawn_iFrame > 0) // i-frames when spawned so that it won't get killed the moment it is spanwed
        {
            spawn_iFrame -= Time.deltaTime;
        }
        if (!PlayerScript.me.slashing)
        {
            FacePlayer();
        }
        Shoot();
        ControlShadow();
        if (GameManager.me.gameState == GameManager.GameState.over)
        {
            if (myEnemyType == EnemyType.score)
            {
                Destroy(gameObject);
            }
            else if (myEnemyType == EnemyType.restart)
            {

            }
            else
            {
                GetHit(1);
            }
        }
    }
    private void Shoot()
    {
        if (myEnemyType == EnemyType.shooter)
        {
            if (shoot_timer > 0)
            {
                shoot_timer -= Time.deltaTime;
            }
            else
            {
                shoot_timer = shoot_interval;
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
            }
        }
    }
    private void MoveLogic()
    {
        if (!hurt_stunned)
        {
            if (myEnemyType == EnemyType.bullet) // if i'm a bullet
            {
                // go straight ahead
                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, moveSpd * Time.deltaTime);
            }
            else if (myEnemyType == EnemyType.shooter) // if i'm a shooter
            {
                // stop at a distance
                if (Vector3.Distance(PlayerScript.me.transform.position, transform.position) > shooter_stopDis)
                {
                    transform.position = Vector3.MoveTowards(transform.position, PlayerScript.me.transform.position, moveSpd * Time.deltaTime);
                }
            }
            else // if i'm anything else
            {
                // chase player
                transform.position = Vector3.MoveTowards(transform.position, PlayerScript.me.transform.position, moveSpd * Time.deltaTime);
            }
        }
    }
    public void GetHit(int amount)
    {
        if (spawn_iFrame <= 0 &&
            hp > 0)
        {
            if (!undying)
            {
                hp -= amount;
            }
            All_Hit_VFXs();
        }
    }
    IEnumerator HurtStun()
    {
        hurt_stunned = true;
        yield return new WaitForSecondsRealtime(hurt_stunDuration);
        hurt_stunned = false;
    }
    private void Die()
    {
        if (hp <= 0)
        {
            switch (myEnemyType)
            {
                case EnemyType.brawler:
                    ShootOutCorpse(GameManager.me.scorePrefab, GameManager.me.score_spawnForce);
                    break;
                case EnemyType.slimer:
                    foreach (var slimee in slimees)
                        ShootOutCorpse(slimee, slimee_spawnForce);
                    break;
                case EnemyType.shooter:
                    ShootOutCorpse(GameManager.me.scorePrefab, GameManager.me.score_spawnForce);
                    break;
                case EnemyType.bullet:
                    break;
                case EnemyType.score:
                    GameManager.me.score++;
                    break;
                case EnemyType.restart:
                    GameManager.me.RestartGame();
                    GameManager.me.gameState = GameManager.GameState.play;
                    transform.localPosition = new Vector3(1.7f, -1.7f, 1);
                    hp = 1;
                    break;
                default:
                    break;
            }
            Time.timeScale = 1;
            if (myEnemyType != EnemyType.restart)
            {
                Destroy(gameObject);
            }
        }
    }
    private void ShootOutCorpse(GameObject obj2Shoot, float spawnForce)
    {
        GameObject obj = Instantiate(obj2Shoot);
        float randX = Random.Range(-1f, 1f);
        float randY = Random.Range(-1f, 1f);
        Vector3 randDir = new(randX, randY, 0);
        randDir = randDir.normalized;
        obj.transform.position = transform.position;
        obj.GetComponent<Rigidbody2D>().AddForce(randDir * spawnForce, ForceMode2D.Impulse);
    }
    public void RestartButton_Init()
    {
        hp = 1;
        transform.localPosition = new Vector3(1.7f, -1.7f, 1);
        myImg.transform.localPosition = new Vector3(0, 0, 0);
        myShadow.transform.localPosition = new Vector3(.2f, -.2f, 1);
    }
    private void ControlShadow()
    {
        // shadow keeps a constant relative position
        myShadow.transform.position = new Vector3(transform.position.x + shadow_xOffset, transform.position.y + shadow_yOffset, 1);
        // shadow mimics rotation
        myShadow.transform.rotation = transform.rotation;
    }
    private void FacePlayer()
    {
        if (myEnemyType != EnemyType.bullet && 
            myEnemyType != EnemyType.score)
        {
            Vector3 dir = PlayerScript.me.transform.position - transform.position;
            dir = dir.normalized;
            var targetRot = Quaternion.LookRotation(Vector3.forward, dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpd * Time.deltaTime);
        }
    }
    private void MakeHpIndicators()
    {
        for (int i = 0; i < hp; i++)
        {
            GameObject indicator = Instantiate(prefab_indicatorHp);
            indicator.transform.position = new Vector3(transform.position.x, transform.position.y + .8f + gap_indicatorHp * i, 0f);
            myHpIndicators.Add(indicator);
            hpIndicators_Showed++;
            indicator.transform.SetParent(gameObject.transform);
        }
    }
    private void UpdateHpIndicator()
    {
        if (hpIndicators_Showed > hp)
        {
            for (int i = myHpIndicators.Count - 1; i > 0; i--)
            {
                if (myHpIndicators[i].activeSelf)
                {
                    myHpIndicators[i].SetActive(false);
                    hpIndicators_Showed--;
                    if (hpIndicators_Showed == hp) break;
                }
            }
        }
        else if (hpIndicators_Showed < hp)
        {
            foreach (var indicator in myHpIndicators)
            {
                if (!indicator.activeSelf)
                {
                    indicator.SetActive(true);
                    hpIndicators_Showed++;
                    if (hpIndicators_Showed == hp) break;
                }
            }
        }
    }
    #region HIT VFXs
    private void All_Hit_VFXs()
    {
        CameraScript.me.CamShake_EnemyHit();
        GameObject ps = Instantiate(PS_blood);
        ps.transform.position = transform.position;
        StartCoroutine(HurtStun());
        HitFlash();
        if (PlayerScript.me.hitBulletTime)
        {
            StartCoroutine(Hurt_BulletTime());
        }
        else
        {
            Die();
        }
    }
    private void HitFlash()
    {
        myImg.GetComponent<SpriteRenderer>().material = hurtMat;
        StartCoroutine(Back2OgMat());
    }
    IEnumerator Back2OgMat()
    {
        yield return new WaitForSecondsRealtime(flashDuration);
        myImg.GetComponent<SpriteRenderer>().material = ogMat;
    }
    IEnumerator Hurt_BulletTime()
    {
        Time.timeScale = hurt_bulletTime_scale;
        yield return new WaitForSecondsRealtime(hurt_bulletTime_duration);
        Die();
        Time.timeScale = 1;
    }
    #endregion
}
