using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyGenerator : MonoBehaviour {

  
    public GameObject enemy;
    public GameObject bastion;
    float timer = 0;
    public float create_time;

    // Use this for initialization
    void Start ()
    {
        create_time = Random.Range(5,20);
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        if(timer >= create_time)
        {
            //Debug.Log("Generando enemigo... a los " + timer);
            timer = 0f;
            create_time = Random.Range(5f, 20f);

            enemy.transform.position = transform.position;
            enemy.gameObject.GetComponent<EnemyController>().bastion = bastion;
            //Instantiate(enemy,transform.position,Quaternion.identity);
            Instantiate(enemy);

        }
    }
}
