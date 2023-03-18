using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamHolderScript : MonoBehaviour
{
    public static CamHolderScript me;
    [Header("FOLLOW")]
    public Transform camTarget_wZ;
    private Vector3 camTargetPos_woZ;
    public float smoothTime;
    private Vector3 velocity = Vector3.zero;
    private void Awake()
    {
        me = this;
    }
    private void Update()
    {
        if (GameManager.me.gameState == GameManager.GameState.play)
        {
            camTargetPos_woZ = new(camTarget_wZ.position.x, camTarget_wZ.position.y, -10);
            transform.position = Vector3.SmoothDamp(transform.position, camTargetPos_woZ, ref velocity, smoothTime);
        }
    }
}
