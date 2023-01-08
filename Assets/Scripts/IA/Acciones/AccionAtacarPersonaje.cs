using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Para crear la accion en Unity:
[CreateAssetMenu(menuName = "IA/Acciones/Atacar Personaje")]
public class AccionAtacarPersonaje : IAAccion
{
    public override void Ejecutar(IAController controller)
    {
        Atacar(controller);
    }

    //Para atacar, antes debemos verificar si tenemos una 
    //referencia del personaje y si es tiempo para atacar.
    //Tambien deberemos verificar si estamos
    //dentro del rango de ataque hacia el personaje. Si lo 
    //estamos llamamos a la logica de atacar y actualizamos
    //el tiempo para el siguiente ataque con el 
    //metodo de ActualizarTiempoEntreAtaques():
    private void Atacar(IAController controller)
    {
        if(controller.PersonajeReferencia == null)
        {
            return;
        }

        if(controller.EsTiempoDeAtacar() == false)
        {
            return;
        }

        if (controller.PersonajeEnRangoDeAtaque(controller.RangoDeAtaqueDeterminado))
        {
            //Ataque:
            if(controller.TiposDeAtaque == TiposDeAtaque.Placaje)
            {
                controller.AtaquePlacaje(controller.Daño);
            } else
            {
                controller.AtaqueMelee(controller.Daño);
            }

            //Actualizar el tiempo entre ataques:
            controller.ActualizarTiempoEntreAtaques();
        }

    }
}
