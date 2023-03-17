using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakerScript : MonoBehaviour
{
    public float moveSpd;

    private void Update()
    {
        Vector3 lookDir = (PlayerScript.me.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, lookDir);
        transform.position = Vector3.MoveTowards(transform.position, PlayerScript.me.transform.position, Time.deltaTime * moveSpd);
    }
}
