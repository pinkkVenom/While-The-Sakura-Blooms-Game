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

    public float tick;
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
        if(hours >=21 && hours < 22) //dusk at 9pm - 10pm
        {
            ppv.weight = (float)mins / 60;
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

        if (hours >= 6 && hours < 7) //dawn at 6am - 7am
        {
            ppv.weight = 1- (float)mins / 60;
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
