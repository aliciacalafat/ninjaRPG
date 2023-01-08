using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemTienda : MonoBehaviour
{
    //Referencia sobre el item de la tienda:
    [Header("Config")]
    [SerializeField] private Image itemIcono;
    [SerializeField] private TextMeshProUGUI itemNombre;
    [SerializeField] private TextMeshProUGUI itemCoste;
    [SerializeField] private TextMeshProUGUI cantidadPorComprar;

    //Para guardar propiedad del item que esta siendo cargado
    //en la tarjeta:
    public ItemVenta ItemCargado {get; private set;}

    //Para controlar el item que estamos intentando comprar y su coste:
    private int cantidad;
    private int costeInicial;
    private int costeActual;

    //Para actualizar la cantidad que estamos intentando comprar,
    //la cantidad y el coste con coste actual del item.
    private void Update()
    {
        cantidadPorComprar.text = cantidad.ToString();
        itemCoste.text = costeActual.ToString();
    }

    //Para actualizar la tarjeta de cada item con su info:
    public void ConfigurarItemVenta(ItemVenta itemVenta)
    {
        ItemCargado = itemVenta;
        ItemCargado = itemVenta;
        itemIcono.sprite = itemVenta.Item.Icono;
        itemNombre.text = itemVenta.Item.Nombre;
        itemCoste.text = itemVenta.Coste.ToString();
        cantidad = 1;
        costeInicial = itemVenta.Coste;
        costeActual = itemVenta.Coste;
    }

    //Para comprar item:
    public void ComprarItem()
    {
        if (MonedasManager.Instance.MonedasTotales >= costeActual)
        {
            Inventario.Instance.AñadirItem(ItemCargado.Item, cantidad);
            MonedasManager.Instance.EliminarMonedas(costeActual);
            cantidad = 1;
            costeActual = costeInicial;
        }
    }

    //Para sumar la cantidad que nos queramos llevar:
    public void SumarItemPorComprar()
    {
        int costeDeCompra = costeInicial * (cantidad + 1);
        if(MonedasManager.Instance.MonedasTotales >= costeDeCompra)
        {
            cantidad++;
            costeActual = costeInicial * cantidad;
        }
    }

    //Para restar la cantidad que nos queramos llevar:
    public void RestarItemPorComprar()
    {
        if(cantidad == 1)
        {
            return;
        }
        cantidad--;
        costeActual = costeInicial * cantidad;
    }
}
