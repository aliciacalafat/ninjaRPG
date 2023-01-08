using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Para mostra o una seleccion roja o blanca, debemos 
//especificar que tipo de deteccion se acaba de realizar:
public enum TipoDeteccion
{
    Rango,
    Melee
}
public class EnemigoInteraccion : MonoBehaviour
{
    //Para obtener la referencia del circulo blanco para
    //seleccionar enemigos con armas tipo magia:
    [SerializeField] private GameObject seleccionRangoFX;
    //Para obtener la referencia del circulo rojo para
    //seleccionar enemigos con armas tipo melee:
    [SerializeField] private GameObject seleccionMeleeFX;

    //Para mostrar o no el enemigo seleccionado:
    public void MostrarEnemigoSeleccionado(bool estado, TipoDeteccion tipo)
    {   
        if(tipo == TipoDeteccion.Rango)
        {
            seleccionRangoFX.SetActive(estado);
        }
        else
        {
            seleccionMeleeFX.SetActive(estado);
        }
    }

    //Para desactivar los sprites de seleccion del enemigo una vez
    //ha sido derrotado:
    public void DesactivarSpritesSeleccion()
    {
        seleccionMeleeFX.SetActive(false);
        seleccionRangoFX.SetActive(false);
    }
}
