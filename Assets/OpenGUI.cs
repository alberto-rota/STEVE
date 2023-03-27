using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGUI : MonoBehaviour
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

    public Vector3 p1_screen;
    public Vector3 button_screen;
    public float dfb;

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
    }

    void ActivateGUI(bool flag) {
        // if (flag) {
            GameObject.Find("/Text/CanvasBar/GUI").SetActive(flag);
            GameObject.Find("/Text/CanvasBar/Cursor").SetActive(flag);
        // }
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
        ActivateGUI(guiActive);

        if (guiActive)
        {
            Camera cam = GameObject.Find("/Camera/CameraR").GetComponent<Camera>();
            p1_screen = cam.WorldToScreenPoint(p1);
            // button_screen = cam.WorldToScreenPoint(GameObject.Find("/Text/CanvasBar/GUI/Button_P2P").transform.position);

            // Check if button is hovered by p1_screen

            Rect button_screen = RectTransformUtility.PixelAdjustRect(GameObject.Find("/Text/CanvasBar/GUI/Button_P2P").GetComponent<RectTransform>(), GameObject.Find("/Text/CanvasBar").GetComponent<Canvas>());
            Debug.Log(button_screen.center);
            // Debug.Log(
            // dfb = Vector3.Distance(
            // );
            GameObject.Find("/Text/CanvasBar/Cursor").transform.position = p1_screen;

            dfb = Vector2.Distance(
                new Vector2(GameObject.Find("/Text/CanvasBar/Cursor").GetComponent<RectTransform>().position.x, GameObject.Find("/Text/CanvasBar/Cursor").GetComponent<RectTransform>().position.y),
                new Vector2(GameObject.Find("/Text/CanvasBar/GUI/Button_P2P").GetComponent<RectTransform>().position.x, GameObject.Find("/Text/CanvasBar/GUI/Button_P2P").GetComponent<RectTransform>().position.y)
                );
            if (dfb < 70) {
                GameObject.Find("/Text/CanvasBar/GUI/Button_P2P").GetComponent<UnityEngine.UI.Image>().color = Color.red;
            } else {
                GameObject.Find("/Text/CanvasBar/GUI/Button_P2P").GetComponent<UnityEngine.UI.Image>().color = Color.cyan;
            }
        }
    }
}
