using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("Horizontal Sensitivity")]
    [Range(100, 1000)] [SerializeField] float sensHor;
    [Tooltip("Vertical Sensitivity")]
    [Range(100, 1000)] [SerializeField] float sensVer;

    [Tooltip("Lower Vertical Camera Limit")]
    [Range(-180, 0)] [SerializeField] float lockMin;
    [Tooltip("Upper Vertical Camera Limit")]
    [Range(0, 180)] [SerializeField] float lockMax;

    [Tooltip("Invert Vertical Input")]
    [SerializeField] bool invertX;

    float xRotation;

    public float YSen 
    {
        get { return sensHor; }
        set { sensHor = value; }
    }
    public float XSen
    {
        get { return sensVer; }
        set { sensVer = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Rotation();
    }


    void Rotation()
    {
        float mouseY = Input.GetAxis("Mouse Y") * sensHor * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * sensVer * Time.deltaTime;

        if (invertX)
            xRotation += mouseY;
        else
            xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, lockMin, lockMax);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
