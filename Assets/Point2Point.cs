using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point2Point : MonoBehaviour
{

    public GameObject instr1;
    public GameObject instr2;

    Vector3 p1; // Position of gripper 1
    Vector3 p2; // Position of gripper 2
    public float distance; // Distance between grippers
    public bool touching=false; // Whether grippers are touching
    public float distanceThreshold=0.01f; // Threshold distance for considering gripper as touching
    public float timer=0f; // Timer for how long gripper has been touching
    public float timeThreshold=3f; // Threshold time for activating GUI
    public bool guiActive=false; // Whether GUI is active


    void RenderLoadingBar(bool flag_touching, bool flag_active) {
        if (flag_touching) {
            if (timer < timeThreshold) {
                GameObject.Find("/Text/CanvasBar/Bar").transform.localScale = new Vector3(20*timer/timeThreshold,
                    GameObject.Find("/Text/CanvasBar/Bar").transform.localScale.y, GameObject.Find("/Text/CanvasBar/Bar").transform.localScale.z);
                GameObject.Find("/Text/CanvasBar/Bar").GetComponent<UnityEngine.UI.Image>().color = Color.Lerp(Color.green, Color.red, timer/timeThreshold);
            } else {
                GameObject.Find("/Text/CanvasBar/Bar").transform.localScale = new Vector3(20,
                    GameObject.Find("/Text/CanvasBar/Bar").transform.localScale.y, GameObject.Find("/Text/CanvasBar/Bar").transform.localScale.z);
                GameObject.Find("/Text/CanvasBar/Bar").GetComponent<UnityEngine.UI.Image>().color = Color.cyan;
            }
        }
        if (flag_active) {
            GameObject.Find("/Text/CanvasBar/Bar").transform.localScale = new Vector3(20,
            GameObject.Find("/Text/CanvasBar/Bar").transform.localScale.y, GameObject.Find("/Text/CanvasBar/Bar").transform.localScale.z);
            GameObject.Find("/Text/CanvasBar/Bar").GetComponent<UnityEngine.UI.Image>().color = Color.cyan;
            GameObject.Find("/Text/CanvasBar/DistanceText").GetComponent<UnityEngine.UI.Text>().text=(distance*100).ToString("F2")+" cm";
        }
        else {
            GameObject.Find("/Text/CanvasBar/DistanceText").GetComponent<UnityEngine.UI.Text>().text="";
        }
    }

    void RenderLine(bool flag) {
        if (flag) {
            Global.Arrow(p1, p2, Color.cyan);
            Global.Arrow(p2, p1, Color.cyan);
        }
    }

    void Start()
    {

    }

    void Update()
    {
        p1 = instr1.transform.position;
        p2 = instr2.transform.position;
        distance = Vector3.Distance(p1, p2);
        if (distance < distanceThreshold)
        {
            touching = true;
            timer += Time.deltaTime;

        }
        else
        {
            touching = false;
            timer = 0;
        }
        if (timer > timeThreshold)
        {
            guiActive = true;
        }

        RenderLoadingBar(touching, guiActive);
        RenderLine(guiActive);
    }
}
