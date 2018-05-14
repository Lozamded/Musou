using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class colorChanger : MonoBehaviour
{

    public Material[] material;
    public Material CurrMat;
    Renderer renderer;
    

    // Use this for initialization
    void Start()
    {

        renderer = GetComponent<Renderer>();
        renderer.enabled = true;
        renderer.sharedMaterial = CurrMat;

    }

    // Update is called once per frame
    void Update()
    {

    }

    //render blue orange Color
    public void BlackColor()
    {
        //Debug.Log("Hola, cambiar a azul a GFX");
        //Debug.Log("soy el objeto: " + this.name);
        CurrMat = material[1];
        renderer.sharedMaterial = CurrMat;
    }

    //render red color
    public void RedColor()
    {
        CurrMat = material[2];
        renderer.sharedMaterial = CurrMat;
    }

    //render greencolor
    public void GrayColor()
    {
        CurrMat = material[3];
        renderer.sharedMaterial = CurrMat;
    }

    //render greencolor
    public void GreenColor()
    {
        CurrMat = material[4];
        renderer.sharedMaterial = CurrMat;
    }


    //render yellow color
    public void YellowColor()
    {
        CurrMat = material[5];
        renderer.sharedMaterial = CurrMat;
    }
}