using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Definiremos las direcciones a las cuales se puede mover un personaje.
//Para almacenar las dos direcciones: horizontal y vertical,
//creamos una enumeracion:
public enum DireccionMovimiento
{
    Horizontal,
    Vertical
}
public class WaypointMovimiento : MonoBehaviour
{
    [SerializeField] protected float velocidad;

    //Propiedad que nos dice la posicion a la cual nos movemos:
    public Vector3 PuntoPorMoverse => _waypoint.ObtenerPosicionMovimiento(puntoActualIndex);

    //Queremos el metodo ObtenerPosicionMovimiento de la clase
    //Waypoint. Para ello necesitaremos una referencia a esta clase.
    protected Waypoint _waypoint;

    //Referencia del componente Animator:
    protected Animator _animator;

    //Para controlar el index de a que punto queremos movernos:
    protected int puntoActualIndex;

    //Para animar al personaje, queremos saber si se esta moviendo hacia la 
    //izquierda o hacia la derecha.
    protected Vector3 ultimaPosicion;

    private void Start()
    {
        puntoActualIndex = 0;
        _animator = GetComponent<Animator>();
        _waypoint = GetComponent<Waypoint>();
    }

    private void Update()
    {
        MoverPersonaje();
        RotarPersonaje();
        RotarVertical();
        if(ComprobarPuntoActualAlcanzado())
        {
            ActualizarIndexMovimiento();
        }
    }

    //Para mover al personaje:
    private void MoverPersonaje()
    {
        transform.position = Vector3.MoveTowards(transform.position, PuntoPorMoverse, velocidad * Time.deltaTime);
    }

    //Para actualizar el index:
    private bool ComprobarPuntoActualAlcanzado()
    {
        float distanciaHaciaPuntoActual = (transform.position - PuntoPorMoverse).magnitude;
        if(distanciaHaciaPuntoActual < 0.1f)
        {
            ultimaPosicion = transform.position;
            return true;
        }
        return false;
    }

    //Para actualizar el index, creamos este metodo. En el primer if, verificamos si 
    //el punto actual Index es igual al ultimo punto en nuestra ruta, 
    //es decir, si nuestro punto actual llega al ultimo punto de la ruta, entonces
    //hay que resetearlo. Basicamente es para que los personajes
    //se muevan infinitamente, para que vayan en bucle. En el caso en que no hayamos
    //alcanzado el ultimo punto, hay que actualizar el index sin pasarnos
    //del ultimo punto de nuestro array.
    private void ActualizarIndexMovimiento()
    {
        if (puntoActualIndex == _waypoint.Puntos.Length - 1)
        {
            puntoActualIndex = 0;
        }
        else
        {
            if (puntoActualIndex < _waypoint.Puntos.Length - 1)
            {
                puntoActualIndex++;
            }   
        }
    }

    //Para saber si se esta moviendo hacia la izquierda o derecha,
    //y poder asi invertir la animacion, comparamos la
    //ultima posicion con el PuntoPorMoverse.
    //El segundo if significa que nos estamos moviendo hacia la derecha.
    //En el else miramos hacia la izquierda.
    protected virtual void RotarPersonaje()
    {

    }

    //Para saber como vamos a mover las animaciones de vertical y horizontal:
    protected virtual void RotarVertical()
    {

    }
}
