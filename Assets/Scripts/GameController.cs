using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Lean.Gui;

public class GameController : MonoBehaviour
{

    // UI elements
    public Text scoreLabel;
    public Text timeLabel;
    public Text promptLabel;
    public Text resultLabel;
    public LeanButton menuButton;
    public LeanButton replayButton;
    public GameObject panel;

    private CarController carController;

    // Game logic
    public int parkTimes = 0;
    public int totalParkTimes;

    public GameState state = GameState.BeforeStart;
    public enum GameState
    {
        BeforeStart,
        Started,
        CarStopped,
        Won, 
        Lost
    }

    public void StartGame()
    {
        state = GameState.Started;
        promptLabel.text = "Press R to Respawn";
        promptLabel.gameObject.SetActive(false);
    }
    
    public void carStopped()
    {
        state = GameState.CarStopped;
        promptLabel.text = "Press R to Respawn";
        promptLabel.gameObject.SetActive(true);
    }

    public void GameWon()
    {
        state = GameState.Won;
        promptLabel.gameObject.SetActive(false);
        resultLabel.text = "You Won!";
        resultLabel.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
        replayButton.gameObject.SetActive(true);
        panel.SetActive(true);

    }
    
    void Start()
    {
        carController = GameObject.Find("TankFree_Red").GetComponent<CarController>();

        Debug.Log("GameController Start");

        menuButton.OnClick.AddListener(() => {
            Debug.Log("MenuButton Clicked");
            SceneManager.LoadScene("MenuScene");
        });
        replayButton.OnClick.AddListener(() => {
            Debug.Log("ReplayButton Clicked");
            SceneManager.LoadScene("Level1");
        });

        
        resultLabel.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);
        panel.SetActive(false);

        scoreLabel.text = "TODO: 0/n Parked";
        timeLabel.text = "TODO: 0:00";
        
        promptLabel.text = "Press W to Start";
        promptLabel.gameObject.SetActive(true);
    }

    void Update()
    {
        if (state == GameState.Started || state == GameState.CarStopped)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                carController.ResetPosition();
                promptLabel.gameObject.SetActive(false);
                state = GameState.BeforeStart;
            }
        } else if (state == GameState.Lost)
        {
            resultLabel.text = "You Lost!";
            resultLabel.gameObject.SetActive(true);
            menuButton.gameObject.SetActive(true);
            replayButton.gameObject.SetActive(true);
            panel.SetActive(true);
        }
    }
}
