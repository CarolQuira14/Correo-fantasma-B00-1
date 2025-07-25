using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
public class imgSeguirJugador : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform targetToLookAt; // normalmente la cámara
    public Image mensaje; // Asigna el panel (Image) desde el Inspector
    public GameObject panelMensaje; 
    public float velocidadTitilar = 2f;
    public PostProcessVolume processVolume;
    public PostProcessProfile postProfileIDLE, postProfileTeObservo; // Perfil normal y perfil del ataque de almasç
    // Update is called once per frame
    private void Start()
    {
        processVolume.profile = postProfileIDLE;
    }

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
            panelMensaje.SetActive(true);
            processVolume.profile = postProfileTeObservo;
            StartCoroutine(TitilarPanel());
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Usuario esta en la mirada de los ojos");
        if (other.CompareTag("Player"))
        {
            panelMensaje.SetActive(false);
            processVolume.profile = postProfileIDLE;
            StopCoroutine(TitilarPanel());
        }
    }

    IEnumerator TitilarPanel()
    {
        while (true)
        {
            float tiempo = 0f;

            while (tiempo < Mathf.PI * 2f)
            {
                float alpha = (Mathf.Sin(tiempo) + 1f) / 2f; // Oscila entre 0 y 1
                Color color = mensaje.color;
                color.a = alpha;
                mensaje.color = color;

                tiempo += Time.deltaTime * velocidadTitilar;
                yield return null;
            }
        }
    }
}
