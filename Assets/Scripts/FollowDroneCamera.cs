using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowDroneCamera : MonoBehaviour
{
    [SerializeField] Transform drone;
    // Start is called before the first frame update
    [SerializeField] GameObject dronecam;

    bool useMainCam = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (useMainCam) {
            transform.LookAt(drone);
        }


    }

    public void toggleCamera() {
        useMainCam = !useMainCam;
        //gameObject.SetActive(useMainCam);
        dronecam.gameObject.SetActive(!useMainCam);
    }
}
