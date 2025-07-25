    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MujerEspejo : MonoBehaviour
{
    public Transform targetToLookAt; // normalmente la cámara
    public PostProcessVolume processVolume;
    public PostProcessProfile postProfileIDLE, postProfileTeObservo; // Perfil normal y perfil del ataque de almas
    public GameObject video;
    void Start()
    {
        processVolume.profile = postProfileIDLE;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetToLookAt != null)
        {
            // Hace que el canvas mire hacia el jugador o cámara
            transform.LookAt(targetToLookAt);

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            video.SetActive(true);
            processVolume.profile = postProfileTeObservo;

            gameObject.GetComponent<AudioSource>().Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Usuario esta en la mirada de los ojos");
        if (other.CompareTag("Player"))
        {
            video.SetActive(false);
            processVolume.profile = postProfileIDLE;
        }
    }
}
