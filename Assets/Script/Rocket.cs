using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    bool collisionsDisabled = false;

    enum State { Alive, Dying, Transcending}
    State state = State.Alive;

    // Start is called before the first frame update

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKey();
        }      
    }

    private void RespondToDebugKey()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            FindObjectOfType<Level>().LoadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled; //toggle

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionsDisabled){return;}

        switch (collision.gameObject.tag)
        {
            case "Friendly":

                break;
            case "Done":
                state = State.Transcending;
                audioSource.Stop();
                audioSource.PlayOneShot(success);
                successParticles.Play();
                Invoke("LoadNextLevel", levelLoadDelay);
                break;

            default:

                state = State.Dying;
                audioSource.Stop();
                audioSource.PlayOneShot(death);
                deathParticles.Play();
                Invoke("ReloadLevel", levelLoadDelay);
            
                break;
        }
    }
    private void LoadNextLevel()
    {
        FindObjectOfType<Level>().LoadNextScene();
    }
    private void ReloadLevel()
    {
        FindObjectOfType<Level>().ReloadLevel();
    }

    private void RespondToRotateInput()
    {
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up  * thrustThisFrame);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }
}
