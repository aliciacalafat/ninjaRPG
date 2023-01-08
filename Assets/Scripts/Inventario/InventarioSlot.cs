using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

//Tipos de interacciones que tendremos para cada slot:
public enum TipoDeInteraccion
{
    Click, 
    Usar,
    Equipar,
    Remover
}

public class InventarioSlot : MonoBehaviour
{
    //Para ver la descripcion de los items de cada slot, necesitamos 
    //lanzar un evento (Action) para notificar que hemos hecho click en un slot
    //y poder actualizar el panel de descripcion.
    //Al evento le pasaremos el tipo de iteraccion que queremos hacer,
    //asi como una referencia al index del slot con el que
    //queremos interaccionar, que sera un int.
    public static Action<TipoDeInteraccion, int> EventoSlotInteraccion;

    //Propiedad para almacenar el index de cada slot, es decir,
    //queremos poner una "etiqueta" en cada slot. Esto nos facilita
    //la vida para obtener referencias de un slot en particular
    //para cuando queramos hacer algo en especifico, como por ejemplo
    //para poder usar el item de un slot. Para inicializar
    //esta propiedad, metemos la indexacion en la logica de InventarioUI
    public int Index {get; set;}

    //Para actualizar los iconos de los items en los slots, debemos primero
    //agregar las referencias del icono, del fondo que contiene la cantidad
    //del item y del texto que contiene la cantidad:
    [SerializeField] private Image itemIcono;
    [SerializeField] private GameObject fondoCantidad;
    [SerializeField] private TextMeshProUGUI cantidadTMP;

    //Referencia del boton. La queremos utilizar para mejorar la
    //seleccion de slots en el inventario. En conreto, para que cuando
    //le demos a un slot y a una de las 3 instrucciones posibles,
    //el slot se siga viendo como seleccionado.
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    //Para actualizar el item y la cantidad en los slots:
    public void ActualizarSlotUI(InventarioItem item, int cantidad)
    {
        itemIcono.sprite = item.Icono;
        cantidadTMP.text = cantidad.ToString();
    }

    //Para activar o desactivar el icono y el fondo que contiene el texto de los slots:
    public void ActivarSlotUI(bool estado)
    {
        itemIcono.gameObject.SetActive(estado);
        fondoCantidad.SetActive(estado);
    }

    //Para que al apretar uno de los tres botones del inventario, se siga seleccionando el slot:
    public void SeleccionarSlot()
    {
        _button.Select();
    }

    //Para lanzar el evento de los tipos de interaccion que se pueden hacer
    //con los slots, creamos el siguiente metodo. Este metodo, se escuchara
    //en la clase InventarioUI.
    //Debemos verificar si podemos mover items.
    public void ClickSlot()
    {
        EventoSlotInteraccion?.Invoke(TipoDeInteraccion.Click, Index);

        //MoverItem
        if(InventarioUI.Instance.IndexSlotInicialPorMover != -1)
        {
            if(InventarioUI.Instance.IndexSlotInicialPorMover != Index)
            {
                //Mover
                Inventario.Instance.MoverItem(InventarioUI.Instance.IndexSlotInicialPorMover, Index);
            }
        }
    }

    //Para notificar que queremos usar el item que se encuentra en un slot.
    //Para ello verificamos primero si tenemos un item en el slot para 
    //lanzar el evento (el if). Lo llamaremos en la clase InventarioUI.
    public void SlotUsarItem()
    {
        if(Inventario.Instance.ItemsInventario[Index] != null)
        {
            EventoSlotInteraccion?.Invoke(TipoDeInteraccion.Usar, Index);
        }
    }

    //Para equipar un item:
    public void SlotEquiparItem()
    {
        if(Inventario.Instance.ItemsInventario[Index] != null)
        {
            EventoSlotInteraccion?.Invoke(TipoDeInteraccion.Equipar, Index);
        }
    }

    //Para remover un item equipado:
    public void SlotRemoverItem()
    {
        if(Inventario.Instance.ItemsInventario[Index] != null)
        {
            EventoSlotInteraccion?.Invoke(TipoDeInteraccion.Remover, Index);
        }
    }

}
