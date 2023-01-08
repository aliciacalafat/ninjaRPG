using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Para que un arma sea un item debe heredar de InventarioItem.
//Para manejarlo en unity hacemos lo de createAssetMenu.
[CreateAssetMenu(menuName = "Items/Arma")]
public class ItemArma : InventarioItem
{
    //Para obtener una referencia del arma:
    [Header("Arma")]
    public Arma Arma;

    //Para escribir la logica de equipar un arma, debemos sobreescribir
    //el metodo EquiparItem de la clase InventarioItem.
    public override bool EquiparItem()
    {
        if(ContenedorArma.Instance.ArmaEquipada != null)
        {
            return false;
        }
        ContenedorArma.Instance.EquiparArma(this);
        return true;
    }

    //Para escribir la logica de remover un arma equipada, debemos sobreescribir
    //el metodo RemoverItem de la clase InventarioItem.
    public override bool RemoverItem()
    {
        if(ContenedorArma.Instance.ArmaEquipada == null)
        {
            return false;
        }
        ContenedorArma.Instance.RemoverArma();
        return true;
    }
}
