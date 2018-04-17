using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BastionController: MonoBehaviour {

    public string nombre;
    public string faccion;  
    public float bastionRadius = 15f; 
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnDrawGizmosSelected()//Para dibujar el lookRadius
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, bastionRadius);
    }
}
