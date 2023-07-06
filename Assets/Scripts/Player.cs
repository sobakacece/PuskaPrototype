using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody MyRigidbody;
    [Header("Movement")]
    [SerializeField] float speed = 1f;
    [SerializeField] float jumpVelocity = 30f;
    [SerializeField] private float transitionSharpness;
    [SerializeField][Range(0, 1f)] private float velocityDeadThreshold;
    Vector3 moveVector;
    private float targetSpeed;
    private Quaternion targetRotation;
    private CameraController cameraController;

    private Vector3 currVelocity;
    private float currSpeed;
    private Quaternion currRotation;

    void OnEnable()
    {
        PlayerInputHandler.OnJump += Jump;
    }
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
        currSpeed = Mathf.Lerp(currSpeed, targetSpeed, transitionSharpness);

        currVelocity = moveVector * currSpeed;

        if (targetSpeed != 0)
        {
            targetRotation = Quaternion.LookRotation(moveVector);
            currRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * transitionSharpness);
            transform.rotation = currRotation;
        }
        MyRigidbody.velocity += currVelocity;
        // MyRigidbody.AddRelativeForce(currVelocity, ForceMode.Impulse);
        // transform.Translate(currVelocity * Time.deltaTime, Space.World);
    }
    private void ApplyThreshold()
    {
        if (MyRigidbody.velocity.magnitude < velocityDeadThreshold)
        {
            MyRigidbody.velocity = Vector3.zero;
        }
    }
    private void CalculateMoveDirection()
    {
        moveVector = new Vector3(PlayerInputHandler.MoveInput.x, 0, PlayerInputHandler.MoveInput.y).normalized;
        Vector3 cameraDirection = cameraController.planarDir;
        Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraDirection);

        moveVector = cameraPlanarRotation * moveVector;
    }
    private void OnDisable()
    {
        PlayerInputHandler.OnJump -= Jump;

    }
    private void Jump()
    {
        // Debug.Log("Jump happened");
        // MyRigidbody.AddForce(jumpVelocity * Vector3.up);
        MyRigidbody.velocity += jumpVelocity * Vector3.up;

    }

}
