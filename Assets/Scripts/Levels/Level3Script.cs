using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level3Script : LevelScript
{
    [SerializeField] TMP_Text countDownText;
    int nrOfCheckpoints;

    bool tagChanged = false;

    protected override void Start() {
        nrOfCheckpoints = GameObject.FindGameObjectsWithTag("TempMarker").Length;
        base.Start();
        hasTimer = true;

        // start timer if gamified
        if (PlayerPrefs.GetInt("gamifiedMode") == 1) {
            countDownText.gameObject.SetActive(true);
            StartCoroutine(countTime());
        }

        flypoints = GameObject.FindGameObjectsWithTag("TempMarker"); // todo: change

    }

    IEnumerator countTime() {
        while (true) {
            yield return new WaitForSeconds(1);
            timeNeeded++;
        }
        
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();

        countDownText.text = timeNeeded + "s";

        // last checkpoint --> add final goal tag
        if (nrOfCheckpoints - 1 == checkpointsPassed && !tagChanged) {
            GameObject.FindGameObjectsWithTag("TempMarker")[0].tag = "FinalMarker";
            tagChanged = true;
            StopCoroutine(countTime());
        }


    }

    public override void passedCheckpoint() {
        base.passedCheckpoint();
        
    }

    public override (int, int) getPoints() {
        return (timeNeeded, -1);
    }



}
