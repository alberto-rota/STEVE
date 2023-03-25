using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag : MonoBehaviour
{

    public GameObject instr1;
    public GameObject instr2;

    public GameObject TagPrefab;

    Vector3 p1; // Position of gripper 1
    public float jaw_angle;
    public bool clicked; 
    public bool listening; 
    float lastT;
    private int flag;
    bool delay = false;

    public int num_clicks=0;
    public int clicksTrigger=3;


    void RenderLoadingBar(bool flag) {
        if (flag && num_clicks == clicksTrigger -1) {
            GameObject.Find("/Text/CanvasBar/Bar").transform.localScale = new Vector3(20*num_clicks/clicksTrigger,
                GameObject.Find("/Text/CanvasBar/Bar").transform.localScale.y, GameObject.Find("/Text/CanvasBar/Bar").transform.localScale.z);
            GameObject.Find("/Text/CanvasBar/Bar").GetComponent<UnityEngine.UI.Image>().color = Color.cyan;
        } else if (flag && num_clicks < clicksTrigger -1) {
            GameObject.Find("/Text/CanvasBar/Bar").transform.localScale = new Vector3(0,
            GameObject.Find("/Text/CanvasBar/Bar").transform.localScale.y, GameObject.Find("/Text/CanvasBar/Bar").transform.localScale.z);
        }
    }

    // void RenderLine(bool flag) {
    //     if (flag) {
    //         Global.Arrow(p1, p2, Color.cyan);
    //         Global.Arrow(p2, p1, Color.cyan);
    //     }
    // }

    void Start()
    {
        lastT = Time.realtimeSinceStartup;
        clicked = false;
        listening = false;
    }

    void Update()
    {
        
        jaw_angle = GameObject.FindWithTag("ROBOT").GetComponent<RosSharp.RosBridgeClient.JointJawSubscriber>().jawPosition;

        if (!delay ) {
            if (jaw_angle < -2.6f) {
                clicked = true;
                num_clicks++;
                lastT = Time.realtimeSinceStartup;
                delay = true;
            } else clicked = false;
        }

        if ((Time.realtimeSinceStartup - lastT) > 0.3f ) {
            delay = false;
        } 
        if ((Time.realtimeSinceStartup - lastT) > 1f ) {
            delay = false;
            num_clicks = 0;
        }

        if (num_clicks >= clicksTrigger) {
            num_clicks = 0;
            Instantiate(TagPrefab, instr1.transform.position, Quaternion.identity);
        }   

        // if (num_clicks >= 2)
        RenderLoadingBar(true);
        // else RenderLoadingBar(false);
    }
}
