using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLigtRayCast : MonoBehaviour
{
    [SerializeField] private LayerMask whatToDetect;
    // Start is called before the first frame update
    Ray ray;
    public float maxDistance = 10f;
    RaycastHit hit;
    public float velocidadFade = 2f;
    private float alphaActual = 0f;
    private Image imagenUI;
    public linterna linterna;
    public bool rayoEncendido;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        rayoEncendido = linterna.luzEncendida;
        if (rayoEncendido)
        {
            ray = new(transform.position, transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green);

            if (Physics.Raycast(ray, out hit, maxDistance, whatToDetect))
            {
                //hit.collider.gameObject.GetComponent<Renderer>().enabled = true;
                //hit.transform.gameObject.GetComponent<Image>().fillAmount = 0.5f;
                imagenUI = hit.transform.GetComponent<Image>();
                if (imagenUI != null)
                {
                    alphaActual += Time.deltaTime * velocidadFade;
                }
                Debug.Log("mostrando Mensaje de una img");
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
}
