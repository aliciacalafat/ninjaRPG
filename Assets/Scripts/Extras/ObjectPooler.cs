using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Esta clase es un pool para el canvas, el texto que indica el daño al enemigo.
public class ObjectPooler : MonoBehaviour
{
    //Variable que indica la cantidad de instancias de un objeto que 
    //queremos crear en el pooler.
    [SerializeField] private int cantidadPorCrear;

    //Lista donde guardamos todas las instancias que vamos creando.
    private List<GameObject> lista;

    //GameObject en nuestra jerarquia que contendra las instancias creadas,
    //queremos que sea una propiedad para que lo herede la clase PersonajeFX.
    public GameObject ListaContenedor {get; private set;}

    //Para crear el pooler:
    public void CrearPooler(GameObject objetoPorCrear)
    {
        lista = new List<GameObject>();
        ListaContenedor = new GameObject($"Pool - {objetoPorCrear.name}");
        for(int i = 0; i < cantidadPorCrear; i++)
        {
            lista.Add(AñadirInstancia(objetoPorCrear));
        }
    }

    //Para crear las instancias, instanciamos el objetoPorCrear en el contenedor
    //dentro de la jerarquia y lo guardamos en un nuevo Objeto.
    //Cada instancia creada dentro de pool debe estar desactivada:
    private GameObject AñadirInstancia(GameObject objetoPorCrear)
    {
        GameObject nuevoObjeto = Instantiate(objetoPorCrear, ListaContenedor.transform);
        nuevoObjeto.SetActive(false);
        return nuevoObjeto;
    }

    //Para obtener una instancia del pooler que no esta siendo utilizada:
    public GameObject ObtenerInstancia()
    {
        for (int i = 0; i < lista.Count; i++)
        {
            if (lista[i].activeSelf == false)
            {
                return lista[i];
            }
        }
        return null;
    }

    //Para destruir el pooler y limpiar la lista una vez que queramos
    //remover el arma y los proyectiles:
    public void DestruirPooler()
    {
        Destroy(ListaContenedor);
        lista.Clear();
    }
}
