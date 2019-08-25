using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentOnStart : MonoBehaviour
{
    public Transform parentTo;
    
    private void Start()
    {
        transform.parent = parentTo;
    }
}
