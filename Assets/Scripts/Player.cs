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

    private bool IsHanging;
    float distanceToGround;

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
        distanceToGround = GetComponent<Collider>().bounds.extents.y;
    }
    void FixedUpdate()
    {
        if (IsHanging)
            return;

        if (MyRigidbody.velocity.y < 0)
            LedgeGrab();
        
        Movement();
        ApplyThreshold();
        MyRigidbody.velocity += currVelocity;


        // MyRigidbody.AddRelativeForce(currVelocity, ForceMode.Impulse);
        // transform.Translate(currVelocity * Time.deltaTime, Space.World);
    }
    private void Movement()
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

    }
    private void CalculateMoveDirection()
    {
        moveVector = new Vector3(PlayerInputHandler.MoveInput.x, 0, PlayerInputHandler.MoveInput.y).normalized;
        Vector3 cameraDirection = cameraController.planarDir;
        Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraDirection);

        moveVector = cameraPlanarRotation * moveVector;
    }
    private void ApplyThreshold()
    {
        if (MyRigidbody.velocity.magnitude < velocityDeadThreshold)
        {
            MyRigidbody.velocity = Vector3.zero;
        }
    }
    private void Jump()
    {
        if (IsGrounded() || IsHanging)
        {

        MyRigidbody.velocity += jumpVelocity * Vector3.up;
        IsHanging = false;
        MyRigidbody.useGravity = true;
        }

    }
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.1f, -1);
    }
    private void LedgeGrab()
    {
        RaycastHit downHit;
        Vector3 lineDownStart = (transform.position + Vector3.up * 1.8f) + transform.forward;
        Vector3 lineDownEnd = (transform.position + Vector3.up * 0.7f) + transform.forward;
        Physics.Linecast(lineDownStart, lineDownEnd, out downHit);

        if (downHit.collider == null)
            return;

        RaycastHit forwardHit;
        Vector3 lineForwardStart = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z);
        Vector3 lineForwardEnd = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z) + transform.forward;
        Physics.Linecast(lineForwardStart, lineForwardEnd, out forwardHit);

        if (forwardHit.collider == null)
            return;

        MyRigidbody.useGravity = false;
        MyRigidbody.velocity = Vector3.zero;

        IsHanging = true;

        // Vector3 hangPosition = new Vector3(forwardHit.point.x, downHit.point.y, forwardHit.point.z);
        // Vector3 offset = transform.forward * -0.1f + transform.up * -1f;
        // hangPosition += offset;

        // transform.position = hangPosition;

        transform.forward = -forwardHit.normal;

    }
    private void OnDisable()
    {
        PlayerInputHandler.OnJump -= Jump;
    }
}
