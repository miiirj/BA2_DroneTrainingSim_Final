using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class DroneController : MonoBehaviour {
    [SerializeField] GameObject dronebody;
    private Rigidbody droneRigidbody;


    [SerializeField]
    private GameObject[] rotors;

    private float upDown = 0;
    private float turnLeftRight = 0;
    private float leftRight = 0;
    private float forwardBack = 0;

    [SerializeField] float forwardBackSpeed = 2f;
    [SerializeField] float rotateSpeed = 2f;
    [SerializeField] float rotorRotateSpeed = 2f;
    float maxRotation = 20;

    [HideInInspector] public float speed = 0;
    [HideInInspector] public float startHeight;

    bool crashed = false;
    [SerializeField] GameObject crashEffect;

    [SerializeField] LevelSetup[] levelSetup;
    [SerializeField] GameObject mainSetup;
    LevelScript currentLevelScript;
    [SerializeField] GameObject endMenu;
    [SerializeField] TMP_Text customText;
    [SerializeField] GameObject pauseMenu;

    [SerializeField] AnimationCurve controlSensitivity;

    AudioSource droneSound;

    string currentLevelName = "";

    Vector3 startPos;
    [SerializeField] int range = 100;
    [SerializeField] PostProcessVolume postProcessDrone;

    bool gamified = false;

    // Start is called before the first frame update
    void Start() {
        Time.timeScale = 1;
        droneRigidbody = GetComponent<Rigidbody>();
        droneSound = GetComponent<AudioSource>();
        crashed = false;

        gamified = PlayerPrefs.GetInt("gamifiedMode") == 1;

        // select correct things for current level
        foreach (LevelSetup l in levelSetup) {
            if (l.playerPrefName == PlayerPrefs.GetString("currentLevel")) {
                l.levelObjects.SetActive(true);
                currentLevelName = l.playerPrefName;

                mainSetup.transform.position = l.startPos;
                mainSetup.transform.Rotate(new Vector3(0, l.startYRot, 0));
                currentLevelScript = l.levelObjects.GetComponent<LevelScript>();
            } else {
                l.levelObjects.SetActive(false);
            }
        }

        startHeight = gameObject.transform.position.y;
        startPos = gameObject.transform.position;
    }

    // mostly flight behaviour
    void FixedUpdate() {
        if (!crashed && Time.timeScale > 0) {
            foreach (GameObject rotor in rotors) {
                rotor.transform.Rotate(Vector3.up, rotorRotateSpeed); // new[] {upDown, leftRight, turnLeftRight}.Max()
            }

            speed = Mathf.Round(droneRigidbody.velocity.magnitude);

            // TODO: Figure out if addforce or moveposition or something else?
            // / 3.6f  from m/s to km/h https://answers.unity.com/questions/160813/move-objects-in-kmh.html
            // *2 because otherwise km/h to m/s values with velocity.magnitude don't fit
            droneRigidbody.AddRelativeForce(0, 0, forwardBack * forwardBackSpeed / 3.6f * 2 * Time.fixedDeltaTime, ForceMode.VelocityChange);
            droneRigidbody.AddRelativeForce(0, upDown * forwardBackSpeed / 3.6f * 2 * Time.fixedDeltaTime, 0, ForceMode.VelocityChange);
            droneRigidbody.AddRelativeForce(leftRight * forwardBackSpeed / 3.6f * 2 * Time.fixedDeltaTime, 0, 0, ForceMode.VelocityChange);

            transform.Rotate(Vector3.up, rotateSpeed * turnLeftRight);
            // - needed for leftRight because otherwise rotation in wrong direction
            dronebody.transform.localRotation = Quaternion.Euler(forwardBack * maxRotation, 0, -leftRight * maxRotation);


            if (!droneSound.isPlaying) {
                droneSound.Play();
            }

            if (Vector3.Distance(startPos, transform.position) > range) {
                print("over distance");
                float distanceOverMax = Vector3.Distance(startPos, transform.position) - range;
                postProcessDrone.gameObject.SetActive(true);
            } else {
                if (postProcessDrone.gameObject.activeInHierarchy) {
                    postProcessDrone.gameObject.SetActive(false);
                }
            }
        }

    }

    #region Input
    public void upDownInput(InputAction.CallbackContext context) {
        upDown = context.ReadValue<float>();

    }

    public void turnLeftRightInput(InputAction.CallbackContext context) {
        turnLeftRight = Mathf.Sign(context.ReadValue<float>()) * controlSensitivity.Evaluate(Mathf.Abs(context.ReadValue<float>()));

    }

    public void leftRightInput(InputAction.CallbackContext context) {
        leftRight = Mathf.Sign(context.ReadValue<float>()) * controlSensitivity.Evaluate(Mathf.Abs(context.ReadValue<float>()));
    }

    public void forwardBackInput(InputAction.CallbackContext context) {
        forwardBack = Mathf.Sign(context.ReadValue<float>()) * controlSensitivity.Evaluate(Mathf.Abs(context.ReadValue<float>()));

    }

    public void escapeInput(InputAction.CallbackContext context) {
        if (!crashed) {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
            if (Time.timeScale == 0) droneSound.Stop();
        }
    }
    #endregion

    private void OnCollisionEnter(Collision collision) {
        if (speed > 5) {
            crashed = true;
            crashEffect.SetActive(true);
            gameOverScreen("Unfall");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Flypoint")) {
            currentLevelScript.addScore(1);
            Destroy(other.gameObject);

        }

        if (other.CompareTag("Deadzone")) {
            crashed = true;
            gameOverScreen("Sichere Flugzone verlassen");
            
        }

        if (other.CompareTag("FinalMarker")) {
            (int, int) points = currentLevelScript.getPoints();

            if (gamified) {
                string highscoreMessage = points.Item2 == -1 ? points.Item1 + " s" : points.Item1 + "/" + points.Item2;
                int pointsAdded = points.Item2 == -1 ? Mathf.Clamp((50 - points.Item1), 0, 50) : points.Item1;
                print(Mathf.Clamp((50 - points.Item1), 0, 50));
                gameOverScreen("Level abgeschlossen\n" + highscoreMessage, true);
                PlayerPrefs.SetInt(currentLevelName + "highscore", points.Item1);
                PlayerPrefs.SetInt(currentLevelName + "unlocked", 1);
                int totalScore = PlayerPrefs.GetInt("playerPoints");
                PlayerPrefs.SetInt("playerPoints", totalScore + pointsAdded);

            } else {
                gameOverScreen("Level abgeschlossen", true);
            }

        }

    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("TempMarker")) {
            currentLevelScript.passedCheckpoint();
            Destroy(other.gameObject);           
        }
    }

    public void gameOverScreen(string message, bool completed = false) {
        Time.timeScale = 0;
        endMenu.SetActive(true);
        customText.SetText(message);
        droneSound.Stop();
    }


}

[System.Serializable]
public class LevelSetup {
    public GameObject levelObjects;
    public Vector3 startPos;
    public string playerPrefName;
    public float startYRot;
}
