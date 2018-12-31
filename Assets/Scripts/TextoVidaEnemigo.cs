using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextoVidaEnemigo : MonoBehaviour
{
    public float time = 1.25f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resetext()
    {
        reseText(time);
        Debug.Log("Borrar texto");
        this.gameObject.GetComponent<TextMeshProUGUI>().text = "";
    }

    public IEnumerator reseText(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("texto borrado");   
        this.gameObject.GetComponent<TextMeshProUGUI>().text = "";

    }
}
