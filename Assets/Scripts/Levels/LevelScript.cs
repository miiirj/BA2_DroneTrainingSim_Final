using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelScript : MonoBehaviour
{
    [HideInInspector] public GameObject[] flypoints;

    [SerializeField] TMP_Text scoreText;
    int score = 0;
    [HideInInspector] public int checkpointsPassed = 0;

    [HideInInspector] public bool hasTimer = false;
    [HideInInspector] public int timeNeeded = 0;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        flypoints = GameObject.FindGameObjectsWithTag("Flypoint");
        
        // if not gamified --> disable flypoints and other gamification elements
        if (PlayerPrefs.GetInt("gamifiedMode") == 0) {
            foreach (GameObject g in flypoints) {
                g.SetActive(false);
                // todo: disable counter/countdown
            }

            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Gamification UI Element")) {
                g.SetActive(false);
            }
        }

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
        if (scoreText != null) updateScoreText();

    }

    public void addScore(int nr) {
        score += nr;
        print("added point");
    }


    void updateScoreText() {
        scoreText.SetText(score + "/" + flypoints.Length);
    }

    public virtual void passedCheckpoint() {
        checkpointsPassed++;
        print(checkpointsPassed + " Checkpoint(s) passed");
    }


    public virtual (int, int) getPoints() {
        return (score, flypoints.Length);
    }
}
