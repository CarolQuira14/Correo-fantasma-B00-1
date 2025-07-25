using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class linterna : MonoBehaviour
{
    [Header("Linterna")]
    // Start is called before the first frame update
    public GameObject flashlight;
    List<GameObject> mantas = new List<GameObject>();
    public Light luz;
    public bool luzEncendida = false; // Estado inicial de la linterna
    /*Control rayo linterna*/
    [Header("Raycast")]
    [SerializeField] private LayerMask whatToDetect;
    // Start is called before the first frame update
    Ray ray;
    public float maxDistance = 10f;
    RaycastHit hit;
    
    public bool rayoEncendido;
    [Header("JumpScares")]
    public GameObject mantaCuerpo1;
    public GameObject mantaCuerpo2;
    public GameObject mantaCuerpo3;
    public GameObject ojos;
    public float velocidadFade = 0.3f;
    private float alphaActual = 0f;
    private Image imagenUI;
    public int clipIndexEscogido, ValorPaneo;
    public List<AudioClip> clipsSusto = new();
    public AudioSource aSource;
    private Coroutine ataqueAlmasCoroutine = null;
    public float tiempoParaAtaque = 15f;
    private float tiempoLinternaEncendida = 0f;
    public GameObject panelTeObservo;
    [Header("PostProccesing")]
    public PostProcessVolume processVolume;
    public PostProcessProfile postProfileIDLE, postProfileAlmas; // Perfil normal yperfil del ataque de almas
    

    void Start()
    {
        mantas.Add(mantaCuerpo1);
        mantas.Add(mantaCuerpo2);
        mantas.Add(mantaCuerpo3);
        flashlight = GameObject.Find("FlashlightColor");

        if (flashlight != null)
        {
            luz = flashlight.GetComponentInChildren<Light>();

            if (luz != null)
            {
                luzEncendida = luz.enabled; // Guarda el estado inicial
            }
        }

    }
    void Update()
    {
        // Detectar la tecla F
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleLight();
            flashlight.GetComponent<AudioSource>().Play();
            ojos.SetActive(!luzEncendida);
            clipIndexEscogido = Random.Range(0, 2);
            ValorPaneo = Random.Range(-1, 1);
            processVolume.profile = postProfileIDLE;
            if (!luzEncendida && ataqueAlmasCoroutine != null)
            {
                StopCoroutine(ataqueAlmasCoroutine);
                ataqueAlmasCoroutine = null;
            }

            tiempoLinternaEncendida = 0f; // Reset timer al encender o apagar
        }

        // Contar tiempo si la linterna está encendida
        if (luzEncendida)
        {
            panelTeObservo.SetActive(false);

            tiempoLinternaEncendida += Time.deltaTime;

            if (tiempoLinternaEncendida >= tiempoParaAtaque && ataqueAlmasCoroutine == null)
            {
                ataqueAlmasCoroutine = StartCoroutine(IniciarAtaqueAlmas());
            }
        }

        rayoEncendido = luzEncendida;
        ActivateRaycast();

    }
    IEnumerator IniciarAtaqueAlmas()
    {
        yield return new WaitForSeconds(5f);
        processVolume.profile = postProfileAlmas;
        AudioClip clipEscogido = clipsSusto[clipIndexEscogido];
        aSource.panStereo = ValorPaneo;
        aSource.PlayOneShot(clipEscogido);

        Debug.Log("sonando del ataque: " + clipEscogido.name);
        
    }
    public void ActivateRaycast()
    {
        if (rayoEncendido)
        {
            ray = new(luz.transform.position, luz.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green);

            if (Physics.Raycast(ray, out hit, maxDistance, whatToDetect))
            {
                //hit.collider.gameObject.GetComponent<Renderer>().enabled = true;
                //hit.transform.gameObject.GetComponent<Image>().fillAmount = 0.5f;
                imagenUI = hit.transform.GetComponent<Image>();
                AudioSource audioImagendescubierta = hit.transform.GetComponent<AudioSource>();
                if (imagenUI != null)
                {
                    alphaActual += Time.deltaTime * velocidadFade;
                    audioImagendescubierta.Play();
                    Debug.Log("mostrando Mensaje y sonando audio de una img");
                }
                foreach (GameObject manta in mantas)
                {
                    AudioSource audioJumpScare = hit.transform.GetComponent<AudioSource>();
                    if (audioJumpScare != null && !audioJumpScare.isPlaying)
                    {
                        audioJumpScare.Play();
                        Debug.Log("sonando sonido de manta");
                    }
                }
            }

            //animacion de fade in
            alphaActual = Mathf.Clamp01(alphaActual);
            SetAlpha(alphaActual);
        }
    }
    void SetAlpha(float alpha)
    {
        if (imagenUI != null)
        {
            Color tempColor = imagenUI.color;
            tempColor.a = alpha;
            imagenUI.color = tempColor;
        }
    }
    // Alternar el estado de la luz
    void ToggleLight()
    {
        luzEncendida = !luzEncendida; // Cambia el estado
        luz.enabled = luzEncendida;
    }

}
