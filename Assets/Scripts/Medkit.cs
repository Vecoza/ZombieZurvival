using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    public int HPAmmount = 0;
    public MedKitType medkitType;
    public enum MedKitType
    {
        SmallMedKit,
        BigMedKit
    }

    private void Start()
    {
        if (this.medkitType == MedKitType.SmallMedKit)
        {
            HPAmmount = 20;
        }
        else
        {
            HPAmmount = 50;
        }
    }
}