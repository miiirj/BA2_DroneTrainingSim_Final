using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DroneUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text speedInput;
    float speed;

    [SerializeField]
    TMP_Text heightInput;
    float height;

    DroneController droneController;

    // Start is called before the first frame update
    void Start()
    {
        droneController = GameObject.Find("Drone").GetComponent<DroneController>();
    }

    // Update is called once per frame
    void Update()
    {
        height = Mathf.Round(droneController.gameObject.transform.position.y - droneController.startHeight);
        speed = droneController.speed;
        updateUI();
    }

    void updateUI() {
        speedInput.SetText(speed + " m/s");
        heightInput.SetText(height + " m");
    }
}
