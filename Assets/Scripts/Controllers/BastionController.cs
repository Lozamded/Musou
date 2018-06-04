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
            int[] ponderado_lider = new int[colliders.Length];
            //Debug.Log("Hay: " + colliders.Length + " elementos");
            


            for (int i = 0; i < colliders.Length - 2; i++)
            {
                // Debug.Log(colliders[i].gameObject.GetComponent<EnemyController>().vida);
                if (colliders[i].gameObject.GetComponent<EnemyController>() == true)
                {
                    ponderado_lider[i] = 0;

                    //Compararar la vida
                    float vida1 = getVida(colliders, i);
                    float vida2 = getVida(colliders, i + 1); 
                    if (vida1 > vida2)
                    {
                        ponderado_lider[i] += 12;
                    }

                    //Compararar la velocidad
                    float velocidad1 = getVelocidad(colliders, i);
                    float velocidad2 = getVelocidad(colliders, i + 1); 
                    if (velocidad1 > velocidad2)
                    {
                        ponderado_lider[i] += 6;
                    }

                    //Compararar la resistencia
                    float resistencia1 = getResistencia(colliders, i);
                    float resistencia2 = getResistencia(colliders, i + 1); 
                    if (resistencia1 > resistencia2)
                    {
                        ponderado_lider[i] += 4;
                    }

                    //Compararar el ataque
                    int ataque1 = getAtaque(colliders, i);
                    int ataque2 = getAtaque(colliders, i + 1);
                    if (ataque1 > ataque2)
                    {
                        ponderado_lider[i] += 8;
                    }

                    if(i > 0)
                    {
                        if (ponderado_lider[i] > ponderado_lider[i - 1])
                        {
                            candidato_lider = colliders[i].gameObject;
                        }

                    }
                }

            }

         
            if (candidato_lider != null)//Si se encontro un candidato a Lider
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
        return collider[i].gameObject.GetComponent<EnemyController>().getVida();
    }

    private static float getVelocidad(Collider[] collider, int i)
    {
        return collider[i].gameObject.GetComponent<EnemyController>().getVelocidad();
    }

    private static float getResistencia(Collider[] collider, int i)
    {
        return collider[i].gameObject.GetComponent<EnemyController>().getResistencia();
    }

    private static int getAtaque(Collider[] collider, int i)
    {
        return collider[i].gameObject.GetComponent<EnemyController>().getAtaque();
    }

    private void OnDrawGizmosSelected()//Para dibujar el lookRadius
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, bastionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, centerRadius);
    }
}

