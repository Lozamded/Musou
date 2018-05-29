using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {



    public bool es_lider = false;
    public GameObject lider;

    //Atributos
    public float resistencia;
    public float velocidad;
    public float vida;
    public int ataque; 

    public int soldados = 0;

	// Use this for initialization
	void Awake ()
    {

        vida = Random.Range(10f, 20f);
        //Debug.Log("vida: " + vida);
        resistencia = Random.Range(10f, 20f);
        velocidad = Random.Range(1f, 8f);
        ataque = Random.Range(10, 45);
        
        /*
        if( vida > 15 && resistencia > 15 && ataque > 30)
        {
            es_lider = true;
           //Debug.Log("Soy un lider");
        }
        */
    }

    public float getVelocidad()
    {
        return velocidad;
    }

    public float getVida()
    {
        return vida;
    }

    public float getResistencia()
    {
        return resistencia;
    }

    public int getAtaque()
    {
        return ataque;
    }


    // Update is called once per frame
    void Update ()
    {
		
	}
}
