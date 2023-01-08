using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public enum TiposDeAtaque
{
    Melee,
    Placaje
}

//Clase principal de un enemigo, es el cerebro de la IA.
public class IAController : MonoBehaviour
{
    //Queremos lanzar un evento cuando estamos dañando al personaje para
    //asi poder notificar esta accion, de manera que dentro de la clase
    //PersonajeFX podamos llamar al metodo de IEmostratTexto:
    public static Action<float> EventoDañoRealizado;

    //Antes que el enemigo ataque, queremos coger el stat
    //de bloqueo probabilistico del personaje.
    [Header("Stats")]
    [SerializeField] private PersonajeStats stats;

    //Definimos el estado en el que se encuentra un enemigo 
    //inicialmente y uno default que es un estado al que
    //nosotros no queremos transicionar.
    [Header("Estados")]
    [SerializeField] private IAEstado estadoInicial;
    [SerializeField] private IAEstado estadoDefault;
    
    //Para detectar el personaje, necesitamos una variable que nos
    //defina el rango de accion y otra que nos diga cual es el
    //layer mask de nuestro personaje. Ademas necesitamos
    //la velocidad de movimiento:
    [Header("Config")]
    [SerializeField] private float rangoDeteccion;
    [SerializeField] private float rangoDeAtaque;
    [SerializeField] private float rangoDePlacaje;
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float velocidadDePlacaje;
    [SerializeField] private LayerMask personajeLayerMask;

    //Para ataques personajes:
    [Header("Ataque")]
    [SerializeField] private float daño;
    [SerializeField] private TiposDeAtaque tipoAtaque;
    [SerializeField] private float tiempoEntreAtaques;

    //Para crear el Gizmos que nos permita visualizar
    //el rangoDeteccion, el rangoDeAtaque y el
    //rangoDePlacaje. 
    [Header("Debug")]
    [SerializeField] private bool mostrarDeteccion;
    [SerializeField] private bool mostrarRangoAtaque;
    [SerializeField] private bool mostrarRangoPlacaje;

    //Para controlar el tiempo entre ataques del enemigo.
    private float tiempoParaSiguienteAtaque;
    //Para que cuando el enemigo ataque de forma Placaje contra el personaje
    //y no haya un error con los collider de ambos:
    private BoxCollider2D _boxCollider2D;

    //Necesitamos tantpo rangoDeteccion como personajeLayerMask para
    //el metodo DetectarPersonaje de la clase DecisionDetectarPersonaje,
    //pero como son privados, creamos sus propiedades:
    public float RangoDeteccion => rangoDeteccion;
    public LayerMask PersonajeLayerMask => personajeLayerMask;
    public float Daño => daño;
    public TiposDeAtaque TiposDeAtaque => tipoAtaque;

    //Necesitamos velocidadMovimiento para
    //el metodo SeguirPersonaje de la clase AccionSeguirPersonaje,
    //pero como son privados, creamos sus propiedades:
    public float VelocidadMovimiento => velocidadMovimiento;

    //Propiedad para obtener la referencia de la clase EnemigoMoviminento
    //para poder activarla o desactivarla.
    public EnemigoMovimiento EnemigoMovimiento {get; set;}

    //Para guardar la referencia del personaje que hemos detectado:
    public Transform PersonajeReferencia {get; set;}

    //Para organizarse con los estados, vamos a guardar el
    //estado inicial en la siguiente propiedad:
    public IAEstado EstadoActual {get; set;}

    //Tenemos dos rangos de ataque: rangoDeAtaque (melee) y rangoDePlacaje (placaje),
    //para escoger que sea uno u otro, creamos la propiedad:
    public float RangoDeAtaqueDeterminado => tipoAtaque == TiposDeAtaque.Placaje ? rangoDePlacaje : rangoDeAtaque;

    //Para inicializar el estado, para obtener una referencia
    //de la clase EnemigoMovimiento y para obtener una referencia
    //del boxCollider2D:
    private void Start()
    {
        _boxCollider2D = GetComponent <BoxCollider2D>();
        EstadoActual = estadoInicial;
        EnemigoMovimiento = GetComponent<EnemigoMovimiento>();
    }

    //Para ejecutar estado:
    private void Update()
    {
        EstadoActual.EjecutarEstado(this);
    }

    //Para cambiar de un estado a otro y poder transicionar.
    //Queremos transicionar a nuevoEstado, siempre que nuevoEstado
    //sea un estado que queramos transicionar, es decir que
    //nuevoEstado no sea un estado que nosotros no queramos 
    //transicionar (estadoDefault).
    public void CambiarEstado(IAEstado nuevoEstado)
    {
        if(nuevoEstado != estadoDefault)
        {
            EstadoActual = nuevoEstado;
        }
    }

    //Para el ataque melee que hace el enemigo al personaje, verificamos
    //antes si tenemos la referencia del personaje.
    public void AtaqueMelee(float cantidad)
    {
        if(PersonajeReferencia != null)
        {
            AplicarDañoAlPersonaje(cantidad);
        }
    }

    //Para el ataque placaje que hace el enemigo al personaje, verificamos
    //antes si tenemos la referencia del personaje.
    public void AtaquePlacaje(float cantidad)
    {
        StartCoroutine(IEPlacaje(cantidad));
    }

    //Para el ataque placaje que hace el enemigo al personaje, primero
    //debemos obtener la posicion del personaje, la posicion inicial
    //del enemigo desde donde hara el placaje y la direccion hacia la
    //cual se hara el placaje. Creamos la variable posicionDeAtaque
    //para que el enemigo no se pegue tanto cuando ataque.
    //Antes de que el enemigo haga el placaje debemos desactivar su collider
    //para que no haya errores con el collider del personaje.
    //Con el while actualizamos la transicion de ataque mediante
    //la velocidad de placaje y definimos la trayectoria del enemigo
    //como una parabola que lo que hace es ademas de ir hacia el personaje
    //vuelve a su posicion inicial.
    //Una vez que tenemos la interpolacion debemos ajustar la posicion
    //del enemigo.
    //Como estamos creando una corutina entonces espera yield return null;
    //Para hacerle daño al personaje, haremos un check de si seguimos teniendo
    //la referencia del personaje.
    private IEnumerator IEPlacaje(float cantidad)
    {
        Vector3 personajePosicion = PersonajeReferencia.position;
        Vector3 posicionInicial = transform.position;
        Vector3 direccionHaciaPersonaje = (personajePosicion -posicionInicial).normalized;
        Vector3 posicionDeAtaque = personajePosicion - direccionHaciaPersonaje * 0.5f;
        _boxCollider2D.enabled = false;
        //Placaje:
        float transicionDeAtaque = 0F;
        while(transicionDeAtaque <= 1f)
        {
            transicionDeAtaque += Time.deltaTime * velocidadMovimiento;
            float interpolacion = (-Mathf.Pow(transicionDeAtaque, 2) + transicionDeAtaque) *4f;
            transform.position = Vector3.Lerp(posicionInicial, posicionDeAtaque, interpolacion);
            yield return null;
        }
        //Daño:
        if(PersonajeReferencia != null)
        {
            AplicarDañoAlPersonaje(cantidad);
        }
        _boxCollider2D.enabled = true;
    }

    //Para poder atacar al personaje, primero verificamos si le podemos
    //hacer daño teniendo en cuenta su porcentaje de bloqueo.
    //En el dañoPorRealizar nos aseguramos que al atacar siempre
    //hagamos como minimo 1 de daño. Tenemos la referencia de
    //personajeVida para acceder a recibir daño y pasarle la cantidad 
    //de daño creada.
    //Una vez aplicado el daño, llamamos al evento de daño realizado para
    //que se muestre el texto del daño:
    public void AplicarDañoAlPersonaje(float cantidad)
    {
        float dañoPorRealizar = 0;
        if(Random.value < stats.PorcentajeBloqueo / 100)
        {
            return;
        }
        dañoPorRealizar = Mathf.Max(cantidad - stats.Defensa, 1f);
        PersonajeReferencia.GetComponent<PersonajeVida>().RecibirDaño(dañoPorRealizar);
        EventoDañoRealizado?.Invoke(dañoPorRealizar);
    }

    //Para saber si estamos dentro del rango de ataque que
    //tiene el enemigo para atacar al personaje, necesitamos
    //saber la distancia entre el enemigo y el personaje.
    public bool PersonajeEnRangoDeAtaque(float rango)
    {
        float distanciaHaciaPersonaje = (PersonajeReferencia.position - transform.position).sqrMagnitude;
        if(distanciaHaciaPersonaje < Mathf.Pow(rango,2))
        {
            return true;
        }
        return false;
    }

    //Para saber si el enemigo puede atacar teniendo en cuenta 
    //la variable de espera tiempoParaSiguienteAtaque.
    public bool EsTiempoDeAtacar()
    {
        if (Time.time > tiempoParaSiguienteAtaque)
        {
            return true;
        }

        return false;
    }

    //Para actualizar los tiempos entre ataques:
    public void ActualizarTiempoEntreAtaques()
    {
        tiempoParaSiguienteAtaque = Time.time + tiempoEntreAtaques;
    }

    //Para dibujar el Gizmos de rangoDeteccion y el rangoDeAtaque:
    private void OnDrawGizmos()
    {
        if(mostrarDeteccion)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        }
        if(mostrarRangoAtaque)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, rangoDeAtaque);
        }
        if(mostrarRangoPlacaje)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, rangoDePlacaje);
        }
    }
}
