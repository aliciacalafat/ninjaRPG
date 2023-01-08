using UnityEngine;
using System;

//Como esta clase hereda de InventarioItem, ItemPocionVida tambien
//sera un ScriptableObject.
//Para poder crear el ScriptableObject en nuestras carpetas, este
//en concreto dentro de una subcarpeta:
[CreateAssetMenu(menuName = "Items/Pocion Vida")]
public class ItemPocionVida : InventarioItem
{
    [Header("Pocion info")]
    public float HPRestauracion;

//Metodo sobreescrito desde InventarioItem, con el que podemos usar el item 
//PocionVida para restaurar vida.
    public override bool UsarItem()
    {
        if(Inventario.Instance.Personaje.PersonajeVida.PuedeSerCurado)
        {
            Inventario.Instance.Personaje.PersonajeVida.RestaurarSalud(HPRestauracion);
            return true;
        }

        return false;
    }
}
