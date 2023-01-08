using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Esta clase tiene la logica de la Vida de los personajes, tanto si son
//enemigos como si no, de todos.
public class VidaBase : MonoBehaviour
{
    //Algunas propiedades que defininen la vida de los personajes son las siguientes.
    //En vez de ponerlas en private, las ponemos en protected para que los hijos
    //de esta clase puedan utilizarlas.
    [SerializeField] protected float saludInicial;
    [SerializeField] protected float saludMax;

    //Propiedad que puede regresar o modificar el valor.
    //Como solo modificaremos la Salud dentro de la clase Vida, le damos el 
    //acceso de protected.
    public float Salud {get; protected set;}

    //Para controlar la vida, salud debe ser = salud inicial.
    //Ponemos el protected para que los hijos puedan heredar este metodo.
    protected virtual void Start()
    {
        Salud = saludInicial;
    }

    //Cantidad de dano que vamos a recibir.
    public void RecibirDaño(float cantidad)
    {
        //Para poder recibir dano, antes debemos saber si la cantidad es
        //mayor que 0 y no 0 o inferior, porque no tendria sentido.
        //Entonces si es menor o igual a 0, no recibe dano, por eso ponemos
        //el return, para que no compile las siguientes lineas del metodo.
        if (cantidad <= 0)
        {
            return;
        }

        //Para danar al personaje, nos aseguramos que tenemos salud (el primer if).
        //Si la tenemos, le restamos cantidad a nuestra Salud y actualizamos
        //la barra de vida. En el caso de que cuando danemos al personaje, ya 
        //no tengamos vida, es decir somos derrotados (el segundo if),
        //debemos actualizar la barra y llamar al metodo PersonajeDerrotado().
        if (Salud > 0f)
        {
            Salud -= cantidad;
            ActualizarBarraVida(Salud, saludMax);
            if(Salud <= 0f)
            {
                Salud = 0f;
                ActualizarBarraVida(Salud, saludMax);
                PersonajeDerrotado();
            }
        }
    }

    //Para actualizar la barra del personaje, para ello debemos conocer
    //su salud actual y su salud maxima. Lo de virtual es para
    //poder sobreescribir este metodo. 
    protected virtual void ActualizarBarraVida(float vidaActual, float vidaMax)
    {

    }

    //Para cuando el personaje es derrotado:
    protected virtual void PersonajeDerrotado()
    {

    }

}
