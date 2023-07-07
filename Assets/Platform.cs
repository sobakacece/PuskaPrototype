using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Platform : InteractiveObject
{
    float distance;
    [SerializeField] public PathCreator pathCreator;
    private GameObject  platform;
    public float speed = 5f;
    bool isActivated = false;
    public override void ApplyEffect()
    {
        isActivated = true;
    }
    void Start()
    {
    }
    void Update()
    {
        if (isActivated)
        {
            distance += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distance, EndOfPathInstruction.Reverse);
            Debug.Log($"Activated and at a distance {distance}");
        }
    }
}
