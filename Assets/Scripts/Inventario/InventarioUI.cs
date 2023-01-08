using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

//Para llamar el metodo DibujarItemEnInventario definido en esta clase InventarioUI, 
//en la clase Inventario, necesitamos que InventarioUI sea un Singleton.
public class InventarioUI : Singleton<InventarioUI>
{
    //Referencia del panel de descripcion dentro del inventario, con su imagen, texto 
    //del nombre y la propia descripcion:
    [Header("Panel Inventario Descripcion")]
    [SerializeField] private GameObject panelInventarioDescripcion;
    [SerializeField] private Image itemIcono;
    [SerializeField] private TextMeshProUGUI itemNombre;
    [SerializeField] private TextMeshProUGUI itemDescripcion;

    // Referencia del prefab del slot y del inventario donde lo instanciaremos:
    [SerializeField] private InventarioSlot slotPrefab;
    [SerializeField] private Transform contenedor;
    
    //Necesitamos una referencia del index que va a ser movido:
    public int IndexSlotInicialPorMover {get; private set;}

    //Necesitamos una referencia del slot que estamos seleccionando en tiempo actual:
    public InventarioSlot SlotSeleccionado {get; private set;}

    //Ponemos los slots en una lista, para poder guardar la informacion que los contiene, 
    //al crearlos, los indexaremos con la propiedad creaca en InventarioSlot "Index":
    List<InventarioSlot> slotsDisponibles = new List<InventarioSlot>();

    private void Start()
    {
        InicializarInventario();
        IndexSlotInicialPorMover = -1;
    }

    //Para actualizar el slot seleccionado y para mover index por el inventario:
    private void Update()
    {
        ActualizarSlotSeleccionado();
        if(Input.GetKeyDown(KeyCode.M))
        {
            if(SlotSeleccionado != null)
            {
                IndexSlotInicialPorMover = SlotSeleccionado.Index;
            }
        }

    }
    private void InicializarInventario()
    {
        for(int i = 0; i < Inventario.Instance.NumeroDeSlots; i++)
        {
            InventarioSlot nuevoSlot = Instantiate(slotPrefab, contenedor);
            nuevoSlot.Index = i;
            slotsDisponibles.Add(nuevoSlot);
        }
    }

    //Para actualizar el Slot Seleccionado, escribimos el siguiente metodo.
    //Si no hay ningun objecto seleccionado, regresamos (primer if).
    //Si si hay un objeto seleccionado, hay que verificar si tiene la clase
    //InventarioSlot. En slot estamos intentando de obtener la referencia
    //de <InventarioSlot> del objeto seleccionado.
    //Si el slot no es nulo, es decir, si hay la clase InventarioSlot
    //en el GameObject que estamos seleccionando (un slot),
    //entonces SlotSeleccionado = slot.
    private void ActualizarSlotSeleccionado()
    {
        GameObject goSeleccionado = EventSystem.current.currentSelectedGameObject;
        if(goSeleccionado == null)
        {
            return;
        }

        InventarioSlot slot = goSeleccionado.GetComponent<InventarioSlot>();
        if (slot != null)
        {
            SlotSeleccionado = slot;
        }
    }

    //Para dibujar/actualizar un item en un slot. Necesitamos una referencia del slot para
    //poder llamar a los dos metodos de InventarioSlot: ActualizarSlotUI y ActivarSlotUI.
    //Recordar que el index del slot y del item son los mismos.
    public void DibujarItemEnInventario(InventarioItem itemPorAñadir, int cantidad, int itemIndex)
    {
        InventarioSlot slot = slotsDisponibles[itemIndex];
        if(itemPorAñadir != null)
        {
            slot.ActivarSlotUI(true);
            slot.ActualizarSlotUI(itemPorAñadir, cantidad);
        }
        else
        {
            slot.ActivarSlotUI(false);
        }
    }

    //Metodo para actualizar cada componente de la descripcion del inventario.
    //Con el primer if nos aseguramos que en el index i de nuestro inventario tengamos algo.
    //Para ello hemos tenido que coger la referencia publica itemsInventario de la clase
    //Inventario.
    private void ActualizarInventarioDescripcion(int index)
    {
         if(Inventario.Instance.ItemsInventario[index] != null)
         {
            itemIcono.sprite = Inventario.Instance.ItemsInventario[index].Icono;
            itemNombre.text = Inventario.Instance.ItemsInventario[index].Nombre;
            itemDescripcion.text = Inventario.Instance.ItemsInventario[index].Descripcion;
            panelInventarioDescripcion.SetActive(true);
         }
         else
         {
            panelInventarioDescripcion.SetActive(false);   
         }
    }

    //Para llamar SlotUsarItem() de la clase InventarioSlot y lanzar el evento de Usar
    //el item que se encuentre en un slot. Fijarse que cuando le demos
    //a usar queremos que el slot se siga viendo como seleccionado.
    public void UsarItem()
    {
        if(SlotSeleccionado != null)
        {
            SlotSeleccionado.SlotUsarItem();
            SlotSeleccionado.SeleccionarSlot();
        }
    }

    //Para equipar un item:
    public void EquiparItem()
    {
        if(SlotSeleccionado != null)
        {
            SlotSeleccionado.SlotEquiparItem();
            SlotSeleccionado.SeleccionarSlot();
        }
    }

    //Para remover un item equipado:
    public void RemoverItem()
    {
        if(SlotSeleccionado != null)
        {
            SlotSeleccionado.SlotRemoverItem();
            SlotSeleccionado.SeleccionarSlot();
        }
    }

    #region Evento
        //Metodo de respuesta para el OnEnable y OnDisable:
        private void SlotInteraccionRespuesta(TipoDeInteraccion tipo, int index)
        {
            if(tipo ==TipoDeInteraccion.Click)
            {
                ActualizarInventarioDescripcion(index);
            }
        }

        //Para escuchar el metodo ClickSlot que lanza el evento de InventarioSlot,
        //para interactuar con los slots para hacer una accion:
        private void OnEnable()
        {
            InventarioSlot.EventoSlotInteraccion += SlotInteraccionRespuesta;
        }
        private void OnDisable()
        {
            InventarioSlot.EventoSlotInteraccion -= SlotInteraccionRespuesta;
        }

    #endregion

}
