using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    
    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        Vector3 translation = Vector3.zero;
        
        if (mouse.y >= Screen.height) { translation += Vector3.up; }
        if (mouse.y <= 0) { translation += Vector3.down; }
        if (mouse.x >= Screen.width) { translation += Vector3.right; }
        if (mouse.x <= 0) { translation += Vector3.left; }
        
        if (translation.magnitude >= 0.1)
        {
            transform.Translate(translation * Time.deltaTime * moveSpeed, Space.World);
        }
    }
}
