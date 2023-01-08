using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Para crear un estado desde Unity:
[CreateAssetMenu(menuName = "IA/Estado")]
public class IAEstado : ScriptableObject
{
    //Un estado tiene acciones y decisiones.
    //Para obtener todas las acciones en el estado
    //creamos el array de Acciones, para las decisiones
    //definiremos su logica con la clase de transicion
    //porque alli ya tenemos la decision.
    public IAAccion[] Acciones;
    public IATransicion[] Transiciones;

    //Para llamar el metodo de realizar transiciones y
    //el metodo de ejecutar acciones para poder correr un estado:
    public void EjecutarEstado(IAController controller)
    {
        EjecutarAcciones(controller);
        RealizarTransiciones(controller);
    }

    //Para ejecutar todas las acciones del estado:
    private void EjecutarAcciones(IAController controller)
    {
        if(Acciones == null || Acciones.Length <= 0)
        {
            return;
        }
        for(int i = 0; i < Acciones.Length; i++)
        {
            Acciones[i].Ejecutar(controller);
        }
    }

    //Para ejecutar todas las transiciones (decisiones) del estado,
    //verificamos que tenemos transicion y obtenemos la transicion
    //recorriendolas con un ciclo for. Luego verificamos si
    //decisionValor es true entonces vamos al nuevo estado.
    private void RealizarTransiciones(IAController controller)
    {
        if(Transiciones == null || Transiciones.Length <= 0)
        {
            return;
        }
        for(int i = 0; i < Transiciones.Length; i++)
        {
            bool decisionValor = Transiciones[i].Decision.Decidir(controller);
            if(decisionValor)
            {
                controller.CambiarEstado(Transiciones[i].EstadoVerdadero);
            }
            else
            {
                controller.CambiarEstado(Transiciones[i].EstadoFalso);
            }
        }
    }
}
