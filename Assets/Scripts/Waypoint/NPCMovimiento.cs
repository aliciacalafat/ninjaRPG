using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovimiento : WaypointMovimiento
{
    //Definimos la variable DireccionMovimiento para poder
    //elegir cual usar y la variable velocidad.
    [SerializeField] private DireccionMovimiento direccion;

    //Para obtener el hash del parametro que hemos creado dentro de la ventana Animator:
    private readonly int caminarAbajo = Animator.StringToHash("CaminarAbajo");
    private readonly int caminarDer = Animator.StringToHash("CaminarDer");

    //Sobreescribimos el metodo rotar personaje de la clase WaypointMovimiento:
    protected override void RotarPersonaje()
    {
        if(direccion != DireccionMovimiento.Horizontal)
        {
            return;
        }

        if(PuntoPorMoverse.x > ultimaPosicion.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);  
        }
    }

    //Para saber como vamos a actualizar las animaciones verticales del NPC.
    //Solo actualizamos las animaciones si el NPC se mueve de forma vertical.
    //Antes debemos saber si el NPC se mueve hacia arriba o hacia abajo.
    protected override void RotarVertical()
    {
        if(direccion != DireccionMovimiento.Vertical)
        {
            return;
        }   

        if(PuntoPorMoverse.y > ultimaPosicion.y)
        {
            _animator.SetBool(caminarAbajo, false);
 //           _animator.SetBool(caminarDer, true);
        }
        else
        {
            _animator.SetBool(caminarAbajo, true);
 //           _animator.SetBool(caminarDer, false);
        }
    }
}
