using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class DayNightCycle : MonoBehaviour
{
    public Volume ppv;
    public TMP_Text time;
    public TMP_Text day;
    public DayNightController controller;

    public static float tick = 50.0f;
    public float seconds;
    public int mins;
    public int hours = 12;
    public int days = 1;

    public bool activateLights; //checks if lights are on
    GameObject[] lights;
    // Start is called before the first frame update
    void Start()
    {
        lights = GameObject.FindGameObjectsWithTag("Light");
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(false);
        }
        activateLights = false;

        ppv = gameObject.GetComponent<Volume>();
    }

    public static void SetTick(float tickVal)
    {
        tick = tickVal;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CalcTime();
    }

    public void CalcTime()
    {
        seconds += Time.fixedDeltaTime * tick;

        if(seconds >= 60)
        {
            seconds = 0;
            mins += 1;
            controller.UpdateLight(ppv.weight = Mathf.MoveTowards(ppv.weight, ppv.weight + 0.0006944444f, 1f));
            if(ppv.weight >= 1.0f)
            {
                ppv.weight = 0;
            }
        }

        if(mins >= 60)
        {
            mins = 0;
            hours += 1;
        }

        if(hours >= 24)
        {
            hours = 0;
            days += 1;
        }

        time.SetText($"{hours} : {mins}");
        day.SetText($"Day {days}");
        ControlPPV();
    }

    public void ControlPPV()
    {
        if(hours >=19 && hours < 24) //dusk at 9pm - 10pm
        {
            if(activateLights == false)
            {
                if(mins > 45)
                { 
                    for (int i = 0; i < lights.Length; i++)
                    {
                        lights[i].SetActive(true);
                    }
                    activateLights = true;
                }
            }
        }

        if (hours >= 6 && hours < 8) //dawn at 6am - 7am
        {
            if (activateLights == true)
            {
                if (mins > 45)
                {
                    for (int i = 0; i < lights.Length; i++)
                    {
                        lights[i].SetActive(false);
                    }
                    activateLights = false;
                }
            }
        }
    }

}
