using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class TankMovement : MonoBehaviour {
    public float m_Speed = 2f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;

    private Rigidbody m_Rigidbody;
    private float m_OriginalPitch;
    private float m_MovementInputValue;


    private void Awake() {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable() {
        m_Rigidbody.isKinematic = false;
    }


    private void OnDisable() {
        m_Rigidbody.isKinematic = true;
    }


    private void Start() {
        m_OriginalPitch = m_MovementAudio.pitch;
    }

    private void Update() {
        // Store the player's input and make sure the audio for the engine is playing.
        EngineAudio();
    }

    private void FixedUpdate() {
        // Adjust the position of the tank based on the player's input.
        Vector3 movement = transform.forward * (m_MovementInputValue * m_Speed * Time.deltaTime);
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        
        
        m_MovementInputValue = 0;
    }

    private void EngineAudio() {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
        if (Mathf.Abs(m_MovementInputValue) < 0.1f) {
            if (m_MovementAudio.clip == m_EngineDriving) {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else {
            if (m_MovementAudio.clip == m_EngineIdling) {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context) {
        if (context.performed) {
            m_MovementInputValue = context.ReadValue<float>();
        }
    }
}
