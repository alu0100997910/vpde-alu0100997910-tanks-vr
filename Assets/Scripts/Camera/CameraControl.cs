using System;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public Rigidbody m_Rigidbody;

    private void FixedUpdate()
    {
        var currentRotation = m_Rigidbody.rotation.eulerAngles;
        Vector3 targetRotation = new Vector3(currentRotation.x,
            transform.rotation.eulerAngles.y,
            currentRotation.z);
        
        m_Rigidbody.rotation = Quaternion.Euler(targetRotation);
    }
}
