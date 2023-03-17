using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashPathHolderScript : MonoBehaviour
{
    public float extentionMultiplyer;
    private float baseScaleY;
    public SlashPathCollisionScript myImage;
    private void Start()
    {
        baseScaleY = transform.localScale.y;
    }
    private void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x, baseScaleY + myImage.HowManyEnemiesIHit() * extentionMultiplyer, 1);
    }
}
