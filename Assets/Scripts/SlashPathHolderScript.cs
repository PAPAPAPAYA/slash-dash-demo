using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashPathHolderScript : MonoBehaviour
{
    public float extentionMultiplyer;
    private float baseScaleY;
    public SlashPathCollisionScript myImage;
    public float startLength;
    public float maxLength;
    private void Start()
    {
        baseScaleY = transform.localScale.y;
    }
    private void Update()
    {
        if (GameManager.me.charge)
        {
            // charge mechanic
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, 
                baseScaleY + myImage.HowManyEnemiesIHit() * extentionMultiplyer, 
                1);
        }
    }
    private void Charge()
    {
        // get the rotio between time hold and time max, that's the ratio between added length and max length
        // add added length to start length = current length
    }
}
