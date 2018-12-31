using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextoDano : MonoBehaviour
{

    private Vector3 Target;
    public float speed = 6;
    public float subida = 6f;

    public Vector3 posInicial;

    public bool moving = false;

    private float timer;

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0);
        Gizmos.DrawLine(transform.position, Target);
    }
    // Use this for initialization
    void Start()
    {
        posInicial = transform.position;
        Target = new Vector3(transform.position.x, transform.position.y + subida, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {

        if (moving == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target, speed * Time.deltaTime);

            if (transform.position == Target)
            {
                moving = false;
                transform.position = posInicial;
                this.gameObject.GetComponent<TextMeshProUGUI>().text = "";
            }
        }

    }

    public void move()
    {
        posInicial = transform.position;
        Target = new Vector3(transform.position.x, transform.position.y + subida, transform.position.z);
        moving = true;
    }
}

