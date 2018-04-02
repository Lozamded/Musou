using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyGenerator : MonoBehaviour {

    public GameObject enemy;
    float timer = 0;
    float create_time = Random.Range(5,20);

    // Use this for initialization
    void Start ()
    {
  

    }
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        if(timer >= create_time)
        {
            Debug.Log("Generando enemigo... a los " + timer);
            timer = 0f;
            create_time = Random.Range(5f, 20f);

            Instantiate(enemy);

        }
    }
}
