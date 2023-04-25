using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("CAM SHAKEs")]
    public MilkShake.Shaker camShaker;
    public MilkShake.ShakePreset enemyHit;
    public MilkShake.ShakePreset playerHit;
    [Header("CHANGE SIZEs")]
    private Camera cam;
    private float ogSize;
    public float enlargeMultiplyer;
    public float enlargeSpd;
    #region SINGLETON
    public static CameraScript me;
    private void Awake()
    {
        me = this;
    }
    #endregion
    private void Start()
    {
        ogSize = GetComponent<Camera>().orthographicSize;
        cam = GetComponent<Camera>();
    }
    private void Update()
    {
        IncreaseCamSize();
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 4f, float.MaxValue);
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
            ogSize + enlargeMultiplyer * (PlayerScript.me.slashPath.transform.localScale.y), 
            Time.deltaTime * enlargeSpd) ;
    }
}