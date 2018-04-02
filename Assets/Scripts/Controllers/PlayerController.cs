using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : MonoBehaviour
{

    public Interactive focus;
    public LayerMask movementMask; 

    Camera cam;
    PlayerMotor motor;

	// Use this for initialization
	void Start ()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
	}
	
	// Update is called once per frame
	void Update ()
    {
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
}