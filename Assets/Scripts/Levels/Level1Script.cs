using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level1Script : LevelScript
{
    [SerializeField] GameObject[] checkpoints;
    [SerializeField] TMP_Text[] checkPointsText;
    [SerializeField] GameObject finalObject;
    bool setFinalObjectActive = false;

    protected override void Start() {

        base.Start();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
        if (checkpointsPassed == checkpoints.Length && !setFinalObjectActive) {
            print("spawn final point");
            checkPointsText[checkpointsPassed].gameObject.SetActive(true);
            finalObject.SetActive(true);
            setFinalObjectActive = true;
        }
    }

    public override void passedCheckpoint() {
        base.passedCheckpoint();
        checkPointsText[checkpointsPassed - 1].fontStyle = FontStyles.Strikethrough;
    }




}
