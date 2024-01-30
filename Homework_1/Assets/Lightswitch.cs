using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Lightswitch : MonoBehaviour
{
    public InputActionReference lightswitchAction;

    public Light sunLamp;
    public Light candle;
    public ReflectionProbe reflectionProbe;

    public Material houseMaterial;
    public Material propsMaterial;
    public Material[] backgroundTreeMaterials;
    public Material[] foregroundTreeMaterials;
    public Material groundMaterial;

    public Material daySkybox;
    public Material nightSkybox;

    public Texture houseTextureDay;
    public Texture houseTextureNight;
    public Texture propsTextureDay;
    public Texture propsTextureNight;

    public Color darknessEmissive;
    public Color darknessDiffuse;
    public Color greenHue;
    public Color candleFlame;

    private bool isDay = true;


    // Start is called before the first frame update
    void Start()
    {
        switchToDay();
        lightswitchAction.action.Enable();
        lightswitchAction.action.performed += lightswitchActionHandler;
    }

    private void lightswitchActionHandler(InputAction.CallbackContext context)
    {
        toggleDay();
    }

    public void toggleDay()
    {
        if (isDay)
            switchToNight();
        else
            switchToDay();
    }
    private void switchToNight()
    {
        isDay = false;

        houseMaterial.SetTexture("_EmissionMap", houseTextureNight);
        propsMaterial.SetTexture("_EmissionMap", propsTextureNight);

        sunLamp.intensity = 0;
        RenderSettings.skybox = nightSkybox;

        foreach (Material material in backgroundTreeMaterials)
        {
            material.SetColor("_EmissionColor", darknessEmissive);
        }
        foreach (Material material in foregroundTreeMaterials)
        {
            material.SetColor("_BaseColor", darknessDiffuse);
        }
        groundMaterial.SetColor("_BaseColor", darknessDiffuse);
        
        //The actual point light of the candle is unfortunately not visible in-game.
        //This is just here to meet exercise criteria:
        candle.color = candleFlame;

        reflectionProbe.RenderProbe();
    }
    private void switchToDay()
    {
        isDay = true;

        houseMaterial.SetTexture("_EmissionMap", houseTextureDay);
        propsMaterial.SetTexture("_EmissionMap", propsTextureDay);

        sunLamp.intensity = 5;
        RenderSettings.skybox = daySkybox;

        foreach (Material material in backgroundTreeMaterials)
        {
            material.SetColor("_EmissionColor", Color.white);
        }
        foreach (Material material in foregroundTreeMaterials)
        {
            material.SetColor("_BaseColor", Color.white);
        }
        groundMaterial.SetColor("_BaseColor", greenHue);
        candle.color = Color.black;

        reflectionProbe.RenderProbe();
    }
}
