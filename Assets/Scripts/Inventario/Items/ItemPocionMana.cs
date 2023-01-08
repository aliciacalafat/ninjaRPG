using UnityEngine;

//Como esta clase hereda de InventarioItem, ItemPocionVida tambien
//sera un ScriptableObject.
//Para poder crear el ScriptableObject en nuestras carpetas, este
//en concreto dentro de una subcarpeta:
[CreateAssetMenu(menuName = "Items/Pocion Mana")]
public class ItemPocionMana : InventarioItem
{
    [Header("Pocion info")]
    public float MPRestauracion;

    //Metodo sobreescrito desde InventarioItem, con el que podemos usar el item 
    //PocionMana para restaurar mana.
    public override bool UsarItem()
    {
        if(Inventario.Instance.Personaje.PersonajeMana.SePuedeRestaurar)
        {
            Inventario.Instance.Personaje.PersonajeMana.RestaurarMana(MPRestauracion);
            return true;
        }

        return false;
    }
}
