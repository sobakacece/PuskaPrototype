using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInputHandler : MonoBehaviour
{
    public const string mouseXstring = "Mouse X";
    public const string mouseYstring = "Mouse Y";
    public const string mouseScrollingString = "Mouse ScrollWheel";
    public static float MouseXInput {get => UnityEngine.Input.GetAxis(mouseXstring);}
    public static float MouseYInput {get => UnityEngine.Input.GetAxis(mouseYstring);}
    public static float MouseScrollInput {get => UnityEngine.Input.GetAxis(mouseScrollingString);}
}
