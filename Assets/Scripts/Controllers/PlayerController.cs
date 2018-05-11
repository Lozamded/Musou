using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : MonoBehaviour
{

    public Interactive focus;
    public LayerMask movementMask;
    public List<Transform> KungFuPoints = new List<Transform>(5); //Siempre necesito 5 puntos
    public float kungFuRadio = 4;

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

    public bool[] GetKungFuPoints()
    {
        return  KungFuPointsChecker;
    }
}