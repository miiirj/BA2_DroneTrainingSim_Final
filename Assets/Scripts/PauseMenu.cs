using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    string menuScene = "";

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void continueButton() {
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void settingsButton() {
        print("Settings");
    }

    public void backToMenuButton() {
        SceneManager.LoadScene(menuScene);
    }

    public void retryButton() {
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
