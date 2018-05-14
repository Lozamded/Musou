using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : EnemyStats {

    //Variables
    public float lookRadius = 15f;
    public float bastionRadius = 40f;
    public float circleRadius = 6.5f;
    public LayerMask LayerMask;
    public float ReclutationRadius = 12f;
    float tiempo_reclutamiento;

    //Para caminata inicial por el area
    public float timer; // Para caminata inicial
    public float new_preTarget; //target de inicio
    public Vector3 preTarget; 

    //Para verificar estados
    public string estado;
    public string estado_previo;

    //Variables para eseguimiento
    GameObject cercano; //Se asignara aqui el objeto mas cercano;
    public int generadorPoint = 0; //Generador actual
    int shearchPoint = 0;
    public int bastionPoint = 0;

    //Variables de ruta y bastiones
    public List<Transform> path = new List<Transform>();
    public GameObject bastion; //Se asignara el bastion donde fue creado;
    Transform target;
    Transform player;
    NavMeshAgent agent;
    public List<Transform> bastiones = new List<Transform>();
    
    //Variables para colision
    Transform bottom;
    Transform front; 
    GameObject bottomObject;

    //Variables de circulo de Kung-Fu
    public Transform KungfuPoint;
    public int indiceKungfuPoint;

	// Use this for initialization
	void Start ()
    {
        player = PlayerManager.instance.player.transform; //Para usar un target generico para le enemigo que es el personaje.
        target = player; //Para usar un target generico para le enemigo que es el personaje.

        bottom = this.gameObject.transform.GetChild(1);
        front = this.gameObject.transform.GetChild(2);
        bottomObject = this.gameObject.transform.GetChild(1).gameObject;

        agent = GetComponent<NavMeshAgent>();

        if(es_lider == false)
        {
            estado = "esperando";
            //Debug.Log("Me voy al centro del bastion");
            agent.SetDestination(bastion.transform.position);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
      
       float distance = Vector3.Distance(target.gameObject.transform.position, transform.position);
       float distance_player = Vector3.Distance(player.position, transform.position);
       float distance_bastion = Vector3.Distance(bastion.transform.position, transform.position);

       

        if (es_lider == true) //Si es un lider
        {   

            switch (estado)
            {
                case "reclutando":

                    agent.SetDestination(path[generadorPoint].transform.position);

                    timer += Time.deltaTime;
                    if (timer >= tiempo_reclutamiento)
                    {
                        if (soldados > 5)
                        {
                            estado = "busqueda";

                            bastionPoint = UnityEngine.Random.Range(1,5);
                            Debug.Log("Parti mi busquea, voy a el  " + bastionPoint);
                        }
                    }

                    float distance_generador = Vector3.Distance(path[generadorPoint].transform.position, transform.position);
                    if (distance_generador < 5)
                    {

                        if (generadorPoint >= path.Count - 1)
                        {
                            generadorPoint = 0;
                        }
                        else { generadorPoint++; }

                        //Debug.Log("Encontre un generador, ahora voy por el " + currentPoint);

                    }


                    break;

                case "busqueda":

                    if (distance_player > lookRadius)
                    { 

                        agent.SetDestination(bastiones[bastionPoint].transform.position);


                        float distance_bastiones = Vector3.Distance(bastiones[bastionPoint].transform.position, transform.position);
                        if (distance_bastiones < 10)
                        {

                            switch(bastionPoint)
                            {

                                case 0:
                                    bastionPoint = UnityEngine.Random.Range(1,5);  
                                break;

                                case 1:
                                    bastionPoint = UnityEngine.Random.Range(2,5);
                                break;

                                case 2:
                                    bastionPoint = UnityEngine.Random.Range(3,6);
                                break;

                                case 3:
                                    bastionPoint = UnityEngine.Random.Range(2,6);
                                break;

                                case 4:
                                    bastionPoint = UnityEngine.Random.Range(3,6);
                                break;
                            }

                            Debug.Log("Me viro " + "al " + bastionPoint);
                        }
                    }else
                    {
                        estado_previo = estado;
                        estado = "Persecucion";
                    }

                break;


                case "persecucion":

                    if (distance_player < lookRadius)
                    {
                        agent.SetDestination(target.position);
                        agent.speed = getVelocidad();

                        if (distance <= agent.stoppingDistance)
                        {
                            //Atacar el tarjet
                            FaceTarget(target);
                        }
                    }
                    else
                    {
                        estado = estado_previo;
                    }
                break;
            }
     
        }
        else
        { //Si no es un lider

            //Debug.Log(agent.destination);

            switch(estado)
            {
                case "esperando":
                    
                    //Recorrido random
                    if (distance_bastion < bastionRadius)
                    {
                        timer += Time.deltaTime; 
                        if(timer >= new_preTarget)
                        {
                            timer = 0;
                            newPreTarget();
                            new_preTarget = UnityEngine.Random.Range(6, 12);
                            //Debug.Log("pre target en : " + new_preTarget + "segundos ");
                        }
                    }
                    else
                    {
                        agent.SetDestination(bastion.transform.position);
                        //Debug.Log("Me pase me devuelvo al bastion");
                        //Debug.Log("voy para: " + bastion.transform.position);
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
                                    //Debug.Log("Tengo nuevo lider: ");
                                    //Debug.Log(item.name);
                                    //target = item.gameObject.transform;
                                    target = item.gameObject.GetComponent<EnemyController>().bottomObject.transform;
                                    lider = item.gameObject;
                                    GetComponent<EnemyStats>().velocidad = lider.gameObject.GetComponent<EnemyStats>().velocidad - 1;
                                    lider.gameObject.GetComponent<EnemyController>().soldados += 1;
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
                        if(distance_player <= circleRadius)
                        {
                            //Debug.Log("Me acerco al circurlo");
                            //estado = "Esperando circulo";
                            ValidarCirculo();
                        }
                        else {
                            if (KungfuPoint != null)
                            {
                                //Debug.Log("Perdi mi target");
                                player.gameObject.GetComponent<PlayerController>().KungFuPointsChecker[indiceKungfuPoint] = false;
                                GetComponentInChildren<colorChanger>().GrayColor();//Pintar color plateado
                            }
                            estado = "perseguir";
                        }


                        
                    }
                    else
                    {
                        agent.SetDestination(target.position);
                        agent.stoppingDistance = 1;

                        if (distance > 25)
                        {
                            velocidad = lider.gameObject.GetComponent<EnemyStats>().velocidad + 3;
                        }
                    }

                break;

                case "combate":
                    //Atacar el tarjet
                    FaceTarget(player);
                    agent.SetDestination(target.position);

                    if (distance_player > lookRadius)
                    {
                        if (KungfuPoint != null)
                        {
                            //Debug.Log("Perdi mi target");
                            player.gameObject.GetComponent<PlayerController>().KungFuPointsChecker[indiceKungfuPoint] = false;
                            GetComponentInChildren<colorChanger>().GrayColor();//Pintar color plateado
                            target = player;
                            KungfuPoint = null;
                            estado = "perseguir";
                            GetComponent<NavMeshAgent>().avoidancePriority = 20;
                        }
                    }

                    /*
                    if (KungfuPoint != null)
                    {
                        if (player.gameObject.GetComponent<PlayerController>().Rotando == true)
                        {
                            Debug.Log("Vuelta...");
                            rotarCirculo();
                        }
                    }*/
                   

                    break;

                case "delante":

                    agent.speed = getVelocidad() + 5;

                    timer += Time.deltaTime;
                    if (timer >= 35)
                    {
                        timer = 0;
                        if (lider != null)
                        {
                            target = lider.gameObject.GetComponent<EnemyController>().bottomObject.transform;
                            velocidad = lider.gameObject.GetComponent<EnemyStats>().velocidad - 2;
                        }
                        else {
                            estado = "esperando";
                        }
                        
                        //Debug.Log("pre target en : " + new_preTarget + "segundos ");
                    }

                    break;
            }

        }
    }

    void OnCollisionEnter(Collision col)//Chequear si ha colisionado concon otra hormiga
    {
        if (es_lider == true && col.gameObject.name == "Enemy" && col.gameObject.GetComponent<EnemyController>().es_lider == false )
        {
            Debug.Log("toque otra hormiga");
            col.gameObject.GetComponent<EnemyController>().target = front;
            col.gameObject.GetComponent<EnemyController>().estado = "delante";
            col.gameObject.GetComponent<EnemyController>().timer = 0;
            col.gameObject.GetComponent<EnemyStats>().velocidad += 5;


        }
    }

    ///Funciones

    void newPreTarget() //Para buscar un lugar al azar en el mapa
    {
        float myX = gameObject.transform.position.x;
        float myZ = gameObject.transform.position.z;

        float xPos = myX + UnityEngine.Random.Range(myX - 1200, myX + 1200);
        float zPos = myX + UnityEngine.Random.Range(myZ - 1200, myZ + 1200);

        preTarget = new Vector3(xPos,gameObject.transform.position.y,zPos);
        //Debug.Log("Voy al azar hacia " + xPos +","+zPos);
        agent.SetDestination(preTarget);
    } 

    void FaceTarget(Transform target_to_look) //Mirar al target
    {
        Vector3 direction = (target_to_look.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void inicializarLider()//Valores iniciales del lider
    {
        GetComponentInChildren<colorChanger>().BlackColor();//Pintar color negro

        GetComponent<NavMeshAgent>().radius = 1;
        GetComponent<NavMeshAgent>().acceleration = 25;
        GetComponent<NavMeshAgent>().stoppingDistance = 0;
        GetComponent<NavMeshAgent>().avoidancePriority = 3;


        GetComponent<Rigidbody>().mass = 3;
        GetComponent<Rigidbody>().drag = 3;


        estado = "reclutando";
        tiempo_reclutamiento = UnityEngine.Random.Range(25f, 165f);//Tiempo que el lider reclutara soldados.

        //Busqueda de los generadores de enemigos para buscar reclutas.
        Collider[] colliders = Physics.OverlapSphere(transform.position, bastionRadius, LayerMask);
        Collider[] colliders_lejanos = Physics.OverlapSphere(transform.position, 9500, LayerMask);
        //Ordenar los arreglos.
        Array.Sort(colliders, new DistanceComparer(transform));
        Array.Sort(colliders_lejanos, new DistanceComparer(transform));

        foreach (Collider item in colliders)
        {
            if (item.gameObject.GetComponent<enemyGenerator>() == true)
            {
                //Debug.Log("Encontre un generador");
                path.Add(item.gameObject.transform);
            }

        }

        //Buscar los bastiones
        foreach (Collider item in colliders_lejanos)
        {

            if (item.gameObject.GetComponent<BastionController>() == true)
            {
                bastiones.Add(item.gameObject.transform);
            }
        }
    }

    public void ValidarCirculo()
    {
        //Debug.Log("revisar arreglo de kung fu");
        indiceKungfuPoint = 0;

        foreach (bool Point in player.gameObject.GetComponent<PlayerController>().KungFuPointsChecker)
        {
            if (Point == false)
            {
                KungfuPoint = player.gameObject.GetComponent<PlayerController>().KungFuPoints[indiceKungfuPoint].transform;
                player.gameObject.GetComponent<PlayerController>().KungFuPointsChecker[indiceKungfuPoint] = true;
                break;
            }
            indiceKungfuPoint++;
        }

        if (KungfuPoint != null)
        {
            //Debug.Log("Asignado Punto: " + indiceKungfuPoint);
            GetComponentInChildren<colorChanger>().RedColor();//Pintar color rojo
            target = KungfuPoint;
            agent.stoppingDistance = 1;
            estado = "combate";
            GetComponent<NavMeshAgent>().avoidancePriority = 4;
        }
        else {
            //Debug.Log("Estan todos ocupados ");
            target = player;
            agent.stoppingDistance = 6;
            estado = "perseguir";
        }
        
    }

    public void rotarCirculo()
    {
        if (indiceKungfuPoint < 4)
        {
            indiceKungfuPoint += 1;
        }
        else {
            Debug.Log("Vuelvo a 0");
            indiceKungfuPoint = 0;
        }
        KungfuPoint = player.gameObject.GetComponent<PlayerController>().KungFuPoints[indiceKungfuPoint].transform;
        player.gameObject.GetComponent<PlayerController>().KungFuPointsChecker[indiceKungfuPoint] = true;
        player.gameObject.GetComponent<PlayerController>().Rotando = false;
    }


    private void OnDrawGizmosSelected()//Para dibujar el lookRadius
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, circleRadius);
    }
}
