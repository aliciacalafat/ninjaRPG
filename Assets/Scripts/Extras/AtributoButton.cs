using System;
using UnityEngine;

//Con este boton notificaremos que queremos modificar uno de los 
//tipos de atributos, para ello nos ayudaremos de un evento.
//Antes, debemos decirle que tipo de tributo estamos clickeando:
public enum TipoAtributo
{
    Fuerza,
    Inteligencia,
    Destreza
}
public class AtributoButton : MonoBehaviour
{
    //Para lanzar el evento, le tenemos que decir que tipo de 
    //atributo estamos intentando aumentar, para ello le ponemos
    //<TipoAtributo>:
    public static Action<TipoAtributo> EventoAgregarAtributo;

    //Para manejar el evento creamos en el inspector:
    [SerializeField] private TipoAtributo tipo;

    //En el inspector podremos especificar que tipo de atributo se
    //relaciona con el buton:
    public void AgregarAtributo()
    {
        EventoAgregarAtributo?.Invoke(tipo);
    }
}
