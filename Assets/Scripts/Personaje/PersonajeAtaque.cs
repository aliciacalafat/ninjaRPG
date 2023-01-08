using System.Collections;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

//Clase para que el personaje lleve un arma.
public class PersonajeAtaque : MonoBehaviour
{
    //Para mostrar el daño del enemigo, lanzaremos un evento. Para
    //evitar que el daño que le haces a un enemigo se vea en 
    //mas de un enemigo, le tenemos que compartir la referencia
    //del daño al enemigo mismo, es decir EnemigoVida,
    //para luego en PersojeFX podamos verificar que el enemigo
    //que esta sufriendo el daño es el mismo enemigo del cual
    //se ha lanzado la referencia:
    public static Action<float, EnemigoVida> EventoEnemigoDañado;

    //Para poder utilizar los metodos de añadir o remover puntos
    //a los stats del personaje segun el arma utilizada,
    //debemos obtener la siguiente referencia:
    [Header("Stats")]
    [SerializeField] private PersonajeStats stats;

    //Con la referencia del prefab del proyectil que hemos creado en 
    //la clase Arma, podemos obtener una referencia del ObjectPooler
    //para asi poder llamar a su metodo de crear Pooler y pasar
    //la referencia de ese prefab del proyectil.
    [Header("Pooler")]
    [SerializeField] private ObjectPooler pooler;

    //Referencia de las 4 posiciones de disparo de proyectil creadas
    //en el personaje principal y el tiempo entre cada proyectil:
    [Header("Ataque")]
    [SerializeField] private float tiempoEntreAtaques;
    [SerializeField] private Transform[] posicionesDisparo;

    //Propiedad donde almacenamos la referencia de arma equipada.
    public Arma ArmaEquipada {get; set;}

    //Propiedad donde almacenamos la referencia una vez que hemos 
    //seleccionado al enemigo.
    public EnemigoInteraccion EnemigoObjetivo {get; private set;}

    //Antes de lanzar el proyectil debemos verificar que tengamos mana suficiente:
    private PersonajeMana _personajeMana;

    //Para controlar si se puede atacar o no teniendo en cuenta el tiempoEntreAtaques:
    private float tiempoParaSiguienteAtaque;

    //Variable para almacenar la referencia de en que posicion de disparo
    //del proyectil hay que utilizar (si el personaje se mueve hacia
    //la derecha por ejemplo, la posicion de disparo sera la de la derecha):
    private int indexDireccionDisparo;
    
    //Para mostrar las animaciones de atacar, primero debemos saber si estamos
    //atacando:
    public bool Atacando {get; set;}

    private void Update()
    {
        ObtenerDireccionDisparo();
        if(Time.time > tiempoParaSiguienteAtaque)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(ArmaEquipada == null || EnemigoObjetivo == null)
                {
                    return;
                }
                UsarArma();
                tiempoParaSiguienteAtaque = Time.time + tiempoEntreAtaques;
                StartCoroutine(IEEstablecerCondicionAtaque());
            }
        }
    }

    private void Awake()
    {
        _personajeMana = GetComponent<PersonajeMana>();
    }

    //Para utilizar el Arma teniendo en cuenta el mana disponible:
    private void UsarArma()
    {
        if(ArmaEquipada.Tipo == TipoArma.Magia)
        {
            if(_personajeMana.ManaActual < ArmaEquipada.ManaRequerida)
            {
                return;
            }
            GameObject nuevoProyectil = pooler.ObtenerInstancia();
            nuevoProyectil.transform.localPosition = posicionesDisparo[indexDireccionDisparo].position;

            Proyectil proyectil = nuevoProyectil.GetComponent<Proyectil>();
            proyectil.InicializarProyectil(this);

            nuevoProyectil.SetActive(true);
            _personajeMana.UsarMana(ArmaEquipada.ManaRequerida);
        }
        else
        {
            float daño = ObtenerDaño();
            EnemigoVida enemigoVida = EnemigoObjetivo.GetComponent<EnemigoVida>();
            enemigoVida.RecibirDaño(daño);
            EventoEnemigoDañado?.Invoke(daño, enemigoVida);
        }
    }

    //Para poder dañar al enemigo, creamos este metodo que nos devuelva cuanto le hacemos de daño.
    //Primero obtenemos una referencia del daño actual que hace el personaje, después
    //utilizaremos el porcentaje de critico del personaje para duplicar el daño.
    //Es decir, si el valor random que nos sale entre 0 y 1 es menor o si esta entre los parametros
    //de nuestro porcentaje critico, vamos a duplicar la cantidad de daño que podemos hacer al 
    //enemigo. Si el daño que le hacemos no cumple esto, vamos a hacerle daño con esa cantidad:
    public float ObtenerDaño ()
    {
        float cantidad = stats.Daño;
        if(Random.value < stats.PorcentajeCritico / 100)
        {
            cantidad *= 2;
        }
        return cantidad;
    }

    //Para poder establecer atacando verdadero y luego falso, para asi saber si estamos
    //atacando o no y poder mostrar las animaciones de atacar:
    private IEnumerator IEEstablecerCondicionAtaque()
    {
        Atacando = true;
        yield return new WaitForSeconds(0.3f);
        Atacando = false;
    }

    //Para equipar un arma, antes de llamar al pooler relacionado
    //con los proyectiles de las armas magicas, debemos verificar
    //si el arma es de tipo magia:
    public void EquiparArma(ItemArma armaPorEquipar)
    {
        ArmaEquipada = armaPorEquipar.Arma;
        if(ArmaEquipada.Tipo == TipoArma.Magia)
        {
            pooler.CrearPooler(ArmaEquipada.ProyectilPrefab.gameObject);
        }
        stats.AñadirBonusPorArma(ArmaEquipada);
    }

    //Para quitar un arma primero debemos verificar que tengamos 
    //un arma equipada:
    public void RemoverArma( )
    {
        if(ArmaEquipada == null)
        {
            return;
        }
        if(ArmaEquipada.Tipo == TipoArma.Magia)
        {
            pooler.DestruirPooler();
        }
        stats.RemoverBonusPorArma(ArmaEquipada);
        ArmaEquipada = null;
    }

    //Para obtener la direccion de disparo utilizaremos la variable
    //indexDireccionDisparo que va del 0 al 3, que como hemos especificado
    //en unity: 0 es arriba,1 derecha,2 abajo y 3 es izquierda.
    //Primero debemos obtener hacia que direccion se mueve el personaje,
    //que definimos en la clase PersonajeMovimiento, lo guardamos en input.
    private void ObtenerDireccionDisparo()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (input.x > 0.1f)
        {
            indexDireccionDisparo = 1;
        }
        else if (input.x < 0f)
        {
            indexDireccionDisparo = 3;
        }
        else if (input.y > 0.1f)
        {
            indexDireccionDisparo = 0;
        }
        else if (input.y < 0f)
        {
            indexDireccionDisparo = 2;
        }
    }

    //Para suscribirse y desuscribirse a un personaje para seleccionarlo:
    private void EnemigoRangoSeleccionado(EnemigoInteraccion enemigoSeleccionado)
    {
        if(ArmaEquipada == null)
        {
            return;
        }
        if(ArmaEquipada.Tipo != TipoArma.Magia)
        {
            return;
        }
        if(EnemigoObjetivo == enemigoSeleccionado)
        {
            return;
        }
        EnemigoObjetivo = enemigoSeleccionado;
        EnemigoObjetivo.MostrarEnemigoSeleccionado(true, TipoDeteccion.Rango);
    }
    private void EnemigoNoSeleccionado()
    {
        if(EnemigoObjetivo == null)
        {
            return;
        }
        EnemigoObjetivo.MostrarEnemigoSeleccionado(false, TipoDeteccion.Rango);
        EnemigoObjetivo = null;
    }
    private void EnemigoMeleeDetectado(EnemigoInteraccion enemigoDetectado)
    {
        if(ArmaEquipada == null)
        {
            return;
        }
        if(ArmaEquipada.Tipo != TipoArma.Melee)
        {
            return;
        }
        EnemigoObjetivo = enemigoDetectado;
        EnemigoObjetivo.MostrarEnemigoSeleccionado(true, TipoDeteccion.Melee);
    }
    private void EnemigoMeleePerdido()
    {
        if(ArmaEquipada == null)
        {
            return;
        }
        if(EnemigoObjetivo == null)
        {
            return;
        }
        if(ArmaEquipada.Tipo != TipoArma.Melee)
        {
            return;
        }
        EnemigoObjetivo.MostrarEnemigoSeleccionado(false, TipoDeteccion.Melee);
        EnemigoObjetivo = null;
    }
    private void OnEnable( )
    {
        SeleccionManager.EventoEnemigoSeleccionado += EnemigoRangoSeleccionado;
        SeleccionManager.EventoObjetoNoSeleccionado += EnemigoNoSeleccionado;
        PersonajeDetector.EventoEnemigoDetectado += EnemigoMeleeDetectado;
        PersonajeDetector.EventoEnemigoPerdido += EnemigoMeleePerdido;
    }
    private void OnDisable( )
    {
        SeleccionManager.EventoEnemigoSeleccionado -= EnemigoRangoSeleccionado;
        SeleccionManager.EventoObjetoNoSeleccionado -= EnemigoNoSeleccionado;
        PersonajeDetector.EventoEnemigoDetectado -= EnemigoMeleeDetectado;
        PersonajeDetector.EventoEnemigoPerdido -= EnemigoMeleePerdido;
    }

}
