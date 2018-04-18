using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : EnemyStats {

    //Variables
    public float lookRadius = 10f;
    public float bastionRadius = 40f;
    public LayerMask LayerMask;
    public float ReclutationRadius = 12f;
    float tiempo_reclutamiento;

    //Para caminata inicial por el area
    public float timer; // Para caminata inicial
    public int new_preTarget = 2; //target de inicio
    public Vector3 preTarget; 

    //Para verificar estados
    public string estado;

    //Variables para eseguimiento
    GameObject cercano; //Se asignara aqui el objeto mas cercano;
    public int currentPoint = 0; //Generador actual
    public List<Transform> path = new List<Transform>();
    public GameObject bastion; //Se asignara el bastion donde fue creado;
    Transform target;
    Transform player;
    NavMeshAgent agent;

	// Use this for initialization
	void Start ()
    {
        player = PlayerManager.instance.player.transform; //Para usar un target generico para le enemigo que es el personaje.
        target = player; //Para usar un target generico para le enemigo que es el personaje.

        agent = GetComponent<NavMeshAgent>();

        if(es_lider == true)
        {
            GetComponentInChildren<colorChanger>().OrangeColor();

            estado = "reclutando";
            tiempo_reclutamiento = UnityEngine.Random.Range(45f, 165f);

            //Busqueda de los generadores de enemigos para buscar reclutas.
            Collider[] colliders = Physics.OverlapSphere(transform.position, bastionRadius, LayerMask);
            Array.Sort(colliders, new DistanceComparer(transform));
            foreach (Collider item in colliders)
            {
                if (item.gameObject.GetComponent<enemyGenerator>() == true)
                {
                    //Debug.Log("Encontre un generador");
                    path.Add(item.gameObject.transform);
                }
            }
        }
        else
        {
            estado = "esperando";
            agent.SetDestination(bastion.transform.position);
        }
 
	}
	
	// Update is called once per frame
	void Update ()
    {
       float distance = Vector3.Distance(target.position, transform.position);
       float distance_player = Vector3.Distance(player.position, transform.position);
       //if(bastion != obj)
       float distance_bastion = Vector3.Distance(bastion.transform.position, transform.position);

       

        if (es_lider == true) //Si es un lider
        {
            switch(estado)
            {
                case "reclutando":

                    agent.SetDestination(path[currentPoint].transform.position);

                    timer += Time.deltaTime;
                    if (timer >= tiempo_reclutamiento)
                    {
                       if(soldados > 5)
                        {
                            estado = "busqueda";
                        }
                    }

                    float distance_generador = Vector3.Distance(path[currentPoint].transform.position, transform.position);
                    if(distance_generador < 5)
                    {
     
                        if(currentPoint >= path.Count-1)
                        {
                            currentPoint = 0;
                        }
                        else { currentPoint++; }

                        Debug.Log("Encontre un generador, ahora voy por el " + currentPoint);

                    }


                break;
          
                case "busqueda":
                    estado = "persecucion";
                break;


                case "persecucion":

                    agent.SetDestination(target.position);
                    agent.speed = getVelocidad();

                    if (distance <= agent.stoppingDistance)
                    {
                        //Atacar el tarjet
                        FaceTarget();
                    }
                break;
            }
     
        }else{ //Si no es un lider

            switch(estado)
            {
                case "esperando":

                    //Recorrido random
                    if (distance_bastion < bastionRadius)
                    {
                        timer += Time.deltaTime; 
                        if(timer >= new_preTarget)
                        {
                            newPreTarget();
                            new_preTarget = UnityEngine.Random.Range(6, 12);
                            //Debug.Log("pre target en : " + new_preTarget + "segundos ");
                            timer = 0;
                        }
                    }
                    else
                    {
                        agent.SetDestination(bastion.transform.position);
                        Debug.Log("Me pase me devuelvo al bastion");
                        Debug.Log("voy para: " + bastion.transform.position);
                    }


                    //Busqueda de un lider
                    Collider[] colliders = Physics.OverlapSphere(transform.position, lookRadius, LayerMask);
                    Array.Sort(colliders, new DistanceComparer(transform));

                    foreach(Collider item in colliders)
                    {
                        // Debug.Log("El orden es:");
                        // Debug.Log(item.name);
                        if (distance_player <= lookRadius)
                        {
                            agent.SetDestination(player.position);
                        }
                        else
                        {
                            if (item.gameObject.GetComponent<EnemyController>() == true)
                            {
                                //Debug.Log("Encontre un simil");
                                if (item.gameObject.GetComponent<EnemyController>().es_lider == true)
                                {
                                    item.gameObject.GetComponent<EnemyController>().soldados += 1;
                                    Debug.Log("Tengo nuevo lider: ");
                                    Debug.Log(item.name);
                                    target = item.gameObject.transform;
                                    target.GetComponent<EnemyController>().soldados += 1;
                                    estado = "perseguir";
                                    
                               
                                }
                            }

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

        float xPos = myX + UnityEngine.Random.Range(myX - 1200, myX + 1200);
        float zPos = myX + UnityEngine.Random.Range(myZ - 1200, myZ + 1200);

        preTarget = new Vector3(xPos,gameObject.transform.position.y,zPos);
        Debug.Log("Voy al azar hacia " + xPos +","+zPos);
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
