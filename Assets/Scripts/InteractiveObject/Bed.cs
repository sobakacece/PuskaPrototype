using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : InteractiveObject
{
    [SerializeField] private float shoveVelocity =  50f;
    public override void ApplyEffect()
    {
        player.GetComponent<Rigidbody>().AddForce(Vector3.up * shoveVelocity, ForceMode.Impulse);
    }
}
