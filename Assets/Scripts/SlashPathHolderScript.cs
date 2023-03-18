using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashPathHolderScript : MonoBehaviour
{
    public float extentionMultiplyer;
    private float baseScaleY;
    public SlashPathCollisionScript myImage;
    public float maxScaleY;
    public float chargePower;
    private void Start()
    {
        baseScaleY = transform.localScale.y;
    }
    private void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x,
                baseScaleY + ReturnLength2Add_enemyBoost() + ReturnLength2Add_charge(),
                1);
    }
    private float ReturnLength2Add_enemyBoost()
    {
        if (GameManager.me.enemyBoost)
        {
            return myImage.HowManyEnemiesIHit() * extentionMultiplyer;
        }
        return 0f;
    }
    private float ReturnLength2Add_charge()
    {
        if (GameManager.me.charge)
        {
            // get the rotio between time hold and time max, that's the ratio between added length and max length
            float length2Add = InteractionScript.me.mouseDrag_time * maxScaleY / InteractionScript.me.mouseDrag_maxTime;
            return Mathf.Pow(length2Add, chargePower);
        }
        return 0f;
    }
}
