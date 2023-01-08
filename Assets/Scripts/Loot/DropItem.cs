using System;
using System.Collections.Generic;
using UnityEngine;

//Clase que define que item es soltado por el enemigo en el loot.
//Para ponerlo en el inspector:
[Serializable]
public class DropItem 
{
    //Referencia del nombre, cantidad e item que se dropea,
    //que suelta el enemigo.
    [Header("Info")]
    public string Nombre;
    public InventarioItem Item;
    public int Cantidad;
    //Porcentaje de dropeo que tiene un item, una probabilidad de 
    //que enemigo suelte el item.
    [Header("Drop")]
    [Range(0, 100)]public float PorcentajeDrop;

    //Propiedad de si el item dropeado fue recogido:
    public bool ItemRecogido { get; set;}
}
