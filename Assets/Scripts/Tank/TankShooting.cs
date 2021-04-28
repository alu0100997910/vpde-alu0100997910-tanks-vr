using System;
using Cinemachine;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour {
    public Rigidbody m_Shell;
    public Transform m_FireTransform;
    public Slider m_AimSlider;
    public AudioSource m_ShootingAudio;
    public AudioClip m_ChargingClip;
    public AudioClip m_FireClip;
    public float m_MinLaunchForce = 15f;
    public float m_MaxLaunchForce = 30f;
    public float m_MaxChargeTime = 2f;
    private PlayerInput m_PlayerInput;

    private float m_CurrentLaunchForce;
    private float m_ChargeSpeed;
    private bool m_Fired = true;
    private bool pressed;
    private CinemachineImpulseSource m_impulseSource;

    private void OnEnable() {
        m_CurrentLaunchForce = m_MinLaunchForce;
    }

    private void Start() {
        m_impulseSource = GetComponent<CinemachineImpulseSource>();
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }

    private void Update() {
        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired) {
            // at max charge, not yet fired
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
            return;
        }

        switch (pressed) {
            case true when m_Fired:
                m_Fired = false;
                m_CurrentLaunchForce = m_MinLaunchForce;

                m_ShootingAudio.clip = m_ChargingClip;
                m_ShootingAudio.Play();
                break;
            case true when !m_Fired:
                // Holding the fire button, not yet fired
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
                m_AimSlider.value = m_CurrentLaunchForce;
                break;
            case false when !m_Fired:
                Fire();
                break;
        }

        pressed = false;
    }

    public void OnFire(InputAction.CallbackContext context) {
        if (context.started) {
            Debug.Log("lel");
        }
        if (context.performed) {
            pressed = true;
        }

        if (context.canceled) {
            Debug.Log("lel2");

        }
    }


    private void Fire() {
        // Instantiate and launch the shell.
        m_Fired = true;
        pressed = false;

        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();
        if (m_impulseSource) {
            Debug.Log("impulsing");
            var impulseVelocity = 10f;
            m_impulseSource.GenerateImpulse(new Vector3(impulseVelocity, impulseVelocity, impulseVelocity));
        }

        m_CurrentLaunchForce = m_MinLaunchForce;

        m_AimSlider.value = m_MinLaunchForce;
    }
}
