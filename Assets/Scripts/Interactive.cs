using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour {

    public float radius = 12f;
    public Transform interactionTransform;

    bool isFocus = false;
    Transform player;

    bool hasInteracted = false;

    private void Awake()
    {
        interactionTransform = this.gameObject.transform;
    }

    public virtual void Interact()//Combate
    {
        //Esto significa que este metodo sera reescrito
        Debug.Log("Interactuar con " + interactionTransform.name);
        if (interactionTransform.GetComponent<EnemyController>().estado == "combate")
        {
            interactionTransform.GetComponent<EnemyStats>().vida -= player.gameObject.GetComponent<PlayerController>().ataque; 
            Debug.Log("Vida = " + interactionTransform.GetComponent<EnemyStats>().vida);
            player.gameObject.GetComponent<PlayerController>().VidaEnemigo.text = interactionTransform.GetComponent<EnemyStats>().getVida().ToString();
            player.gameObject.GetComponent<PlayerController>().Dano.text = "-" + player.gameObject.GetComponent<PlayerController>().ataque.ToString();
            player.gameObject.GetComponent<PlayerController>().Dano.GetComponent<TextoDano>().move();
        }
    }

    void Update()
    {
        if(isFocus && !hasInteracted)
        {
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            if (distance <= radius)
            {
                Interact();
                hasInteracted = true;
            }
        }
    }

    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    public void OnDeFocused()
    {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

}
