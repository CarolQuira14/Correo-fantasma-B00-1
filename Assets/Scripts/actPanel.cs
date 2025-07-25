using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class actPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject panelF;
    public GameObject flashlight;
    public Light luzlint;
    public GameObject player,ojos;

    public Rigidbody rb;
    public float timer=1000;
    public bool empezarTimer = false;
    private Animator anim;

    private void Start()
    {
        flashlight = GameObject.Find("FlashlightColor");
        luzlint = flashlight.GetComponentInChildren<Light>();
        player = GameObject.Find("attachLinterna");

        rb = flashlight.GetComponent<Rigidbody>();
        anim = GetComponentInParent<Animator>();
    }
    void Update()
    {
        if (luzlint.isActiveAndEnabled && empezarTimer){

            timer -= Time.deltaTime;
            if (timer < 0){
                anim.SetTrigger("Pasillo");
                empezarTimer = false ;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            /*panelF.SetActive(true);
            Invoke("controlPanel", 6.0f); //desactivarPanel a los 6sg*/
            Debug.Log("jugador entro en linterna");
            flashlight.transform.position = player.transform.position;
            flashlight.transform.SetParent(player.transform);
            flashlight.transform.localRotation = Quaternion.Euler(90, 0, 0); // Ajusta la rotaci�n (opcional)
        }

        if (other.CompareTag("detector")){
            empezarTimer = true;
            Debug.Log("Pasillo Detectado");
            timer = Random.Range((float)0.5, 3);
            ojos.SetActive(true);
            
        }

    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            panelF.SetActive(false);
            Debug.Log("usuarioSalio");
        }
    }*/

    public void ApagarLinterna(){
        luzlint.enabled = false;
    }
}
