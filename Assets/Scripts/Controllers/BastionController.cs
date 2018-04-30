using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class BastionController : MonoBehaviour {

    public string nombre;
    public string faccion;
    public float bastionRadius = 15f;
    public float centerRadius = 5f;
    public float timer; // Para caminata inicial
    public float tiempo_reunion;
    public GameObject candidato_lider;
    public LayerMask Layer;


    // Use this for initialization
    void Start ()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        tiempo_reunion = UnityEngine.Random.Range(15f, 30f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
 

        if (timer >= tiempo_reunion)
        {
            timer = 0f;
            tiempo_reunion = UnityEngine.Random.Range(15f, 30f);
            //Debug.Log("Revisar cuantas hay");

            Collider[] colliders = Physics.OverlapSphere(transform.position, radius: centerRadius, layerMask: Layer);
            Array.Sort(colliders, new DistanceComparer(transform));

            //Debug.Log("Hay: " + colliders.Length + " elementos");

            for (int i = 0; i < colliders.Length - 2; i++)
            {
                // Debug.Log(colliders[i].gameObject.GetComponent<EnemyController>().vida);
                if (colliders[i].gameObject.GetComponent<EnemyController>() == true)
                { 
                    float n1 = getVida(colliders, i);
                    float n2 = getVida(colliders, i + 1); ;
                    if (n1 > n2)
                    {
                        candidato_lider = colliders[i].gameObject;
                        //Debug.Log("Canditato a lider " + colliders[i].gameObject.GetComponent<EnemyStats>().vida + " de vida");
                    }
                }
                
            }

            if(candidato_lider != null)
            {
                //Debug.Log("Canditato a lider Ganador" + candidato_lider.GetComponent<EnemyStats>().vida + " de vida");
                candidato_lider.GetComponent<EnemyStats>().es_lider = true;
                candidato_lider.GetComponent<EnemyController>().es_lider = true;
                candidato_lider.GetComponent<EnemyController>().inicializarLider();
               
            }

            /*
            foreach (Collider item in colliders)
            {
                if (item.gameObject.GetComponent<EnemyStats>() == true)
                {
                    Debug.Log(item.name);
                }

            }*/
            
        }
    }

    private static float getVida(Collider[] collider, int i)
    {
        return collider[i].gameObject.GetComponent<EnemyController>().getVelocidad();
    }

    private void OnDrawGizmosSelected()//Para dibujar el lookRadius
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, bastionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, centerRadius);
    }
}

