using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightController : MonoBehaviour
{
    public Transform LightHolder;

    public Light2D DayLight;
    public Gradient DayLightGradient;

    public Light2D NightLight;
    public Gradient NightLightGradient;

    // Start is called before the first frame update
    void Start()
    {
        UpdateLight(0.5f);
    }

    // Update is called once per frame
    public void UpdateLight(float ratio)
    {
        DayLight.color = DayLightGradient.Evaluate(ratio);
        NightLight.color = NightLightGradient.Evaluate(ratio);

        LightHolder.rotation = Quaternion.Euler(0, 0, 360.0f * ratio);
    }
}
