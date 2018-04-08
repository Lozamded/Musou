using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    public float lookRadius = 10f;
    public float ReclutationRadius = 12f;
    public string estado = "esperando";

    public bool es_lider = false;

    Transform target;
    Transform player;
    Transform colega_cercano;
    NavMeshAgent agent;

	// Use this for initialization
	void Start ()
    {
        player = PlayerManager.instance.player.transform; //Para usar un target generico para le enemigo que es el personaje.

        if (es_lider == false)
        {
            target = EnemyManager.instance.enemy.transform; //Para usar un target generico en el enemigo que es que siga al lider.
        }
        else {

            target = player; //Para usar un target generico para le enemigo que es el personaje.
        }
        
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
    {
            float distance = Vector3.Distance(target.position, transform.position);

            if (distance <= lookRadius)
            {
                agent.SetDestination(target.position);

                if (distance <= agent.stoppingDistance)
                {
                    //Atacar el tarjet
                    FaceTarget();
                }
            }
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
