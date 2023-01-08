using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Para verlo en unity y poder crear la Decision:
[CreateAssetMenu(menuName = "IA/Decisiones/Detectar Personaje")]
public class DecisionDetectarPersonaje : IADecision
{
    //Metodo principal:
    public override bool Decidir(IAController controller)
    {
        return DetectarPersonaje(controller);
    } 
    //Para detectar al personaje y poder atacarlo, se utilizan 
    //las fisicas 2D de Unity que permite crear una colision
    //en circulo para detectarlo con radio el rangoDeteccion
    //y centro en la posicion del enemigo.
    //Si detectamos al personaje, se guardara en la variable
    //personajeDetectado y regresamos true.
    //Para evitar problemas una vez que sale fuera del rango de
    //deteccion, debe olvidar la referencia
    private bool DetectarPersonaje(IAController controller)
    {
        Collider2D personajeDetectado = Physics2D.OverlapCircle(controller.transform.position, 
            controller.RangoDeteccion, controller.PersonajeLayerMask);

        if(personajeDetectado != null)
        {
            controller.PersonajeReferencia = personajeDetectado.transform;
            return true;
        }
        controller.PersonajeReferencia = null;
        return false;
    }
}
