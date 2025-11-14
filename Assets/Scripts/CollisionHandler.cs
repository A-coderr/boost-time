using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float levelLoaadDelay = 2f;
    [SerializeField] AudioClip successSound;
    [SerializeField] AudioClip crashSound;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;

    bool isControllable = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!isControllable)
        {
            return;
        }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Collided with Friendly!");
                break;
            case "Finish":
                StartSuccessSequence();

                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        successParticles.Play();
        GetComponent<Movement>().enabled = false; // Disable player movement
        Invoke("LoadNextLevel", levelLoaadDelay); // Delay load for 2 second
    }

    void StartCrashSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false; // Disable player movement
        Invoke("ReloadLevel", levelLoaadDelay); // Delay reload for 2 second
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0; // Loop back to the first scene
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

}
