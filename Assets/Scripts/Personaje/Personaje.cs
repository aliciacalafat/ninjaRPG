using UnityEngine;

public class Personaje: MonoBehaviour
{
    //Para manejar el Atributo, debemos crear una referencia a nuestros
    //stats:
    [SerializeField] private PersonajeStats stats;

    //Propiedad para poder equipar un arma al personaje:
    public PersonajeAtaque PersonajeAtaque {get; private set;}

    //Propiedad para poder añadir experiencia al personaje una vez
    //le demos al boton de obtener:
    public PersonajeExperiencia PersonajeExperiencia {get; private set;}

    //Para restaurar al personaje despues de derrotado,
    //debemos utilizar el metodo RestaurarPersonaje()
    //creado en la clase PersonajeVida, para ello debemos 
    //obtener una referencia esa clase:
    //private PersonajeVida _personajeVida;
    //Como necesitamos esta propiedad de _personajeVida en 
    //la clase LevelManager, creamos:
    public PersonajeVida PersonajeVida {get; private set;}

    //Para que el personaje una vez derrotado y revivido
    //pase a tener una animacion de revivido y no de 
    //derrotado, utilizaremos el metodo RevivirPersonaje()
    //definido en PersonajeAnimaciones:
    public PersonajeAnimaciones PersonajeAnimaciones {get; private set;}

    //Para obtener la referencia del metodo para restaurar el mana:
    public PersonajeMana PersonajeMana {get; private set;}

    //Para obtener la referencia de la clase PersonajeVida,
    //la de PersonajeAnimaciones, la de PersonajeMana, PersonajeExperiencia
    //y PersonajeAtaque:
    private void Awake()
    {
        PersonajeAtaque = GetComponent<PersonajeAtaque>();
        PersonajeVida = GetComponent <PersonajeVida>();
        PersonajeAnimaciones = GetComponent <PersonajeAnimaciones>();
        PersonajeMana = GetComponent <PersonajeMana>();
        PersonajeExperiencia = GetComponent <PersonajeExperiencia>();
    }

    //Para restaurar al personaje con el metodo de la clase
    //PersonajeVida y PersonajeMana:
    public void RestaurarPesonaje()
    {
        PersonajeVida.RestaurarPersonaje();
        PersonajeAnimaciones.RevivirPersonaje();
        PersonajeMana.RestablecerMana();
    }

    //Metodo para subir los puntos de los atributos del personaje.
    //Como este metodo hereda del evento EventoAgregarAtributo en 
    //la clase AtributoButton que tiene como argumento el tipoAtributo,
    //debemos ponerlo tambien aqui.
    //Debemos tener en cuenta que solo queremos añadir atributos si 
    //tenemos los puntos suficientes.
    //Segun el tipo de atributo clickeado tenemos un switch que actualiza en 1
    //el texto de cada atributo, y aumenta las caracteristicas del personaje
    //segun el atributo seleccionado.
    //Por ultimo, una vez gastado el punto, tenemos que restarlo a los disponibles.
    private void AtributoRespuesta(TipoAtributo tipo)
    {
        if(stats.PuntosDisponibles <= 0)
        {
            return;
        }

        switch (tipo)
        {
            case TipoAtributo.Fuerza:
                stats.Fuerza++;
                stats.AñadirBonusPorAtributoFuerza();
                break;
            case TipoAtributo.Inteligencia:
                stats.Inteligencia++;
                stats.AñadirBonusPorAtributoInteligencia();
                break;
            case TipoAtributo.Destreza:
                stats.Destreza++;
                stats.AñadirBonusPorAtributoDestreza();
                break;
        }

        stats.PuntosDisponibles -= 1;
    }

    //Para escuchar el evento de EventoAgregarAtributo en la clase AtributoButton:
    private void OnEnable()
    {
        AtributoButton.EventoAgregarAtributo += AtributoRespuesta;
    }

    private void OnDisable()
    {
        AtributoButton.EventoAgregarAtributo += AtributoRespuesta;
    }
}