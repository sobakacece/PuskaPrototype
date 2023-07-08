using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Platform : InteractiveObject
{
    float distance;
    [SerializeField] public PathCreator pathCreator;
    Rigidbody MyRigidbody;
    // Vector3 moveVector;
    public float speed = 5f;
    private float currSpeed;
    bool isActivated = false;
    public override void ApplyEffect()
    {
        isActivated = true;
    }
    void Start()
    {
        MyRigidbody = GetComponent<Rigidbody>();
        // pathCreator.path.OnEndOfPath += () => isActivated = false;
    }
    void FixedUpdate()
    {
        // MyRigidbody.velocity =  Vector3.zero;
        if (isActivated)
        {
            distance += speed;
            MyRigidbody.MovePosition(pathCreator.path.GetPointAtDistance(distance, EndOfPathInstruction.Reverse));
            // moveVector = pathCreator.path.GetPointAtDistance(distance, EndOfPathInstruction.Reverse).normalized;
            if (distance >= pathCreator.path.length * 2)
            {
                Refresh();
            }
        }

    }
    void Refresh()
    {
        distance = 0;
        MyRigidbody.MovePosition(pathCreator.path.GetPointAtDistance(distance));
        isActivated = false;
    }
}
