using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    protected GameObject player;
    protected void OnCollisionEnter(Collision collision)
    {
        Vector3 hit = collision.contacts[0].normal;
        float angle = Vector3.Angle(hit, Vector3.up);
        if (collision.gameObject.tag == "Player" && Mathf.Approximately(angle, 180))
        {
            player = collision.gameObject;
            ApplyEffect();
        }
    }
    public abstract void ApplyEffect();
}
