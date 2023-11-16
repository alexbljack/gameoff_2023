using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float activeBorderWidth = 0.05f;
    [SerializeField] float screenDeadZone = 0.15f;

    float ScreenCoeff => 1 - activeBorderWidth;
    float HeightDz => Screen.height * screenDeadZone;
    float WidthDz => Screen.width * screenDeadZone;

    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        Vector3 translation = Vector3.zero;
        
        if (mouse.y >= Screen.height * ScreenCoeff && mouse.y <= Screen.height + HeightDz) { translation += Vector3.up; }
        if (mouse.y >= -HeightDz && mouse.y <= activeBorderWidth * Screen.height) { translation += Vector3.down; }
        if (mouse.x >= Screen.width * ScreenCoeff && mouse.x <= Screen.width + WidthDz) { translation += Vector3.right; }
        if (mouse.x >= -WidthDz && mouse.x <= activeBorderWidth * Screen.width) { translation += Vector3.left; }
        
        if (translation.magnitude >= 0.1)
        {
            transform.Translate(translation * Time.deltaTime * moveSpeed, Space.World);
        }
    }
}
