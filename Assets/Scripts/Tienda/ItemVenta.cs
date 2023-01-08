using System;
using System.Collections.Generic;
using UnityEngine;

//Para que se pueda ver esta clase en el inspector:
[Serializable]
public class ItemVenta
{
    //Referencias sobre el item de la tienda:
    public string Nombre;
    public InventarioItem Item;
    public int Coste;
}
