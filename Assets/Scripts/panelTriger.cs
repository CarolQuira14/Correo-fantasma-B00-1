using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panelTriger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject panelx;
    private void OnTriggerEnter(Collider other)
    {
        panelx.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        panelx.SetActive(false);
    }
}
