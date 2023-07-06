using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody MyRigidbody;
    [Header("Movement")]
    [SerializeField] float speed = 30f;
    [SerializeField] float jumpVelocity = 100f;
    [SerializeField] private float transitionSharpness;
    Vector3 moveVector;
    private float targetSpeed;
    private Quaternion targetRotation;
    private CameraController cameraController;

    private Vector3 currVelocity;
    private float currSpeed;
    private Quaternion currRotation;

    void Start()
    {
        if (MyRigidbody == null)
        {
            MyRigidbody = GetComponent<Rigidbody>();
        }
        cameraController = GetComponent<CameraController>();
    }
    void FixedUpdate()
    {
        CalculateMoveDirection();

        targetSpeed = moveVector != Vector3.zero ? speed : 0;
        currSpeed = Mathf.Lerp(currSpeed, targetSpeed, Time.deltaTime * transitionSharpness);

        currVelocity = moveVector * currSpeed;

        if (targetSpeed != 0)
        {
            targetRotation = Quaternion.LookRotation(moveVector);
            currRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * transitionSharpness);
            transform.rotation = currRotation;
        }

        transform.Translate(currVelocity * Time.deltaTime, Space.World);
    }
    private void CalculateMoveDirection()
    {
        moveVector = new Vector3(PlayerInputHandler.MoveInput.x, 0, PlayerInputHandler.MoveInput.y).normalized;
        Vector3 cameraDirection = cameraController.planarDir;
        Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraDirection);

        moveVector = cameraPlanarRotation * moveVector;
    }

}
