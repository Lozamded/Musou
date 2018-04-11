using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {



    public bool es_lider = false;
    public Transform lider;

	// Use this for initialization
	void Start ()
    {

        int vida = Random.Range(10, 20);
        Debug.Log("vida: " + vida);
        int resistencia = Random.Range(10, 20);
        int velocidad = Random.Range(10, 20);
        int ataque = Random.Range(20, 50);

        if( vida > 15 && resistencia > 15 && ataque > 30)
        {
            es_lider = true;
            Debug.Log("Soy un lider");
        }
    }

	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
