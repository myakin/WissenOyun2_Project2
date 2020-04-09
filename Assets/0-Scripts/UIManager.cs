using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Transform backgroundPanel, mainMenuPanel;
    public Button newGameButton, hiScoresButton, exitGameButton;

    private void Start() {
        backgroundPanel.gameObject.SetActive(false);
        mainMenuPanel.gameObject.SetActive(false);
        newGameButton.onClick.AddListener(StartNewGame);
        hiScoresButton.onClick.AddListener(GetHiScores);
        exitGameButton.onClick.AddListener(QuitGame);
    }

    public void ActivteMainMenu() {
        backgroundPanel.gameObject.SetActive(true);
        mainMenuPanel.gameObject.SetActive(true);
    }

    private void StartNewGame() {
        backgroundPanel.gameObject.SetActive(false);
        mainMenuPanel.gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerControllerForManuelSetup>().InitiatePlayer();
    }

    private void GetHiScores() {

    }

    private void QuitGame() {

    }



}
