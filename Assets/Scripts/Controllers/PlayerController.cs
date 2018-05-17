using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : MonoBehaviour
{

    public Interactive focus;
    public LayerMask movementMask;
    public List<Transform> KungFuPoints = new List<Transform>(5); //Siempre necesito 5 puntos
    public List<Transform> KungFuEnemys = new List<Transform>(5); //Siempre necesito 5 enemigos
    public float kungFuRadio = 4;
    float timer = 0; // para rotar en el circulo de KungFu
    float timerRotar = 0;
    float rotarPuntos;
    float rotarTime;
    public bool Rotando = false;


    Camera cam;
    PlayerMotor motor;

    public bool[] KungFuPointsChecker = new bool[5];

    // Use this for initialization
    void Awake ()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();

        KungFuPointsChecker[0] = false;
        KungFuPointsChecker[1] = false;
        KungFuPointsChecker[2] = false;
        KungFuPointsChecker[3] = false;
        KungFuPointsChecker[4] = false;

        KungFuEnemys[0] = null;
        KungFuEnemys[1] = null;
        KungFuEnemys[2] = null;
        KungFuEnemys[3] = null;
        KungFuEnemys[4] = null;

        rotarPuntos = UnityEngine.Random.Range(5,20);
        rotarTime = rotarPuntos+3f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Posicion de los puntos para el circulo de kung fu
        KungFuPoints[0].transform.position = new Vector3(this.transform.position.x - (kungFuRadio/2), this.transform.position.y, this.transform.position.z + kungFuRadio);
        KungFuPoints[1].transform.position = new Vector3(this.transform.position.x - kungFuRadio, this.transform.position.y, this.transform.position.z);
        KungFuPoints[2].transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - kungFuRadio);
        KungFuPoints[3].transform.position = new Vector3(this.transform.position.x + kungFuRadio, this.transform.position.y, this.transform.position.z);
        KungFuPoints[4].transform.position = new Vector3(this.transform.position.x + (kungFuRadio / 2), this.transform.position.y, this.transform.position.z + kungFuRadio);

        if (Input.GetMouseButtonDown(0))
        { 
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                //Debug.Log("Tocamos... " + hit.collider.name);
                //Mover el personaje a lo cliqueado
                motor.MoveToPoint(hit.point);

                RemoveFocus();//No focusear ningun objeto
 
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //Si el rayo toca
            if (Physics.Raycast(ray, out hit, 100))
            {
                Interactive interactable = hit.collider.GetComponent<Interactive>();
                if (interactable != null)
                {
                    SetFocus(interactable);

                }
            }
        }

        //Circulo de KungFu
        if (KungFuEnemys[0] != null || KungFuEnemys[1] != null || KungFuEnemys[2] != null || KungFuEnemys[3] != null || KungFuEnemys[4] != null)
        { 
            timer += Time.deltaTime;
            timerRotar += Time.deltaTime;
            if (timer > rotarPuntos)
            {
                Rotando = true;
                Debug.Log("Rotar circulo de KungFU");
                Rotacion();
                rotarPuntos = UnityEngine.Random.Range(5, 20);
                timer = 0;
                //Rotando = false;
            }
            if (timerRotar > rotarTime)
            {
                timerRotar = 0;
                rotarTime = rotarPuntos + 3f;
                Rotando = false;
            }
            else {
                Rotando = false;
            }
        }

    }

    void SetFocus (Interactive newFocus)
    {
        if(newFocus != focus)
        {
            if(focus != null)
            {
                focus.OnDeFocused();
            }
            focus = newFocus;
            motor.FollowTarget(newFocus);
        }

        newFocus.OnFocused(transform);

    }

    void RemoveFocus()
    {
        if (focus != null)
        {
            focus.OnDeFocused();
        }
        focus = null;
        motor.StopFollowingTarget();
    }


    public bool[] GetKungFuPoints()//Obtener el arreglo con los indices de los puntos
    {
        return  KungFuPointsChecker;
    }

    void Rotacion()
    {
        Transform Enemy;
        for (int i = 0; i<KungFuPointsChecker.Length; i++ )
        {
            if (KungFuPointsChecker[i] == true)
            {
                if(i < 4)
                {
                    KungFuPointsChecker[i] = false;
                    if(KungFuEnemys[i] != null)
                    {
                        Enemy = KungFuEnemys[i];
                        KungFuEnemys[i].gameObject.GetComponent<EnemyController>().indiceKungfuPoint = i + 1;
                        KungFuEnemys[i].GetComponent<EnemyController>().KungfuPoint = KungFuPoints[i + 1].transform;
                        KungFuEnemys[i].gameObject.GetComponent<EnemyController>().target = KungFuEnemys[i].gameObject.GetComponent<EnemyController>().KungfuPoint;
                        KungFuEnemys[i] = null;
                    }
                }
                else{

                    KungFuPointsChecker[i] = false;
                    if (KungFuEnemys[i] != null)
                    {
                        Enemy = KungFuEnemys[i];
                        KungFuEnemys[i].gameObject.GetComponent<EnemyController>().indiceKungfuPoint = 0;
                        KungFuEnemys[i].gameObject.GetComponent<EnemyController>().KungfuPoint = KungFuPoints[0].transform;
                        KungFuEnemys[i].gameObject.GetComponent<EnemyController>().target = KungFuEnemys[i].gameObject.GetComponent<EnemyController>().KungfuPoint;
                        KungFuEnemys[i] = null;
                    }

                }
            }
        }
    }


}