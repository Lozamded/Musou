using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingBlock : MonoBehaviour
{

    public List<Transform> movePoints = new List<Transform>();
    int currentPoint = 0;
    public float velocidad = 15f;
    NavMeshAgent agent;

    // Use this for initialization
    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        GetComponent<NavMeshAgent>().avoidancePriority = 1;
        agent.speed = velocidad;

    }

    // Update is called once per frame
    void Update ()
    {
        agent.SetDestination(movePoints[currentPoint].transform.position);

        float distanceToPoint = Vector3.Distance(movePoints[currentPoint].transform.position, transform.position);
        if (distanceToPoint < 8)
        {

            if (currentPoint >= movePoints.Count - 1)
            {
                currentPoint = 0;
            }
            else { currentPoint++; }

            //Debug.Log("Encontre un punto, ahora voy por el " + currentPoint);

        }


    }
}
