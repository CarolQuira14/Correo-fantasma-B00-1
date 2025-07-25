using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MovimientoJugad : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Camara")]
    public Camera followCamera;
    public float sensiMouse = 3.0f;
    private float rotY, rotX;
    [SerializeField] private float MaxRota;
    [SerializeField] private float angMax;

    [Header("Movimiento")]
    [SerializeField] private CharacterController controller;
    [SerializeField] public float veloMovi = 1f;
    public float veloRota = 10f;
    [Header("Salto")]
    [SerializeField] private float gravedad = -385.83f;
    public float salto = 4f;
    public static float X, Z;
    public Transform checkPiso;
    public float distanciaPiso = 0.04f;
    public LayerMask Piso;
    public bool enPiso, recogioLinterna;
    [Header("otros")]
    public bool sobreEnemigo;
    public Animator animPuerta;
    public GameObject panelF, panelPuerta, panelLlave, panelCarta;
    AudioSource audio;
    Vector3 velocidad;

    void Start()
    {
        recogioLinterna = false;
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        enPiso = Physics.CheckSphere(checkPiso.position, distanciaPiso, Piso);
        if (recogioLinterna)
        {
            Invoke("controlPanelF", 10f);
        }

        //Debug.Log(enPiso+"  "+ checkPiso.position);

        if (enPiso && velocidad.y < 0)
        {
            //anim.SetBool("Jump", false);            
            velocidad.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && enPiso)
        {
            //anim.SetBool("Jump", true);
            velocidad.y = Mathf.Sqrt(salto * -2f * gravedad);
        }
        /*if (!enPiso){
            veloMovi = 5f;
        }
        else{
            veloMovi = 20f;
        }*/

        velocidad.y += gravedad * Time.deltaTime;
        controller.Move(velocidad * Time.deltaTime);
        RotarCamara();
        Movimiento();
    }

    public void Movimiento()
    {
        X = Input.GetAxis("Horizontal");
        Z = Input.GetAxis("Vertical");

        Vector3 moveInput = new Vector3(X, 0f, Z);
        moveInput = transform.TransformDirection(moveInput) * veloMovi;
        controller.Move(moveInput);
    }

    public void RotarCamara()
    {
        rotX = Input.GetAxis("Mouse X") * sensiMouse * Time.deltaTime;
        rotY = Input.GetAxis("Mouse Y") * sensiMouse * Time.deltaTime;

        MaxRota = MaxRota + rotY;
        MaxRota = Mathf.Clamp(MaxRota, -angMax, angMax);

        transform.Rotate(Vector3.up * rotX);
        followCamera.transform.localRotation = Quaternion.Euler(-MaxRota, 0f, 0f);

        /* rotationinput.x = Input.GetAxis("Mouse X") * rotationSensibility * Time.deltaTime;
         rotationinput.y = Input.GetAxis("Mouse Y") * rotationSensibility * Time.deltaTime;

         cameraVerticalAngle = cameraVerticalAngle + rotationinput.y;
         cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -70, 70);

         transform.Rotate(Vector3.up * rotationinput.x);
         playerCamera.transform.localRotation = Quaternion.Euler(-cameraVerticalAngle,0f,0f);*/
    }

    public void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Choqué con: " + collision.name);
        //Si el jugador se choca con una llave
        if (collision.name.Contains("Llave"))
        {
            GameManager.Instance.AgregarLlave(collision.tag.ToString());
            Debug.Log("Encontré la llave: " + collision.tag);
            panelLlave.SetActive(true);
            audio = collision.GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.Play();
                Destroy(collision.gameObject, audio.clip.length);
            }
        }

        //Si el jugador se choca con una puerta la puerta de salida
        if (collision.name.Contains("PuertaSalida"))
        {
            if (GameManager.Instance.Llaves.ContainsKey(collision.tag.ToString()))
            {
                GameManager.Instance.UsarLlave(collision.tag.ToString());
                animPuerta.SetBool("Open", true);
                panelPuerta.SetActive(false);
                audio = collision.GetComponent<AudioSource>();
                if (audio != null)
                {
                    audio.Play();
                }
                //Destroy(collision.gameObject);
                Debug.Log("Has abierto la puerta");
            }
            else
            {
                panelPuerta.SetActive(true);
                Debug.Log("Necesitas una llave para abrir la puerta");
            }
        }

        //Si el jugador se choca con una carta
        if (collision.name.Contains("Carta"))
        {
            GameManager.Instance.RecogerCarta(collision.tag.ToString());
            audio = collision.GetComponent<AudioSource>();
            if (audio != null)
            {
                panelCarta.SetActive(true);
                audio.Play();
                Destroy(collision.gameObject, audio.clip.length);
            }

        }
        //Si el jugador se choca con una puerta 
        if (collision.name.Contains("PuertaMela"))
        {
            Animator puertaAnimator = collision.GetComponent<Animator>();

            if (puertaAnimator != null)
            {
                puertaAnimator.SetBool("Open", true);
                collision.GetComponent<BoxCollider>().enabled = false; 
            }
            audio = collision.GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.Play();
            }
            Debug.Log("Has abierto la puerta");

            /*animPuerta.SetBool("Open", true);
            //Destroy(collision.gameObject);
            Debug.Log("Has abierto la puerta");*/
        }

        if (collision.name.Contains("boxColliderLinterna") & !recogioLinterna)
        {
            panelF.SetActive(true);
            recogioLinterna = true;
            Debug.Log("Encontré la linterna: " + collision.tag + "\n el bool es: " + recogioLinterna);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Si el jugador se choca con una llave
        if (other.name.Contains("Llave"))
        {
            panelLlave.SetActive(false);

        }
        //Si el jugador se choca con PuertaSalida
        if (other.name.Contains("PuertaSalida"))
        {
            panelPuerta.SetActive(false);

        }
        //Si el jugador se choca con una carta
        if (other.name.Contains("Carta"))
        {
            panelCarta.SetActive(false);

        }
    }
    public void controlPanelF()
    {
        panelF.SetActive(false);
    }
    /*
        void OnControllerColliderHit(ControllerColliderHit collision){
            if (collision.collider.CompareTag("Enemigo 1")){
               this.transform.GetComponent<PlayerManager>().getDamage();
            }
        }*/

}
