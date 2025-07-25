using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class controladorVolumen : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider sliderMusica;
    public Slider sliderEfectos;

    void Start()
    {
        float volumenMusica = PlayerPrefs.GetFloat("volumenMusica", 0.75f);
        float volumenEfectos = PlayerPrefs.GetFloat("volumenEfectos", 0.75f);

        sliderMusica.value = volumenMusica;
        sliderEfectos.value = volumenEfectos;

        CambiarVolumenMusica(volumenMusica);
        CambiarVolumenEfectos(volumenEfectos);

        // Conectar eventos manualmente si no se hace desde el Inspector
        sliderMusica.onValueChanged.AddListener(CambiarVolumenMusica);
        sliderEfectos.onValueChanged.AddListener(CambiarVolumenEfectos);
    }

    public void CambiarVolumenMusica(float valor)
    {
        mixer.SetFloat("VolumenMusica", Mathf.Log10(valor) * 20); // para decibeles
        PlayerPrefs.SetFloat("volumenMusica", valor);
    }

    public void CambiarVolumenEfectos(float valor)
    {
        mixer.SetFloat("VolumenEfectos", Mathf.Log10(valor) * 20);
        PlayerPrefs.SetFloat("volumenEfectos", valor);
    }
}
