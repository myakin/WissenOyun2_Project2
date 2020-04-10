using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Transform backgroundPanel, mainMenuPanel;
    public Button loadCheckpointButton, newGameButton, hiScoresButton, exitGameButton;
    public Text scoreText;
    


    private void Start() {
        backgroundPanel.gameObject.SetActive(false);
        mainMenuPanel.gameObject.SetActive(false);

        loadCheckpointButton.onClick.AddListener(LoadCheckpoint);
        newGameButton.onClick.AddListener(StartNewGame);
        hiScoresButton.onClick.AddListener(GetHiScores);
        exitGameButton.onClick.AddListener(QuitGame);
    }

    public void ActivteMainMenu() {
        backgroundPanel.gameObject.SetActive(true);
        mainMenuPanel.gameObject.SetActive(true);
    }

    private void StartNewGame() {
        ResetScore();
        backgroundPanel.gameObject.SetActive(false);
        mainMenuPanel.gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerControllerForManuelSetup>().InitiatePlayer();
    }

    private void LoadCheckpoint() {
         GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerControllerForManuelSetup>().ResumeLastSave();  
    }

    private void GetHiScores() {

    }

    private void QuitGame() {

    }
    
    public void UpdateScoreText(string aScoreString) {
        scoreText.text = aScoreString;
    }
    private void ResetScore() {
        scoreText.text = "0";
    }


}
