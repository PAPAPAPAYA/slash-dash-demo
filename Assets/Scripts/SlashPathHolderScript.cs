using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashPathHolderScript : MonoBehaviour
{
    public SlashPathCollisionScript myImage;
    //[HideInInspector]
    public float baseScaleY;
    [Header("ENEMY BOOSTs")]
    public float extensionMultiplyer;
    private float boostBuffer;
    [Header("CHARGEs")]
    public float maxScaleY;
    public float chargePower;
    [Header("REFLECTIONs")]
    public float ogBaseScaleY;
    public float extensionFactor;
    private Ray2D ray; // this ray is for shortening the path when it hits a reflectable object
    private Ray2D rayLong; // this ray is for calculating the reflection angle
    private bool rayHit = false; // this tracks if the short ray hits
    public bool rayHit_long = false; // this tracks if the long ray hits
    public GameObject raycaster; // the game object that casts the rays (so that when the rays r detecting collision they can ignore the caster)
    public GameObject fatherPath_holder; // the path that holds this path
    public GameObject reflectionObj; // the game object that the refleciton is happening on
    private Vector3 reflectionPoint; // origin of the reflection
    private Vector3 newPathHolder_Dir; // the direction of this path's path's holder
    public float scaleDifference; // store the scale difference when path is shrunk, and this scale is applied to this path's path's holder
    public GameObject pathHolder_holder_prefab; // when reflection happens, if this path doesn't have a child slash path holder, instantiate one
    public GameObject newPathHolder_holder; // this path's path's holder's holer
    public GameObject newPathHolder; // this path's path's holder
    private void Start()
    {
        //print(transform.localScale.y);
        baseScaleY = transform.localScale.y; // get initial scale.y
        ogBaseScaleY = baseScaleY;
    }
    private void Update()
    {
        ChangePathLength();
        Raycasts();
        if (myImage.gameObject.activeSelf)
        {
            if (!ReflectionSlashScript.me.currentlyActivePaths.Contains(gameObject))
                ReflectionSlashScript.me.currentlyActivePaths.Add(gameObject);
        }
        else
        {
            if (ReflectionSlashScript.me.currentlyActivePaths.Contains(gameObject))
                ReflectionSlashScript.me.currentlyActivePaths.Remove(gameObject);
        }
    }
    private void FixedUpdate()
    {
        
    }
    private void LateUpdate()
    {
        
    }
    private float ReturnLength2Add_enemyBoost() // calculate and return scaleY to add based on enemies inside the slash path
    {
        if (GameManager.me.enemyBoost)
        {
            float targetValue = myImage.HowManyEnemiesIHit() * extensionMultiplyer;
            if (boostBuffer < targetValue)
            {
                boostBuffer = targetValue;
            }
            else if (boostBuffer > targetValue)
            {
                boostBuffer -= 20 * Time.deltaTime;
            }
            return boostBuffer;
        }
        return 0f;
    }
    private float ReturnLength2Add_charge() // calculate and return scaleY to add based on drag time
    {
        if (GameManager.me.charge
            && raycaster==null)
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
        float raycastLength = transform.localScale.y / 2f;
        raycastLength = Mathf.Clamp(raycastLength, 0, float.MaxValue);
        if (InteractionScript.me.dragging)
        {
            // center ray
            ray = new Ray2D(transform.position, transform.up);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin,  // origin of raycast
                ray.direction, // raycast direciton
                raycastLength - .2f, // raycast length
                layer); // layer to detect
            Debug.DrawLine(ray.origin, ray.origin + (ray.direction * transform.localScale.y / 2f), Color.magenta);
            // center long ray
            rayLong = new Ray2D(transform.position, transform.up);
            RaycastHit2D[] hits_long = Physics2D.RaycastAll(rayLong.origin,  // origin of raycast
                rayLong.direction, // raycast direciton
                raycastLength + 0f, // raycast length
                layer); // layer to detect
            Debug.DrawLine(rayLong.origin, rayLong.origin + (rayLong.direction * transform.localScale.y / 2f), Color.magenta);
            // set ray hit bools
            int valid_hit_amount_short = 0;
            RaycastHit2D valid_hit_short;
            foreach (var hit in hits)
            {
                if (hit.collider
                    && hit.collider.GetComponent<EnemyScript>().shielded) // the short ray is for shortening the path
                {
                    if (raycaster == null
                    || (raycaster != null && hit.collider.gameObject != raycaster))
                    {
                        valid_hit_amount_short++;
                        valid_hit_short = hit;
                    }
                }
            }
            rayHit = valid_hit_amount_short > 0;
            int valid_hit_amount_long = 0;
            RaycastHit2D valid_hit_long;
            foreach (var hit in hits_long)
            {
                if (hit.collider
                    && hit.collider.GetComponent<EnemyScript>().shielded) // the long ray is for extending the path(and not adjusting the path), and detecting reflection
                {
                    if (raycaster == null
                    || (raycaster != null && hit.collider.gameObject != raycaster))
                    {
                        valid_hit_amount_long++;
                        valid_hit_long = hit;
                        reflectionObj = valid_hit_long.collider.gameObject;
                        reflectionPoint = valid_hit_long.point;
                        Vector3 normal = valid_hit_long.normal;
                        newPathHolder_Dir = Vector3.Reflect(rayLong.direction, normal);
                        if (pathHolder_holder_prefab != null)
                        {
                            if (newPathHolder_holder == null)
                            {
                                newPathHolder_holder = Instantiate(pathHolder_holder_prefab);
                                newPathHolder = newPathHolder_holder.GetComponentInChildren<SlashPathHolderScript>().gameObject;
                                newPathHolder.GetComponent<SlashPathHolderScript>().fatherPath_holder = transform.parent.gameObject;
                            }
                            else
                            {
                                float newPathHoldersHolder_ScaleY = scaleDifference * extensionFactor;
                                newPathHoldersHolder_ScaleY = Mathf.Clamp(newPathHoldersHolder_ScaleY, 0.001f, float.MaxValue);
                                newPathHolder.transform.localScale = new Vector3(1.4f, newPathHoldersHolder_ScaleY, 1);
                                newPathHolder.GetComponent<SlashPathHolderScript>().ogBaseScaleY = newPathHoldersHolder_ScaleY;
                                newPathHolder.GetComponent<SlashPathHolderScript>().myImage.gameObject.SetActive(true);
                                newPathHolder_holder.transform.position = reflectionPoint;
                                newPathHolder.transform.rotation = Quaternion.LookRotation(Vector3.forward, newPathHolder_Dir);
                                newPathHolder.GetComponent<SlashPathHolderScript>().raycaster = reflectionObj;
                            }
                        }
                    }
                }
            }
            rayHit_long = valid_hit_amount_long > 0;
            if (fatherPath_holder!=null
                && (!fatherPath_holder.GetComponentInChildren<SlashPathHolderScript>().rayHit_long
                || !fatherPath_holder.activeSelf)) // if my father is not rayhit_long
            {
                myImage.gameObject.SetActive(false); // hide myself
                if (newPathHolder != null) newPathHolder.GetComponent<SlashPathHolderScript>().myImage.gameObject.SetActive(false); // hide my child path
            }
            if (!rayHit_long) // if i'm not rayhit_long
            {
                if (newPathHolder != null)
                {
                    newPathHolder.GetComponent<SlashPathHolderScript>().rayHit_long = false; // my child path can't be rayhit_long
                }
            }
        }
        else
        {
            if (raycaster!=null
                || ( fatherPath_holder != null
                && !fatherPath_holder.activeSelf))
            {
                myImage.gameObject.SetActive(false);
            }
        }
    }
    private void ChangePathLength()
    {
        // change scale.y
        transform.localScale = new Vector3(transform.localScale.x,
                baseScaleY + ReturnLength2Add_enemyBoost() + ReturnLength2Add_charge(),
                1);
        if (rayHit)
        {
            baseScaleY -= 10f * Time.deltaTime;
        }
        else if (!rayHit
            && rayHit_long)
        {

        }
        else if (!rayHit
            && !rayHit_long)
        {
            if (baseScaleY < ogBaseScaleY)
            {
                baseScaleY += 10f * Time.deltaTime;
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
