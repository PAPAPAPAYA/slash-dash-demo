using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public static GameManager me;
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
    [Header("TESTINGs")]
    public GameMode gameMode;
    public bool charge;
    public bool enemyBoost;
    private void Awake()
    {
        me = this;
    }
    private void Start()
    {
        playerHp = PlayerHpDisplayScript.me.player_HpIndicators.Count;
        ogPlayerHp = playerHp;
        gameState = GameState.play;
        SpawnEnemySpawner();
    }
    private void Update()
    {
        scoreDisplay.GetComponent<TextMeshPro>().text = score+"";
        if (playerHp <= 0)
        {
            gameState = GameState.over;
        }
        GameStateOperator();
        GameModeChanger();
        FeatureToggler();
    }
    public void RestartGame()
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
    private void SpawnEnemySpawner()
    {
        if (enemySpawner == null)
        {
            enemySpawner = Instantiate(enemySpawner_prefab);
            enemySpawner.transform.SetParent(camHolder.transform);
        }
    }
    private void GameModeChanger()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gameMode = GameMode.slingshot;
            InteractionScript.me.MakeSils();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gameMode = GameMode.forward;
            InteractionScript.me.DestroySils();
        }
    }
    private void FeatureToggler()
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
    private void GameStateOperator()
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