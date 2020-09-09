using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnValidator : MonoBehaviour
{
    public GameObject thing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        thing = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        thing = null;
    }
}
