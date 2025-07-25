using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManagerP : MonoBehaviour
{
    public AudioSource[] fuentesDeAudio;   // Lista de AudioSources repartidos por el mapa
    [Range(1, 10)] public int cantidadARandomizar = 3; // Cuántos se activan aleatoriamente
    public float delayEntreReproducciones = 10f; // opcional: intervalo automático
    public bool reproducirAutomatico = false;

    void Start()
    {
        if (reproducirAutomatico)
        {
            InvokeRepeating(nameof(ReproducirAleatorios), 1f, delayEntreReproducciones);
        }
    }

    public void ReproducirAleatorios()
    {
        // Primero, apagamos todos los sonidos para reiniciar
        foreach (var audio in fuentesDeAudio)
        {
            if (audio.isPlaying)
                audio.Stop();
        }

        // Seleccionar aleatoriamente fuentes sin repetir
        int[] indices = ObtenerIndicesUnicos(cantidadARandomizar, fuentesDeAudio.Length);

        foreach (int i in indices)
        {
            if (fuentesDeAudio[i] != null)
            {
                fuentesDeAudio[i].Play();
                Debug.Log("Reproduciendo: " + fuentesDeAudio[i].gameObject.name);
            }
        }
    }

    // Método auxiliar para obtener índices únicos aleatorios
    private int[] ObtenerIndicesUnicos(int cantidad, int total)
    {
        cantidad = Mathf.Clamp(cantidad, 0, total);
        System.Collections.Generic.List<int> indicesDisponibles = new();

        for (int i = 0; i < total; i++)
            indicesDisponibles.Add(i);

        int[] seleccionados = new int[cantidad];
        for (int j = 0; j < cantidad; j++)
        {
            int rand = Random.Range(0, indicesDisponibles.Count);
            seleccionados[j] = indicesDisponibles[rand];
            indicesDisponibles.RemoveAt(rand);
        }

        return seleccionados;
    }
}
