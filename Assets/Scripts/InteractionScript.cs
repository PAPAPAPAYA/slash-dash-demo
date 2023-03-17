using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! this script is for implementing mouse interaction
public class InteractionScript: MonoBehaviour
{
    public static InteractionScript me;
    [Header("REFs")]
    public GameObject aimArea;
    public GameObject slashPath;
    public GameObject imageSlashPath;
    public GameObject player;
    // MOUSE DATAs
    Vector3 mouseDown_pos;
    Vector3 mouseCurrent_pos;
    Vector3 mouseDir;
    float mouseDrag_time;
    [HideInInspector]
    public bool dragging = false;
    [Header("SILHOUETTEs")]
    public int sil_amount;
    public float mouseDrag_maxTime;
    public GameObject sil_prefab;
    public float sil_baseDis;
    public List<GameObject> sils;
    public float sil_returnSpd;
    public float sil_disMultiplyer;
    private float time2ReachPlayer;
    private void Awake()
    {
        me = this;
    }

    private void Start()
    {
        mouseCurrent_pos = new();
        imageSlashPath.GetComponent<SpriteRenderer>().enabled = false; // hide slash path at start
        time2ReachPlayer = PlayerScript.me.smoothTime;
        dragging = false;
        MakeSils();
    }

    private void Update()
    {
        mouseDir = (mouseCurrent_pos - mouseDown_pos).normalized; // calculate direction between initial point and current point
        Debug.DrawLine(mouseCurrent_pos, mouseDown_pos); 
        Debug.DrawLine(slashPath.transform.position, mouseDir + slashPath.transform.position);
        switch (GameManager.me.gameMode) // switch game mode related mechanics
        {
            case GameManager.GameMode.slingshot:
                slashPath.transform.rotation = Quaternion.LookRotation(Vector3.forward, -mouseDir); // rotate slash path so that it faces oppositie mouse direction
                if (!dragging)
                {
                    CollectSilhouette(); // if slingshot, silhouette
                }
                break;
            case GameManager.GameMode.forward:
                slashPath.transform.rotation = Quaternion.LookRotation(Vector3.forward, mouseDir); // rotate slash path so that it faces mouse direction
                break;
            default:
                break;
        }
    }

    private void OnMouseDrag()
    {
        mouseCurrent_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // convert and store mouse position
        mouseCurrent_pos.z = 0; // zero out z position
        if ((mouseCurrent_pos - mouseDown_pos).magnitude > 0.1f) // check if any meaningful input is detected
        {
            imageSlashPath.GetComponent<SpriteRenderer>().enabled = !PlayerScript.me.hitStunned; // show slash path
            dragging = true;
            if (mouseDrag_time < mouseDrag_maxTime)
            {
                mouseDrag_time += Time.deltaTime;
            }
            ReleaseSilhouette();
        }
    }
    private void OnMouseDown()
    {
        mouseDown_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // convert and store mouse initial position
        mouseDown_pos.z = 0; // zero it out
        aimArea.transform.position = Input.mousePosition; // set aim area ui to mouse initial position
        aimArea.SetActive(true);
    }
    // when mouse released and attempted to slashed
    private void OnMouseUp()
    {
        imageSlashPath.GetComponent<SpriteRenderer>().enabled = false; // when no mouse input, hide slash path
        aimArea.SetActive(false);
        if ((mouseCurrent_pos - mouseDown_pos).magnitude > 0.1f && // if meaningul input detected before lifting the mouse
            SlashPathCollisionScript.me.validity &&
            !PlayerScript.me.hitStunned) // and if slash path crosses something slashable
        {
            PlayerScript.me.slashIntiated = true;
            PlayerScript.me.slashTargetPos = PlayerScript.me.endOfPath.transform.position;
        }
        mouseDrag_time = 0;
        dragging = false;
        B4ReturnSils(); 
    }
    #region SILHOUETTE
    private void ReleaseSilhouette()
    {
        if(GameManager.me.gameMode== GameManager.GameMode.slingshot)
        {
            for (int i = 0; i < sil_amount; i++)
            {
                SilhouetteScript ss = sils[i].GetComponent<SilhouetteScript>();
                ss.targetDis = mouseDrag_time * ss.maxDis / mouseDrag_maxTime;
                Vector3 targetPos = PlayerScript.me.transform.position + mouseDir * ss.targetDis;
                sils[i].transform.position = Vector3.MoveTowards(sils[i].transform.position, targetPos, Time.deltaTime * float.MaxValue);
            }
        }
    }
    private void CollectSilhouette()
    {
        if (GameManager.me.gameMode == GameManager.GameMode.slingshot)
        {
            for (int i = 0; i < sil_amount; i++)
            {
                SilhouetteScript ss = sils[i].GetComponent<SilhouetteScript>();
                ss.t += Time.deltaTime / time2ReachPlayer;
                sils[i].transform.position = Vector3.Lerp(ss.posB4Return, PlayerScript.me.transform.position, ss.t);
                ss.imageSil.transform.rotation = PlayerScript.me.imagePlayer.transform.rotation;
            }
        }
    }
    private void B4ReturnSils()
    {
        if (GameManager.me.gameMode == GameManager.GameMode.slingshot)
        {
            foreach (var sil in sils)
            {
                SilhouetteScript ss = sil.GetComponent<SilhouetteScript>();
                ss.posB4Return = sil.transform.position;  // record pos before return
                ss.t = 0; // reset t
            }
        }
    }
    public void MakeSils()
    {
        if (GameManager.me.gameMode == GameManager.GameMode.slingshot)
        {
            for (int i = 0; i < sil_amount; i++)
            {
                GameObject newSil = Instantiate(sil_prefab);
                newSil.transform.position = PlayerScript.me.transform.position;
                newSil.GetComponent<SilhouetteScript>().maxDis = (i + 1) * sil_baseDis * Mathf.Pow(sil_disMultiplyer, i);
                sils.Add(newSil);
            }
        }
    }
    public void DestroySils()
    {
        foreach (var sil in sils)
        {
            Destroy(sil);
        }
        sils.Clear();
    }
    #endregion
}
