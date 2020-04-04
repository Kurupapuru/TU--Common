using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXK.HealthSystem;

public class Attacker : MonoBehaviour
{
    public HealthChangeInfo healthChangeInfo;
    public LayerMask attackMask;
    public Vector3 offset;
    public KeyCode attackKey = KeyCode.E;
    public float coolDownTime = 1f;

    private float cooldownEndTime;
    
    public void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (Time.time < cooldownEndTime)
            return;
        
        var colliders = Physics.OverlapSphere(transform.TransformPoint(offset), .5f, attackMask);
        foreach (var coll in colliders)
        {
            IHealthController hasHealth = null;
            
            if (coll.attachedRigidbody != null)
                hasHealth = coll.attachedRigidbody.GetComponent<IHealthController>();

            if (hasHealth == null)
                hasHealth = coll.GetComponent<IHealthController>();
            
            if (hasHealth != null)
                hasHealth.ApplyHealthChange(healthChangeInfo);
        }

        cooldownEndTime = Time.time + coolDownTime;
    }
}
