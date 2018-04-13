using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : EnemyStats {

    //Variables
    public float lookRadius = 10f;
    public LayerMask LayerMask;
    public float ReclutationRadius = 12f;

    //Para caminata inicial por el area
    public float timer; // Para caminata inicial
    public int new_preTarget = 2; //target de inicio
    public Vector3 preTarget; 

    //Para verificar estados
    public string estado;

    GameObject cercano; //Se asignara aqui el objeto mas cercano
    Transform target;
    Transform player;
    NavMeshAgent agent;

	// Use this for initialization
	void Start ()
    {
        player = PlayerManager.instance.player.transform; //Para usar un target generico para le enemigo que es el personaje.

        if (es_lider == false)//Si el enemigo es un lider
        {
           target = EnemyManager.instance.enemy.transform; //Para usar un target generico en el enemigo que es que siga al lider.
        }
        else {

            target = player; //Para usar un target generico para le enemigo que es el personaje.
            //this.GetComponent<Renderer>().material.color = Color.blue;
        }
        
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
    {
       float distance = Vector3.Distance(target.position, transform.position);
       float distance_player = Vector3.Distance(player.position, transform.position);

       

        if (es_lider == true) //Si es un lider
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                //Atacar el tarjet
                FaceTarget();
            }
        }else{ //Si no es un lider

            switch(estado)
            {
                case "esperando":

                    //Recorrido random
                    timer += Time.deltaTime; 
                    if(timer >= new_preTarget)
                    {
                        newPreTarget();
                        new_preTarget = UnityEngine.Random.Range(1, 4);
                       timer = 0;
                    }

      
                    //Busqueda de un lider
                    Collider[] colliders = Physics.OverlapSphere(transform.position, lookRadius, LayerMask);
                    Array.Sort(colliders, new DistanceComparer(transform));

                    foreach(Collider item in colliders)
                    {
                        //Debug.Log("El orden es:");
                        //Debug.Log(item.name);
                        if(item.gameObject.GetComponent<EnemyController>().es_lider == true)
                        {
                            //item.gameObject.GetComponent<EnemyController>().soldados += 1;
                            Debug.Log("Tengo nuevo lider: ");
                            Debug.Log(item.name);
                            target = item.gameObject.transform;
                            target.GetComponent<EnemyController>().soldados += 1;
                            estado = "perseguir";
                        }
                    }

                break;

                case "perseguir":

                    agent.speed = getVelocidad();

                    if (distance_player <= lookRadius)
                    {
                        agent.SetDestination(player.position);


                        if (distance <= agent.stoppingDistance)
                        {
                            //Atacar el tarjet
                            FaceTarget();
                        }
                    }
                    else
                    {
                        agent.SetDestination(target.position);

                        if (distance <= agent.stoppingDistance)
                        {
                            //Atacar el tarjet
                            FaceTarget();
                        }
                    }

                break;
            }

        }
    }

    void newPreTarget()
    {
        float myX = gameObject.transform.position.x;
        float myZ = gameObject.transform.position.z;

        float xPos = myX + UnityEngine.Random.Range(myX - 100, myX + 100);
        float zPos = myX + UnityEngine.Random.Range(myZ - 100, myZ + 100);

        preTarget = new Vector3(xPos,gameObject.transform.position.y,zPos);
        agent.SetDestination(preTarget);
    } 

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()//Para dibujar el lookRadius
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
