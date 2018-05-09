using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class openBlock : MonoBehaviour
{
    public List<Transform> movePoints = new List<Transform>(2);
    int currentPoint = 0;
    NavMeshAgent agent;
    float timer = 0f;
    float tiempoBloque = 0f;


    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(movePoints[currentPoint].transform.position);
        tiempoBloque = Random.Range(5f, 12f);
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        if (timer >= tiempoBloque)
        {
            timer = 0;
            tiempoBloque = Random.Range(25f, 225f);

            if (currentPoint >= movePoints.Count - 1)
            {
                currentPoint = 0;
            }
            else { currentPoint++; }
            //Debug.Log("Cambio");
            agent.SetDestination(movePoints[currentPoint].transform.position);
        }
    }
}
