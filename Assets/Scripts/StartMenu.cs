using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    string levelSelect = "";

    [SerializeField] GameObject otherMenu;

    string gamifiedPlayerPref = "gamifiedMode";
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void modusAButton() {
        PlayerPrefs.SetInt(gamifiedPlayerPref, 0);
        loadLevelScene();
    }

    public void modusBButton() {
        PlayerPrefs.SetInt(gamifiedPlayerPref, 1);
        loadLevelScene();
    }

    public void controlsButton() {
        print("controls");
    }

    void loadLevelScene() {
        SceneManager.LoadScene(levelSelect);
    }

    public void resetButton() {
        otherMenu.SetActive(true);
    }

    public void resetConfirm() {
        PlayerPrefs.SetInt("level1unlocked", 0);
        PlayerPrefs.SetInt("level2unlocked", 0);
        PlayerPrefs.SetInt("level3unlocked", 0);

        PlayerPrefs.SetInt("level1highscore", 0);
        PlayerPrefs.SetInt("level2highscore", 0);
        PlayerPrefs.SetInt("level3highscore", 0);

        PlayerPrefs.SetInt("playerPoints", 0);

        otherMenu.SetActive(false);
    }

    public void resetDeny() {
        otherMenu.SetActive(false);
    }

    public void quitGame() {
        Application.Quit();
    }
}
