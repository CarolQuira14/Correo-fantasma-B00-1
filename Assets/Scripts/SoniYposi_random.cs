using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoniYposi_random : MonoBehaviour
{
    public AudioSource[] fuentesDeAudio;             // Todos los AudioSources repartidos por el mapa
    [Range(1, 10)] public int cantidadARandomizar = 4;  // Cuántos sonidos se reproducen al azar
    public bool reproducirAutomatico = true;         // Si quieres que lo haga solo cada cierto tiempo
    public float intervalo = 20f;                    // Tiempo entre mezclas

    private List<AudioSource> sonidosActivos = new();

    void Start()
    {
        if (reproducirAutomatico)
        {
            InvokeRepeating(nameof(ReproducirAleatorios), 1f, intervalo);
        }
        else
        {
            ReproducirAleatorios(); // Solo una vez si no es automático
        }
    }

    public void ReproducirAleatorios()
    {
        // Detener y limpiar los anteriores
        foreach (AudioSource fuente in sonidosActivos)
        {
            if (fuente != null && fuente.isPlaying)
            {
                fuente.Stop();
            }
        }
        sonidosActivos.Clear();

        // Elegir nuevos aleatorios
        int[] indices = ObtenerIndicesUnicos(cantidadARandomizar, fuentesDeAudio.Length);

        foreach (int i in indices)
        {
            AudioSource fuente = fuentesDeAudio[i];
            if (fuente != null)
            {
                fuente.Play();
                sonidosActivos.Add(fuente);
                Debug.Log("Reproduciendo: " + fuente.gameObject.name);
            }
        }
    }

    private int[] ObtenerIndicesUnicos(int cantidad, int total)
    {
        cantidad = Mathf.Clamp(cantidad, 0, total);
        List<int> disponibles = new();

        for (int i = 0; i < total; i++)
            disponibles.Add(i);

        int[] resultado = new int[cantidad];
        for (int j = 0; j < cantidad; j++)
        {
            int rand = Random.Range(0, disponibles.Count);
            resultado[j] = disponibles[rand];
            disponibles.RemoveAt(rand);
        }

        return resultado;
    }
}
