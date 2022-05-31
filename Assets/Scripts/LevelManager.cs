using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    string gamifiedPlayerPref = "gamifiedMode";
    [SerializeField] string modeSelect;

    [SerializeField] LevelInput[] levels;

    [HideInInspector] bool gamified;

    GameObject[] gamificationElements;

    [SerializeField] LevelImages[] images;
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text sliderText;

    // Start is called before the first frame update
    void Start()
    {
        gamified = PlayerPrefs.GetInt(gamifiedPlayerPref) == 1;


        int points = PlayerPrefs.GetInt("playerPoints");
        sliderText.text = Mathf.Round(points / 10) + "";
        slider.value = (((float) points % 10) / 10);


        if (!gamified) {
            gamificationElements = GameObject.FindGameObjectsWithTag("Gamification UI Element");

            foreach (GameObject g in gamificationElements) {
                g.SetActive(false);
            }
        }

        if (gamified) {
            // see if level was unlocked
            // display correct medals

            string previousPlayerprefname = "";
            foreach (LevelInput li in levels) {
                // if previous level finished then unlock new one, else keep locked;
                
                bool playerPrefValue = (previousPlayerprefname != "") ? PlayerPrefs.GetInt(previousPlayerprefname + "unlocked") == 1 : true;

                if (PlayerPrefs.GetInt(li.playerPrefName + "highscore") > 0) {
                    int highscore = PlayerPrefs.GetInt(li.playerPrefName + "highscore");
                    li.parentObject.transform.Find("Highscore").gameObject.SetActive(true);
                    li.parentObject.transform.Find("Highscore").gameObject.GetComponent<TMP_Text>().text = (li.playerPrefName == "level3" ? highscore + "s" : highscore + "P");
                } else {
                    li.parentObject.transform.Find("Highscore").gameObject.SetActive(false);
                }

                li.parentObject.transform.Find("Lock").gameObject.SetActive(!playerPrefValue);

                Button b = li.parentObject.transform.Find("Button").gameObject.GetComponent<Button>();
                b.interactable = playerPrefValue;
                
                previousPlayerprefname = li.playerPrefName;
            }
        }

        foreach (LevelImages li in images) {
            li.uiElement.sprite = (gamified ? li.gamified : li.noGamified);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buttonClicked() {
        string name = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.name;

        // load correct scene
        foreach (LevelInput li in levels) {
            if (li.parentObject.name == name) {
                PlayerPrefs.SetString("currentLevel", li.playerPrefName);
                SceneManager.LoadScene(li.scenename);
            }
        }
    }

    public void returnToModeSelect() {
        SceneManager.LoadScene(modeSelect);;
    }
}

[System.Serializable]
public class LevelInput {
    public string scenename;
    public string playerPrefName;
    public GameObject parentObject;
}

[System.Serializable]
public class LevelImages {
    public Image uiElement;
    public Sprite noGamified;
    public Sprite gamified;
}