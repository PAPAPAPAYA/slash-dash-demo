using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        play,
        over
    }
    public enum GameMode
    {
        slingshot,
        forward
    }
    [HideInInspector]
    public int playerHp;
    private int ogPlayerHp;
    [Header("SCOREs")]
    public int score;
    public float score_spawnForce;
    public GameObject scorePrefab;
    public GameObject scoreDisplay;
    [Header("MANAGERs")]
    public GameObject enemySpawner_prefab;
    private GameObject enemySpawner;
    public GameObject camHolder;
    [Header("GAME STATEs")]
    public GameState gameState;
    public GameObject gameOverThingys;
    public GameObject restartButton;
    [Header("TESTs")]
    public GameMode gameMode;
    public bool charge;
    public bool enemyBoost;
    public bool spawnEnemies;
    #region SINGLETON
    public static GameManager me;
    private void Awake()
    {
        me = this;
    }
    #endregion
    private void Start()
    {
        playerHp = PlayerHpDisplayScript.me.player_HpIndicators.Count;
        ogPlayerHp = playerHp;
        gameState = GameState.play;
        SpawnEnemySpawner();
    }
    private void Update()
    {
        scoreDisplay.GetComponent<TextMeshPro>().text = score+""; // disply scores
        if (playerHp <= 0) // change the game state to over when player has no hp
        {
            gameState = GameState.over;
        }
        GameStateOperator();
        GameModeChanger();
        FeatureToggler();
    }
    public void RestartGame() // called in enemy script, used by restart button
    {
        // create enemy spawner
        SpawnEnemySpawner();
        // reset player health
        playerHp = PlayerHpDisplayScript.me.player_HpIndicators.Count;
        PlayerHpDisplayScript.me.ShowPlayerHp();
        // hide game over thingys
        gameOverThingys.SetActive(false);
        // reset score
        score = 0;
    }
    private void SpawnEnemySpawner() // called when RestartGame(), spawn an enemy spawner
    {
        if (enemySpawner == null &&
            spawnEnemies)
        {
            enemySpawner = Instantiate(enemySpawner_prefab);
            enemySpawner.transform.SetParent(camHolder.transform);
        }
    }
    private void GameModeChanger() // change modes(features that can't exist at the same time)
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gameMode = GameMode.slingshot;
            InteractionScript.me.MakeSils();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))//add a switch for phone
        {
            gameMode = GameMode.forward;
            InteractionScript.me.DestroySils();
        }

        if (Input.touchCount ==1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (Input.GetTouch(0).tapCount ==2)
            {
                gameMode = GameMode.forward;
                InteractionScript.me.DestroySils();
            }
        }
    } 
    private void FeatureToggler() // toggle features
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            charge = !charge;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            enemyBoost = !enemyBoost;
        }
    }
    private void GameStateOperator() // called every frame, do things based on current game state
    {
        switch (gameState)
        {
            case GameState.play:
                break;
            case GameState.over:
                if (!gameOverThingys.activeSelf)
                {
                    gameOverThingys.SetActive(true);
                }
                if (enemySpawner != null)
                {
                    Destroy(enemySpawner);
                }
                PlayerScript.me.hitStunned = false;
                break;
            default:
                break;
        }
    } 
}