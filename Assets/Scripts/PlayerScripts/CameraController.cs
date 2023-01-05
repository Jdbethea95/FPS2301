using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] int sensHor;
    [SerializeField] int sensVer;

    [SerializeField] float lockMin;
    [SerializeField] float lockMax;

    [SerializeField] bool invertX;

    float xRotation;

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
