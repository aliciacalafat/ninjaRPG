using System;
using UnityEngine;

public class PersonajeAnimaciones : MonoBehaviour
{
    private Animator _animator;

    //Si el personaje se esta moviendo, queremos activar el layer de Caminar,
    //para mostrar las animaciones de caminar.Si el personaje esta atacando, 
    //queremos activar el layer de Atacar, para mostrar las animaciones de atacar.
    //Si el personaje no se esta moviendo, queremos activar el layer de Idle,
    //para mostrar las animaciones de Idle.
    //Para activar o desactivar los layers, necesitamos conocer el nombre de los
    //layers, para eso ponemos lo siguiente:
    [SerializeField] private string layerIdle;
    [SerializeField] private string layerCaminar;
    [SerializeField] private string layerAtacar;

    //Para pillar la referencia de DireccionMovimiento de la clase PersonajeMovimiento:
    private PersonajeMovimiento _personajeMovimiento;

    //Para pillar la referencia de PersonajeAtaque y asi poder mostrar las animaciones
    //de cuando el personaje esta atacando:
    private PersonajeAtaque _personajeAtaque;

    //En lugar de poner x en varios lugares, como es un parametro que 
    //se le puede cambiar el nombre y seria un conazo cambiarlos en todos, se
    //suele utilizar el StringToHash. Asi en vez de x tenemos direccionX por todo,
    //y si queremos cambiarle el nombre al parametro x es mas facil, porque solo
    //se lo cambiariamos aqui solo una vez.
    private readonly int direccionX = Animator.StringToHash("x");
    private readonly int direccionY = Animator.StringToHash("y");

    //Por buenas practicas, utilizaremos el StringToHash tambien para
    //el parametro de Derrotado.
    private readonly int derrotado = Animator.StringToHash("Derrotado");

    // Para obtener referencia de _animator y de _personajeMovimiento:
    private void Awake()
    {
        _animator = GetComponent <Animator>();
        _personajeMovimiento = GetComponent <PersonajeMovimiento>();
        _personajeAtaque = GetComponent<PersonajeAtaque>();
    }

    //Para acceder a los parametros (x,y) y darles el valor _personajeMovimiento:
    private void Update()
    {   
        //Para actualizar los layers (ver los dos metodos relacionados, mas abajo):
        ActualizarLayers();
        
        //Antes de actualizar los parametros (x,y), hacemos el siguiente if,
        //para poner la logica de la variable EnMovimiento declarada en la
        //clase PersonajeMovimiento por la cual, el personaje mantendra
        //su ultima animacion despues de moverse. En concreto este EnMovimiento
        //nos devuelve un booleano true o false, dependiendo de si el personaje se mueve o no.
        //Con este if, si el personaje no se esta moviendo, hacemos un return,
        //regresamos. Es decir,la logica de este metodo Update, se queda aqui
        //y no continua.
        if(_personajeMovimiento.EnMovimiento == false)
        {
            return;
        }
        _animator.SetFloat(direccionX, _personajeMovimiento.DireccionMovimiento.x);
        _animator.SetFloat(direccionY, _personajeMovimiento.DireccionMovimiento.y);
    }

    //Para activar o desactivar los layers:
    private void ActivarLayer(string nombreLayer)
    {
        //Antes de activar un layer, debemos desactivar los que haya. Para ello
        //hacemos lo siguiente, siendo _animator.layerCount todos los layers que tengamos
        //actualmente y SetLayerWeight() asigna un peso a los layers con 0 es layer 
        //desactivado y 1 es layer activado.
        for(int i = 0; i < _animator.layerCount; i++)
        {
            _animator.SetLayerWeight(i,0);
        }

        //Tras desactivar con el for los layers que haya activados, queremos activar
        //el layer. Pero no sabemos el index que es para asignarle el peso, simplemente
        //lo cogemos con .GetLayerIndex() del parametro nombreLayer.
        _animator.SetLayerWeight(_animator.GetLayerIndex(nombreLayer), 1);
    }
    
    //Para activar un layer del personaje en base al movimiento, caminar, o el de
    //estar parado, los Idle, entonces:
    private void ActualizarLayers()
    {
        if(_personajeAtaque.Atacando)
        {
            ActivarLayer(layerAtacar);
        }
        else if (_personajeMovimiento.EnMovimiento)
        {
            ActivarLayer(layerCaminar);
        }
        else
        {
            ActivarLayer(layerIdle);
        }
    }

    //Una vez que el personaje es derrotado y revivido, cambiando su posicion,
    //para resetear la animacion de personaje derrotado a revivido.
    //Para ello activaremos el layer de Idle y luego le diremos
    //que el personaje ya no esta derrotado.
    public void RevivirPersonaje()
    {
        ActivarLayer(layerIdle);
        _animator.SetBool(derrotado, false);
    }

    //Para que una clase se pueda sobreescribir a un evento, 
    //como el creado en PersonajeVida llamado EventoPersonajeDerrotado,
    //necesitamos onEnable() y onDisable().
    //Dentro de esos dos metodos nos interesara enseñar la animacion
    //de personaje derrotado, por eso definimos el siguiente metodo.
    //Para ello debemos verificar primero si estamos dentro de Idle. 
    //Esto es asi, porque si nos vamos dentro del Animator, podemos 
    //ver que la animacion de Personaje_Derrotado se encuentra dentro 
    //del layer de Idle. Para hacer esto, decimos que si
    //el peso del layer Idle esta activado (=1), podremos
    //mostrar la animacion del personaje derrotado.
    private void PersonajeDerrotadoRespuesta()
    {
        if(_animator.GetLayerWeight(_animator.GetLayerIndex(layerIdle)) == 1)
        {
            _animator.SetBool(derrotado, true);
        }
    }
    //OnEnable() se llama cuando la clase está activada. Es decir,
    //cuando esta clase PersonajueAnimaciones se activa, gracias
    //a este OnEnable, nos vamos a suscribir (con el +=) al evento de
    //PersonajeDerrotado.
    private void OnEnable()
    {
        PersonajeVida.EventoPersonajeDerrotado += PersonajeDerrotadoRespuesta;
    }
    //OnDisable() se llama cuando la clase está desactivada.
    //En este caso, nos desuscribimos (con el -=) del evento de
    //PersonajeDerrotado
    private void OnDisable()
    {
        PersonajeVida.EventoPersonajeDerrotado -= PersonajeDerrotadoRespuesta;
    }
}
