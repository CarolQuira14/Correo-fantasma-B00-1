using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApagarLinterna : MonoBehaviour
{
    public GameObject linterna;
    public Light luzlint;
    // Start is called before the first frame update
    void Start()
    {
        linterna = GetComponent<GameObject>();
        luzlint = GetComponentInChildren<Light>();
    }
    public void Apagar(){
        luzlint.enabled = false;
    }
}
