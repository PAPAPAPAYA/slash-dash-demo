using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public static CameraScript me;
    [Header("CAM SHAKEs")]
    public MilkShake.Shaker camShaker;
    public MilkShake.ShakePreset enemyHit;
    public MilkShake.ShakePreset playerHit;
    [Header("CHANGE SIZEs")]
    private Camera cam;
    private float ogSize;
    public float enlargeMultiplyer;
    public float enlargeSpd;
    private void Awake()
    {
        me = this;
    }
    private void Start()
    {
        ogSize = GetComponent<Camera>().orthographicSize;
        cam = GetComponent<Camera>();
    }
    private void Update()
    {
        IncreaseCamSize();
    }
    public void CamShake_EnemyHit()
    {
        camShaker.Shake(enemyHit);
    }
    public void CamShake_PlayerHit()
    {
        camShaker.Shake(playerHit);
    }
    public void IncreaseCamSize()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 
            ogSize + enlargeMultiplyer * PlayerScript.me.slashPath.transform.localScale.y, 
            Time.deltaTime * enlargeSpd) ;
    }
}