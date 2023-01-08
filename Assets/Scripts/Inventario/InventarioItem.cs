using UnityEngine;

//En esta clase definiremos todo lo relacionado con un item, nombre,
//identificador, icono del item, descripcion...
//Lo primero es crear una enumeracion donde tendremos todos los items:
public enum TiposDeItem
{
    Armas,
    Pociones,
    Pergaminos,
    Ingredientes,
    Tesoros
}

//Definimos los parametros e informacion del item.
//[TextArea] permite tener un area mas grande donde se le
//puede anadir mas descripcion.
//La ID es un parametro unico de cada item que utilizaremos
//para obtener una referencia del item en el codigo.
//En informacion, vemos que tenemos el parametro EsConsumible,
//alli iran los items que podemos usar (como las pociones),
//notar que un arma no se usa, se equipa. Tambien vemos
//el parametro EsAcumulable, seran aquellos items que podemos
//acumular en un mismo slot (los veremos acumulados con un numero).
//[HideInInspector] es para ocultar variables en el inspector, la
//Cantidad sera para saber en cada item cuanta cantidad del item
//le queda.
public class InventarioItem : ScriptableObject
{
    [Header("Parametros")]
    public string ID;
    public string Nombre;
    public Sprite Icono;
    [TextArea] public string Descripcion;

    [Header("Informacion")]
    public TiposDeItem Tipo;
    public bool EsConsumible;
    public bool EsAcumulable;
    public int AcumulacionMax;

    [HideInInspector]public int Cantidad;

    //Para que una vez cogido el item y consumido, se haga solo en un slot
    //y no en todos los que tengan ese mismo item, tenemos que definir un
    //metodo que nos regrese una nueva instancia del item.
    public InventarioItem CopiarItem()
    {
        InventarioItem nuevaInstancia = Instantiate(this);
        return nuevaInstancia;
    }

    //Acciones que podemos hacer en un panel de descripcion, por default
    //estaran en verdadero. Gracias a estos metodos definidos con virtual,
    //podremos entrar en cada Item, por ejemplo ItemPocionVida,
    //y sobreescribir, definir la logica dentro del metodo sobreescrito
    //override UsarItem.
    public virtual bool UsarItem()
    {
        return true;
    }

    public virtual bool EquiparItem()
    {
        return true;
    }

    public virtual bool RemoverItem()
    {
        return true;
    }
}   
