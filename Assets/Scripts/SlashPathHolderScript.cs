using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashPathHolderScript : MonoBehaviour
{
    public SlashPathCollisionScript myImage;
    [HideInInspector]
    public float baseScaleY;
    private float ogBaseScaleY;
    [Header("ENEMY BOOSTs")]
    public float extentionMultiplyer;
    [Header("CHARGEs")]
    public float maxScaleY;
    public float chargePower;
    // bounce
    private Ray2D ray;
    private Ray2D rayL;
    private Ray2D rayR;
    private bool rayHit = false;
    private bool rayHitL = false;
    private bool rayHitR = false;
    private bool triggerHit = false;
    public Transform rayPos_left;
    public Transform rayPos_right;
    public GameObject raycaster; // the game object that casts the rays (so that when the rays r detecting collision they can ignore the caster)
    private GameObject reflectionObj; // the game object that the refleciton is happening on
    private Vector3 reflectionPoint; // origin of the reflection
    private Vector3 newPathHolder_Dir;
    private float scaleDifference;
    public GameObject newPathHolder_holder;
    public GameObject newPathHolder;
    public bool imNew;
    private void Start()
    {
        baseScaleY = transform.localScale.y; // get initial scale.y
        ogBaseScaleY = baseScaleY;
    }
    private void Update()
    {
        ShortenPath();
    }
    private void FixedUpdate()
    {
        //Raycasts();
    }
    private float ReturnLength2Add_enemyBoost() // calculate and return scaleY to add based on enemies inside the slash path
    {
        if (GameManager.me.enemyBoost)
        {
            return myImage.HowManyEnemiesIHit() * extentionMultiplyer;
        }
        return 0f;
    }
    private float ReturnLength2Add_charge() // calculate and return scaleY to add based on drag time
    {
        if (GameManager.me.charge)
        {
            // get the rotio between time hold and time max, that's the ratio between added length and max length
            float length2Add = InteractionScript.me.mouseDrag_time * maxScaleY / InteractionScript.me.mouseDrag_maxTime;
            return Mathf.Pow(length2Add, chargePower);
        }
        return 0f;
    }
    private void Raycasts()
    {
        LayerMask layer = LayerMask.GetMask("DetectSlashRay");
        if (InteractionScript.me.dragging)
        {
            // left ray
            rayL = new Ray2D(rayPos_left.position, transform.up);
            RaycastHit2D hitL = Physics2D.Raycast(rayL.origin,  // origin of raycast
                rayL.direction, // raycast direciton
                transform.localScale.y / 2f - .1f, // raycast length
                layer); // layer to detect
            // left long ray
            //RaycastHit2D hitL_long = Physics2D.Raycast(rayL.origin,  // origin of raycast
            //    rayL.direction, // raycast direciton
            //    transform.localScale.y / 2f, // raycast length
            //    layer); // layer to detect
            Debug.DrawLine(rayL.origin, rayL.origin + (rayL.direction * transform.localScale.y / 2), Color.magenta);
            // right ray
            rayR = new Ray2D(rayPos_right.position, transform.up);
            RaycastHit2D hitR = Physics2D.Raycast(rayR.origin,  // origin of raycast
               rayR.direction, // raycast direciton
               transform.localScale.y / 2f - .1f, // raycast length
               layer); // layer to detect
            Debug.DrawLine(rayR.origin, rayR.origin + (rayR.direction * transform.localScale.y / 2), Color.magenta);
            // center ray
            ray = new Ray2D(transform.position, transform.up);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin,  // origin of raycast
                ray.direction, // raycast direciton
                transform.localScale.y / 2f - .1f, // raycast length
                layer); // layer to detect
            Debug.DrawLine(ray.origin, ray.origin + (ray.direction * transform.localScale.y / 2), Color.magenta);
            // set ray hit bools
            if ((hit.collider && hit.collider != raycaster)
                || raycaster == null)
            {
                if (hit.collider.GetComponent<EnemyScript>().shielded)
                {
                    rayHit = hit.collider;
                    triggerHit = GetComponentInChildren<BoxCollider2D>().IsTouchingLayers(layer);
                }
            }
            if (hitL.collider && hit.collider != raycaster
                || raycaster == null)
            {
                if (hitL.collider.GetComponent<EnemyScript>().shielded)
                {
                    rayHitL = hitL.collider;
                    triggerHit = GetComponentInChildren<BoxCollider2D>().IsTouchingLayers(layer);
                }
            }
            if (hitR.collider && hit.collider != raycaster
                || raycaster == null)
            {
                if (hitR.collider.GetComponent<EnemyScript>().shielded)
                {
                    rayHitR = hitR.collider;
                    triggerHit = GetComponentInChildren<BoxCollider2D>().IsTouchingLayers(layer);
                }
            }
            // get reflection object and point, do it for each raycast
            /*
            if (rayHit && hit.collider)
            {
                reflectionObj = hit.collider.gameObject;
                reflectionPoint = hit.point;
            }
            else if (hitL_long)
            {
                reflectionObj = hitL_long.collider.gameObject;
                reflectionPoint = hitL_long.point;
                Vector3 normal = hitL_long.normal;
                newPathHolder_Dir = Vector3.Reflect(ray.direction, normal);
                if (newPathHolder_holder != null)
                {
                    newPathHolder_holder.SetActive(true);
                    newPathHolder_holder.transform.position = reflectionPoint ;
                    float newPathHoldersHolder_ScaleY = scaleDifference;
                    newPathHoldersHolder_ScaleY = Mathf.Clamp(newPathHoldersHolder_ScaleY, 0.01f, float.MaxValue);
                    newPathHolder.transform.localScale = new Vector3(1.4f, newPathHoldersHolder_ScaleY, 1);
                    newPathHolder_holder.GetComponentInChildren<SlashPathHolderScript>().imNew = true;
                    newPathHolder_holder.GetComponentInChildren<SlashPathHolderScript>().raycaster = reflectionObj;
                }
            }
            else if (rayHitR)
            {
                reflectionObj = hitR.collider.gameObject;
                reflectionPoint = hitR.point;
            }
            else
            {
                if (newPathHolder_holder != null)
                {
                    //newPathHolder_holder.SetActive(false);
                }
            }*/
        }
    }
    private void ShortenPath()
    {
        if (!imNew)
        {
            // change scale.y
            transform.localScale = new Vector3(transform.localScale.x,
                    baseScaleY + ReturnLength2Add_enemyBoost() + ReturnLength2Add_charge(),
                    1);
        }
        if ((rayHit || rayHitL || rayHitR)
                && triggerHit)
            {
                baseScaleY *= .995f;
            }
        else if (!rayHit
            && !rayHitL
            && !rayHitR
            && triggerHit)
        {

        }
        else if (!rayHit
            && !rayHitL
            && !rayHitR
            && !triggerHit)
        {
            if (baseScaleY < ogBaseScaleY)
            {
                baseScaleY *= 1.009f;
            }
            else
            {
                baseScaleY = ogBaseScaleY;
            }
        }
        // calculate how much scale shorten
        scaleDifference = ogBaseScaleY - baseScaleY;
    }
}
