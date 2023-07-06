using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CameraController : MonoBehaviour
{
    [Header("Framing")]
    [SerializeField] private Camera _camera = null;
    [SerializeField] private Transform followTransform = null;
    [SerializeField] private Vector3 framing = new Vector3(0, 0, 0);

    [Header("Rotation")]
    [SerializeField][Range(-90, 90)] private float defaulVertAngle = 20f;
    [SerializeField][Range(-90, 90)] private float minVerticalAngle = -90;
    [SerializeField][Range(-90, 90)] private float maxVerticalAngle = 90;
    [SerializeField] private float rotationSparpness = 25f;

    [Header("Distance")]
    [SerializeField] private float defaultDistance = 10f;
    [SerializeField] private float minDistance = 0f;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float zoomSpeed = 15;
    
    [Header("Obstacles")]
    [SerializeField] private float obstacleCheckRadius = 0.2f;
    [SerializeField] private LayerMask obstacleLayer = -1;
    private List<Collider> ignoreColliders = new List<Collider>();

    public Vector3 planarDir;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float targetVerticalAngle;
    private float targetDistance;

    private Vector3 smoothPosition;
    private Quaternion smoothRotation;
    //executes every time when values in the Inspector change
    private void OnValidate()
    {
        defaultDistance = Mathf.Clamp(defaultDistance, minDistance, maxDistance);
        defaulVertAngle = Mathf.Clamp(defaulVertAngle, minDistance, maxDistance);
    }

    private void Start()
    {
        ignoreColliders.AddRange(GetComponentsInChildren<Collider>());

        targetVerticalAngle = defaulVertAngle;

        targetDistance = defaultDistance;
        planarDir = followTransform.forward;

        targetRotation = Quaternion.LookRotation(planarDir) * Quaternion.Euler(targetVerticalAngle, 0, 0);
        targetPosition = followTransform.position - (targetRotation * Vector3.forward) * targetDistance;

        Cursor.lockState = CursorLockMode.Locked;
    }
    private void FixedUpdate()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            return;
        }
        Vector3 focusPosition = followTransform.position + _camera.transform.TransformDirection(framing);
        float inputX = PlayerInputHandler.MouseXInput;
        float inputY = PlayerInputHandler.MouseYInput;
        float zoom = PlayerInputHandler.MouseScrollInput * zoomSpeed;

        planarDir = Quaternion.Euler(0, inputX, 0) * planarDir;
        targetDistance = Mathf.Clamp(targetDistance + zoom, minDistance, maxDistance);
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle + inputY, minVerticalAngle, maxVerticalAngle);
        // Debug.DrawLine(camera.transform.position, camera.transform.position + planarDir, Color.red);

        //Handling obstacles
        float smallestDistance = targetDistance;
        RaycastHit[] hits = Physics.SphereCastAll(focusPosition, obstacleCheckRadius, targetRotation * Vector3.back, targetDistance, obstacleLayer);
       
        hits = hits.Where(x => !ignoreColliders.Contains(x.collider) && x.distance < smallestDistance).ToArray();
        if (hits.Length != 0)
        {
            smallestDistance = hits
            .Select(x => x.distance)
            .Min();
        }

        targetRotation = Quaternion.LookRotation(planarDir) * Quaternion.Euler(targetVerticalAngle, 0, 0);
        targetPosition = focusPosition - (targetRotation * Vector3.forward) * smallestDistance;

        //Smoothing
        smoothRotation = Quaternion.Slerp(_camera.transform.rotation, targetRotation, Time.deltaTime * rotationSparpness);
        smoothPosition = Vector3.Lerp(_camera.transform.position, targetPosition, Time.deltaTime * rotationSparpness);

        _camera.transform.rotation = smoothRotation;
        _camera.transform.position = smoothPosition;
    }
}
