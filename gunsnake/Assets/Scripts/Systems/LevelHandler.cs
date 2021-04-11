﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{
    public GameObject playerPrefab;

    public static string currentArea; // "Jungle", "Dungeon", "Temple"
    public static int currentFloor; // 0, 1
    public const int numFloors = 2;

    public static string jungleSceneName = "Jungle";
    public static string dungeonSceneName = "Dungeon";
    public static string templeSceneName = "Dungeon";

    // Set by DungeonRoomPlacer
    public static GameObject startObject;
    public static Direction startDirection;
    public static GameObject endObject;

    [Header("References")]
    public DungeonGenerator dungeonGen;
    public Fog fog;


    private static LevelHandler instance;
    private static GameObject playerObj;

    private bool didInit = false;
    private static bool shouldResetPlayer = true;

    [Header("Fade")]
    public Fade f;
    public GameObject darkness;
    public TextMeshProUGUI levelNameText;
    public float levelTime = .04f;
    public void Awake()
    {
        if (currentArea == null)
        {
            SetToJungle();
        }

        if (!didInit)
        {
            Initialize();
        }

        if (shouldResetPlayer)
        {
            shouldResetPlayer = false;
            Player.ResetSnakeToDefault();
        }
    }

    void Start()
    {
        StartLevel();
    }

    public static LevelHandler GetInstance()
    {
        return instance;
    }

    public void Initialize()
    {
        if (didInit)
            return;

        didInit = true;

        instance = this;

        // spawn player
        playerObj = GameObject.Find(instance.playerPrefab.name);
        if (playerObj == null)
            playerObj = Instantiate(instance.playerPrefab);
    }

    public void StartLevel()
    {
        // audio
        if (Random.Range(0, 2) == 0)
        {
            AudioManager.Play("Jungle Music");
        }
        else
        {
            AudioManager.Play("Temple Music");
        }
        //if (currentArea == "Jungle")
        //{
        //    AudioManager.Play("Jungle Music");
        //}
        //else if (currentArea == "Dungeon")
        //{
        //    AudioManager.Play("Dungeon Music");
        //}
        //else if (currentArea == "Temple")
        //{
        //    AudioManager.Play("Temple Music");
        //

        // dungeon stuff
        dungeonGen.CreateDungeon();
        fog?.Init();
        EnemyManager.InitializeEnemyDrops();

        //fade shows the title, pauses time as well
        StartCoroutine(ShowingLevelTitle());

        //player.transform.position;
        Player.playerMovement.SetSnakeSpawn(startObject.transform.position, startDirection);
        Player.playerEffects.SetPlayerEntering();
    }

    public void EndLevel()
    {
        Debug.Log("Level handler called to end level!");
        Player.playerEffects.SetPlayerExiting();
        // add a between level UI for short cut quests, etc
        
        // temp
        StartNextLevel();
    }

    public static void StartNextLevel()
    {
        currentFloor += 1;
        if (currentFloor < numFloors)
        {
            LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            currentFloor = 0;
            if (currentArea == "Jungle")
            {
                currentArea = "Dungeon";
                LoadScene(dungeonSceneName);
            }
            else if (currentArea == "Dungeon")
            {
                currentArea = "Temple";
                LoadScene(templeSceneName);
            }
            else
            {
                WinGame();
            }
        }
    }

    public static void SetToJungle()
    {
        currentArea = "Jungle";
        currentFloor = 0;
    }

    public static void RestartGame()
    {
        SetToJungle();
        shouldResetPlayer = true;
        LoadScene(jungleSceneName);
    }

    private static void LoadScene(string sceneName)
    {
        Debug.Log("Now loading " + currentArea + "-" + (currentFloor + 1));
        TimeTickSystem.ClearDelegates();
        ProjectileManager.ResetAllProjectiles();
        EnemyManager.ResetAllEnemies();
        SceneManager.LoadScene(sceneName);
    }

    private static void WinGame()
    {
        Player.EndGame(true);
    }

    //fade stuff
    public IEnumerator ShowingLevelTitle()
    {
        darkness.SetActive(true);
        Debug.Log("testing tp see if this is running");
        levelNameText.text = currentArea + "-" + (currentFloor + 1);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(levelTime);
        Time.timeScale = 1;
        StartCoroutine(HidingTitle());
    }
    public IEnumerator HidingTitle()
    {
        f.FadeOut();
        yield return new WaitForSeconds(f.Duration);
        darkness.SetActive(false);
    }
}
