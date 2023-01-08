using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPorAgregar : MonoBehaviour
{
    //Referencia del InventarioItem y la cantidad que vamos a anadir:
    [Header("Config")]
    [SerializeField] private InventarioItem inventarioItemReferencia;
    [SerializeField] private int cantidadPorAgregar;

    //Metodo para que al colisionar el Player con los items, añadirlos
    //al inventario y luego destruirlos del suelo.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Inventario.Instance.AñadirItem(inventarioItemReferencia, cantidadPorAgregar);
            Destroy(gameObject);
        }
    }

}
