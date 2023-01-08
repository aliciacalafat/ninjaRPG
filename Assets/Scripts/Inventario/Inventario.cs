using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventario : Singleton<Inventario>
{
    //Creamos un array de los items disponibles en el inventario en la clase
    //InventarioItem, para tener una forma de visualizar como estamos trabajando
    // con el inventario.
    [Header("Items")]
    [SerializeField] private InventarioItem[] itemsInventario;
    //Referencia del personaje para poderle aplicar las diferentes acciones que se pueden hacer
    //con los items.
    [SerializeField] private Personaje personaje;
    // Referenciamos una variable que definira el numero de slots del inventario:
    [SerializeField] private int numeroDeSlots;

    //Como algunas de las referencias anteriores las necesitamos publicas para
    //utilizarlas en otras clases, creamos las siguientes propiedades:
    public int NumeroDeSlots => numeroDeSlots;
    public InventarioItem[] ItemsInventario => itemsInventario;
    public Personaje Personaje => personaje;

    private void Start()
    {
        itemsInventario = new InventarioItem[numeroDeSlots];
    }

    //La logica de anadir un item al inventario sera la siguiente: Tenemos
    //dos formas de anadir los items.
    //-Primera: Si podemos acumular el item, ya tenemos un item igual
    //en nuestro inventario y aun no ha llegado al maximo de 
    //acumular por slot, entonces lo acumularemos en el primer 
    //slot donde aun quepa.
    //-Segunda: Si no tenemos el item en el inventario, pero tenemos
    //otros items diferentes, el nuevo debera ocupar el primer
    //slot disponible despues de esos que ya hay.
    //Entonces, para AnadirItem debemos pasar como parametros la 
    //referencia del item en itemPorAñadir y su cantidad en cantidad.
    //Lo primero que haremos dentro del metodo, en el if,
    //sera verificar que el item que vamos a anadir no es nulo.
    //Despues viene la primera parte del metodo que sera verificar en el
    //caso en que tengamos ya un item en el inventario,
    //para poder ir acumulandolo. Si no lo tenemos
    //en el inventario, lo que haremos sera anadir este item con su cantidad
    //en un nuevo slot que se encuentre vacio.
    //Para obtener la referencia de los items similares que se encuentran
    //ya en el inventario, tenemos el metodo VerificarExistencias.
    //En la lista despues del if, estamos guardando todos los indices
    //de los items que son similares al que estamos tratando de anadir.
    //En el segundo if, si el item es acumulable, lo hacemos. Para ello
    //hacemos un doble ckeck de si los indexes son mayores a 0, podemos
    //continuar. Dentro del for que recorre los indexes totales que
    //tenemos, en el primer if si la cantidad de items con indice i del 
    //inventario no ha superado la acumulacion maxima, hay que anadir la
    //nueva cantidad que hemos recogido. En el segundo if dentro de este,
    //le decimos que si hemos superado la acumulacion maxima, queremos saber
    //la diferencia, es decir lo restante que no deberia estar en ese
    //slot, para asi poder anadir el item en otro slot. Despues le decimos
    //que su cantidad no supere la acumulacion maxima y luego por
    //recurrencia añadimos el mismo item con la diferencia.
    //Para acabar la primera parte, actualizamos el inventario y regresamos.
    //En la segunda parte del metodo pondremos el caso en el que el item
    //que tratamos de anadir, no tiene un item similar en el inventario, es decir,
    //tendriamos que anadirlo en el primer slot disponible y vacio que tengamos.
    //Para ello necesitamos el metodo AñadirItemEnSlotDisponible, que nos permite
    //anadir un item de manera nueva a un slot.
    //En esta segunda parte, primero comprobaremos si la cantidad es mayor que cero,
    //si no no tiene sentido.
    //Depues verificaremos si la cantidad que vamos a anadir supera la acumulacion
    //maxima del item, entonces AñadirItemEnSlotDisponible, es decir en un slot
    //vacio hay que poner su acumulacion maxima de 50 unidades.
    //Desspués hay que actualizar la cantidad que queda despues de
    //anadir en un slot vacío y con esto utilizando recurrencia de nuevo
    //anadimos el item que queremos anadir por la cantidad restante.
    //Por ultimo en el else, nos preguntamos que que pasa si la cantidad
    //que estamos tratando de anadir no es mayor a la acumulacion maxima del item,
    //simplemente hay que anadir ese item en un slot vacio.
    public void AñadirItem(InventarioItem itemPorAñadir, int cantidad)
    {
        if(itemPorAñadir == null)
        {
            return;
        }
        //Verificacion en caso de tener ya un item en el inventario.
        List<int> indexes = VerificarExistencias(itemPorAñadir.ID);
        
        if(itemPorAñadir.EsAcumulable)
        {
            if (indexes.Count > 0)
            {
                for(int i =0; i < indexes.Count; i++)
                {
                    if(itemsInventario[indexes[i]].Cantidad < itemPorAñadir.AcumulacionMax)
                    {
                        itemsInventario[indexes[i]].Cantidad += cantidad;
                        if(itemsInventario[indexes[i]].Cantidad > itemPorAñadir.AcumulacionMax)
                        {
                            int diferencia = itemsInventario[indexes[i]].Cantidad - itemPorAñadir.AcumulacionMax;
                            itemsInventario[indexes[i]].Cantidad = itemPorAñadir.AcumulacionMax;
                            AñadirItem(itemPorAñadir, diferencia);
                        }

                        InventarioUI.Instance.DibujarItemEnInventario(itemPorAñadir, itemsInventario[indexes[i]].Cantidad, indexes[i]);
                    }
                }
            }
        }
        //Segunda parte del metodo.
        if (cantidad <= 0)
        {
            return;
        }

        if(cantidad > itemPorAñadir.AcumulacionMax)
        {
            AñadirItemEnSlotDisponible(itemPorAñadir, itemPorAñadir.AcumulacionMax);
            cantidad -= itemPorAñadir.AcumulacionMax;
            AñadirItem(itemPorAñadir, cantidad);
        }
        else
        {
            AñadirItemEnSlotDisponible(itemPorAñadir, cantidad);
        }
    }

    //Para encontrar el itemPorAnadir dentro del inventario hay que anadir 
    //como parametro un tipo string itemID. Tener en cuenta que en 
    //numeroDeSlots habiamos definido un index para cada slot que en total
    //son 24, esos index seran tambien los de los items.
    //Notar que tenemos 24 slots, 24 indices para los items que van
    //desde el 0 hasta el 23.
    //Con el for recorremos todo el inventario para buscar si tenemos un
    //item con ese ID.
    //Con el segundo if nos aseguramos si el item encontrado tiene el mismo ID
    //que alguno que tengamos en el inventario, en caso afirmativo,
    //lo anadimos.
    //Por ultimo regresamos la lista de indexs
    private List<int> VerificarExistencias(string itemID)
    {
        List<int> indexesDelItem = new List<int>();
        for (int i =0; i < itemsInventario.Length; i++)
        {
            if(itemsInventario[i] != null)
            {
                if(itemsInventario[i].ID == itemID)
                {
                    indexesDelItem.Add(i);
                }
            }
        }
        return indexesDelItem;
    }

    //Para la segunda parte del metodo AñadirItem, necesitamos este otro metodo,
    //con la referencia del item que estamos tratando de anadir y su cantidad
    //como parametros, para anadir un item en un slot vacio.
    //Primero haremos un for para ir por todo nuestro inventario y luego
    //verificaremos con el if que si encontramos en el inventario un slot que se
    //encuentre vacio, hay que anadirlo alli, en una nueva instancia del item 
    //(por eso lo de CopiarItem, definido en InventarioItem). 
    //Por ultimo actualizamos el inventario.
    //El return es necesario para que no añada el item en todos los slots disponibles.
    private void AñadirItemEnSlotDisponible(InventarioItem item, int cantidad)
    {
        for(int i = 0; i < itemsInventario.Length; i++)
        {
            if(itemsInventario[i] == null)
            {
                itemsInventario[i] = item.CopiarItem();
                itemsInventario[i].Cantidad = cantidad;
                InventarioUI.Instance.DibujarItemEnInventario(item, cantidad, i);
                return;
            }
        }
    }

    //Para eliminar un item una vez utilizado en un slot. Para ello, con el Cantidad--, 
    //reducimos la cantidad en uno. Lo siguiente sera verificar si la cantidad no esta
    //por debajo de cero, en el caso que lo sea el item no existe y actulizamos el 
    //slot como nulo. En caso contrario, rellenamos el slot actualizandolo.
    private void EliminarItem(int index)
    {
        ItemsInventario[index].Cantidad--;
        if(itemsInventario[index].Cantidad <= 0)
        {
            itemsInventario[index].Cantidad = 0;
            itemsInventario[index] = null;
            InventarioUI.Instance.DibujarItemEnInventario(null, 0, index);
        }
        else
        {
            InventarioUI.Instance.DibujarItemEnInventario(itemsInventario[index], itemsInventario[index].Cantidad, index);
        }
    }

    //Para mover items por el inventario. Para ello deberemos verificar si en el slot inicial
    //donde moveremos el item no es nulo, es decir que tenemos un item por mover y tambien
    //debemos verificar que el slot al que se movera el item esta ocupado.
    //Despues copiaremos el item en el slot final y a continuacion lo borraremos del inicial.
    public void MoverItem(int indexInicial, int indexFinal)
    {
        if(itemsInventario[indexInicial] == null || itemsInventario[indexFinal] != null)
        {
            return;
        }

        //Copiar Item en Slot final.
        InventarioItem itemPorMover = itemsInventario[indexInicial].CopiarItem();
        itemsInventario[indexFinal] = itemPorMover;
        InventarioUI.Instance.DibujarItemEnInventario(itemPorMover, itemPorMover.Cantidad, indexFinal);

        //Borrar Item del Slot inicial.
        itemsInventario[indexInicial] = null;
        InventarioUI.Instance.DibujarItemEnInventario(null, 0, indexInicial);        
    }

    //Para usar un item. Primero comprobamos si esta vacio o no el slot, si lo esta pues nada, regresamos.
    //Si no lo esta, utilizamos UsarItem(), y despues vamos a tener que borrar una cantidad
    //item en ese slot. Para ello utilizaremos el metodo :
    private void UsarItem(int index)
    {
        if(itemsInventario[index] == null)
        {
            return;
        }

        if(itemsInventario[index].UsarItem())
        {
            EliminarItem(index);
        }

    }

    //Para equipar un item:
    private void EquiparItem(int index)
    {
        if(itemsInventario[index] == null)
        {
            return;
        }

        if(itemsInventario[index].Tipo != TiposDeItem.Armas)
        {
            return;
        }

        itemsInventario[index].EquiparItem();
    }

    //Para remover un item equipado:
    private void RemoverItem(int index)
    {
        if(itemsInventario[index] == null)
        {
            return;
        }

        if(itemsInventario[index].Tipo != TiposDeItem.Armas)
        {
            return;
        }

        itemsInventario[index].RemoverItem();
    }

    #region Eventos

    //Respuesta de la escucha del usar item:
    private void SlotInteraccionRespuesta(TipoDeInteraccion tipo, int index)
    {
        switch(tipo)
        {
            case TipoDeInteraccion.Usar:
                UsarItem(index);
                break;
            case TipoDeInteraccion.Equipar:
                EquiparItem(index);
                break;
            case TipoDeInteraccion.Remover:
                RemoverItem(index);
                break;
        }
    }

    //Para escuchar al evento de usar item:
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
