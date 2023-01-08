using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Para verlo en unity y poder crear la Accion:
[CreateAssetMenu(menuName = "IA/Acciones/Seguir Personaje")]
public class AccionSeguirPersonaje : IAAccion
{
    //Metodo principal:
    public override void Ejecutar(IAController controller)
    {
        SeguirPersonaje(controller);
    }
    //Para poder seguir al personaje, lo primero es verificar
    //si tenemos una referencia del personaje. Si la tenemos
    //sera obtener la direccion hacia el personaje, dirHaciaPersonaje, porque
    //esa sera la direccion hacia la cual debemos mover
    //al enemigo. Si tenemos la referencia tambien nos interesara
    //saber la distancia, distancia, a la cual esta el personaje, porque
    //querremos seguir al personaje hasta un punto donde no colisionemos
    //con el.
    private void SeguirPersonaje(IAController controller)
    {
        if(controller.PersonajeReferencia == null)
        {
            return;
        }

        Vector3 dirHaciaPersonaje = controller.PersonajeReferencia.position - controller.transform.position;
        Vector3 direccion = dirHaciaPersonaje.normalized;
        float distancia = dirHaciaPersonaje.magnitude;

        if(distancia >= 1.30f)
        {
            controller.transform.Translate(direccion * controller.VelocidadMovimiento * Time.deltaTime);
        }
    }
}
