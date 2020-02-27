using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemResetOnEnable : MonoBehaviour
{
    private ParticleSystem ps;
    
    private void OnEnable()
    {
        ps.Stop(true);
        ps.Play(true);
    }
}
