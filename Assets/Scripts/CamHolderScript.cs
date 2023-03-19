using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamHolderScript : MonoBehaviour
{
    [Header("FOLLOW")]
    public Transform camTarget_wZ;
    private Vector3 camTargetPos_woZ;
    public float smoothTime;
    private Vector3 velocity = Vector3.zero;
    #region SINGLETON
    public static CamHolderScript me;
    private void Awake()
    {
        me = this;
    }
    #endregion
    private void Update()
    {
        if (GameManager.me.gameState == GameManager.GameState.play) // if playing
        {
            camTargetPos_woZ = new(camTarget_wZ.position.x, camTarget_wZ.position.y, -10); // set z
            transform.position = Vector3.SmoothDamp(transform.position, camTargetPos_woZ, ref velocity, smoothTime); // smooth damp it to target pos
        }
    }
}
