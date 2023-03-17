using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakerChildScript : MonoBehaviour
{
    public GameObject targetPos;
    public SnakerScript ss;
    public float smoothTime;
    Vector3 velocity = new();
    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPos.transform.position, ref velocity,  smoothTime);
    }
}
