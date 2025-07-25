using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cambioEscena : MonoBehaviour
{
    private bool juegoPausado = false;
    public GameObject menuPausa;
    bool menuCartasAbiertoCE;
    private void Start()
    {
        menuCartasAbiertoCE = GameManager.menuCartasAbierto;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                ReanudarJuego();
            }
            else
            {
                Pausar();
                Debug.Log("Juego pausado");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pausar();
        }
        
    }
    public void Pausar()
    {
        menuPausa.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        juegoPausado = true;

        // Aquí puedes activar un panel UI de "Pausa"
    }

    public void ReanudarJuego()
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        juegoPausado = false;
        // Aquí puedes desactivar el panel UI de "Pausa"
    }


    // Start is called before the first frame update
    public void CambiarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }


    // Llamar esta función desde el botón
    public void AbrirPaginaWeb(string url)
    {
        Application.OpenURL(url);
    }
    public void SalirDelJuego()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego...");
    }

}
