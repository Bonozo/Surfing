using UnityEngine;
using System.Collections;

/// <summary>
/// Test script for SpeedometerUI. Needs to be attached same object which SpeedometerUI Script attached.
/// </summary>
public class SpeedometerUITest3Way : MonoBehaviour
{
    SpeedometerUI[] SpeedometerIUs=null;

    SpeedometerUI oSpeedoForSpeed;
    SpeedometerUI oSpeedoForTurbo;
    SpeedometerUI oSpeedoForRPM;


    float maxSpeed =240;
    float maxRPM = 8000;
    float curSpeed = 0;
    float curRPM = 0;
    string curGear = "";
    bool speedUp = true;
    bool rpmUp = true;
   
    void Start()
    {
        SpeedometerIUs = base.GetComponents<SpeedometerUI>();

         foreach (SpeedometerUI oSUI in SpeedometerIUs)
         {
             if (oSUI.ControlName == "Speed")
             {
                 oSpeedoForSpeed = oSUI;
             }
             else if (oSUI.ControlName == "Turbo")
             {
                 oSpeedoForTurbo = oSUI;
             }
             else if (oSUI.ControlName == "RPM")
             {
                 oSpeedoForRPM = oSUI;
             }
         }
    }

    void Update()
    {

        if (speedUp) { curSpeed += Time.deltaTime * 40; } else { curSpeed -= Time.deltaTime * 40; }
        if (rpmUp) { curRPM += Time.deltaTime * 2000; } else { curRPM -= Time.deltaTime * 2000; }
      
        if (curSpeed >= maxSpeed) { speedUp = false; }
        if (curSpeed <= 0) { speedUp = true; }
        if (curSpeed > 0 && curSpeed < 50) curGear = "1";
        if (curSpeed > 50 && curSpeed < 100) curGear = "2";
        if (curSpeed > 100 && curSpeed < 150) curGear = "3";
        if (curSpeed > 150 && curSpeed < 250) curGear = "4";
        if (curRPM >= maxRPM) { rpmUp = false ; }
        if (curRPM <= 0) { rpmUp = true; }



        if (oSpeedoForSpeed != null)
        {
            oSpeedoForSpeed.Speed = curSpeed;
            string gear = curGear;
            if (gear == "-1") gear = "R";
            if (gear == "0") gear = "N";
            oSpeedoForSpeed.Gear = gear;
        }
        if (oSpeedoForRPM != null) oSpeedoForRPM.Speed = curRPM;
        if (oSpeedoForTurbo != null) oSpeedoForTurbo.Speed = curSpeed;
        
    }
   

}

