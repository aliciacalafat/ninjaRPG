using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Para verlo en unity y poder crear la Accion:
[CreateAssetMenu(menuName = "IA/Acciones/Activar Camino Movimiento")]
public class AccionActivarCaminoMovimiento : IAAccion
{
    //Para activar la clase de EnemigoMovimiento. Primero
    //verificamos si la tenemos, si no regresamos.
    //Si la tenemos, la activamos.
    public override void Ejecutar(IAController controller)
    {
        if(controller.EnemigoMovimiento == null)
        {
            return;
        }
        controller.EnemigoMovimiento.enabled = true;
    }

}
