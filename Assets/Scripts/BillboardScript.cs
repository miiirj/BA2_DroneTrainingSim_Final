using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    GameObject drone;
    // Start is called before the first frame update
    void Start()
    {
        drone = GameObject.Find("Drone");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(drone.transform);
    }
}
