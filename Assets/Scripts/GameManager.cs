using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Dictionary<string, int> Llaves = new Dictionary<string, int>();
    public Dictionary<string, int> Cartas = new Dictionary<string, int>();

    public GameObject carta1, carta2;
    public GameObject carta1Menu, carta2Menu;
    public GameObject menuCartas;
    public static bool menuCartasAbierto = false;
    public TextMeshProUGUI cantidadCartasUI;
    public CanvasGroup panelUbicacion;
    public float fadeDuration = 4f;

    /*
     *  public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(true);
        Invoke("apagarPanel", 10f);
    }

    public void apagarPanel()
    {
        panel.SetActive(false);
    }
     */
    // Start is called before the first frame update
    void Start()
    {   
        Invoke("apagarPanelUbicacion", 4f);
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        carta1.SetActive(false);
        carta2.SetActive(false);
    }

    void Update()
    {
        ControlMenuCartas();
        if (Cartas.ContainsKey("Carta1"))
        {
            carta1.SetActive(true);
            carta2.SetActive(false);
        }
        if(Cartas.ContainsKey("Carta2")) {
        
            carta1.SetActive(false);
            carta2.SetActive(true);
        }
        if (Cartas.Count >= 2)
        {
            carta1.SetActive(true);
            carta2.SetActive(true);
        }
    }

    // PARA LAS LLAVES: 
    public void AgregarLlave(string keyName)
    {
        if (Llaves == null)
        {
            Llaves = new Dictionary<string, int>();
        }

        if (Llaves.ContainsKey(keyName))
        {
            Llaves[keyName]++;
        }
        else
        {
            Llaves.Add(keyName, 1);
        }
        Debug.Log("Encontraste: " + keyName);
        Debug.Log("Llaves encontradas: " + Llaves[keyName]);
    }

    public void UsarLlave(string keyName)
    {
        Llaves.Remove(keyName);
        Debug.Log("Perdiste el objeto Llave " + keyName);
        Debug.Log("Tienes: " + Llaves.Count + " llaves");
    }

    // PARA LAS CARTAS: 
    public void RecogerCarta(string keyName)
    {
        if (Cartas == null)
        {
            Cartas = new Dictionary<string, int>();
        }

        if (Cartas.ContainsKey(keyName))
        {
            Cartas[keyName]++;
        }
        else
        {
            Cartas.Add(keyName, 1);
            
        }
        Debug.Log("Encontraste: " + keyName);
        Debug.Log("Cartas por entregar: " + Cartas.Count);
    }

    public void EntregarCarta(string keyName)
    {
        Cartas.Remove(keyName);
        Debug.Log("Entregaste: " + keyName);
        Debug.Log("Cartas por entregar: " + Cartas.Count);
    }

    public void ControlMenuCartas()
    {
        //abrir
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (menuCartasAbierto)
            {
                if (Cartas.Count > 0)
                {
                    carta1Menu.SetActive(true);
                }
                menuCartas.SetActive(false);
                menuCartasAbierto = false;
            }else
            {
                menuCartas.SetActive(true);
                menuCartasAbierto = true;

                cantidadCartasUI.text = Cartas.Count.ToString();

                if (Cartas.ContainsKey("Carta1"))
                {
                    carta1Menu.SetActive(true);
                    carta2Menu.SetActive(false);
                }

                if (Cartas.ContainsKey("Carta2"))
                {
                    carta1Menu.SetActive(false);
                    carta2Menu.SetActive(true);
                }
            }

        }
        //cerrar
        if (Input.GetKeyDown(KeyCode.Escape) & menuCartasAbierto)
        {
            
        }

        //cambiar carta
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (Cartas.ContainsKey("Carta1"))
            {
                carta1Menu.SetActive(true);
                carta2Menu.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (Cartas.ContainsKey("Carta2"))
            {
                carta1Menu.SetActive(false);
                carta2Menu.SetActive(true);
            }
        }

    }
    public void apagarPanelUbicacion()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float startAlpha = panelUbicacion.alpha;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            panelUbicacion.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
            yield return null;
        }

        panelUbicacion.alpha = 0f;
        panelUbicacion.interactable = false;
        panelUbicacion.blocksRaycasts = false;
        panelUbicacion.gameObject.SetActive(false); // Opcional: desactiva el GameObject completamente
    }
}
