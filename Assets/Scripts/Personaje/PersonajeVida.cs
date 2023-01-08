using System;
using UnityEngine;

//Hereda de VidaBase, con lo cual tiene acceso a lo que hay en esa clase.
public class PersonajeVida : VidaBase
{
  //Para saber cuando el personaje esta derrotado, necesitamos crear un evento:
  public static Action EventoPersonajeDerrotado;

  //Cuando el personaje es derrotado, desactivaremos su collider para evitar 
  //problemas de colisiones con enemigos:
  private BoxCollider2D _boxCollider2D;

  //Para resucitar al personaje, primero debemos saber si fue derrotado.
  //Para ello, definiremos la siguiente propiedad con private set para
  //que solo se pueda modificar dentro de esta clase PersonajeVida
  //pero con un get a secas para que lo podamos pillar desde otro lado.
  public bool Derrotado { get; private set;}

  //Para aplicar la logica de saber si un personaje puede ser curado o no:
  public bool PuedeSerCurado => Salud < saludMax; 

  //Para obtener la referencia de _boxCollider2D:
  private void Awake()
  {
    _boxCollider2D = GetComponent<BoxCollider2D>();
  }

  //Para que al iniciar el juego, se actualice la barra de vida, primero debemos
  //llamar al metodo Start que hay en vidaBase:
  protected override void Start()
  {
    base.Start();
    ActualizarBarraVida(Salud, saludMax);
  }

  //Para probar esta clase, haz una prueba tonta, que cuando pulses la T
  //el jugador reciba daño y cuando pulses la R la restaure:
  private void Update()
  {
      if(Input.GetKeyDown(KeyCode.T))
      {
        RecibirDaño(10);
      }
      if(Input.GetKeyDown(KeyCode.Y))
      {
        RestaurarSalud(10);
      }
  }
  //Para restaurar la salud del Personaje, cierta cantidad, debemos saber 
  //si el personaje puede ser curado (segundo if). Para no pasarnos de la 
  //salud maxima, ponemos el tercer if. Por ultimo, debemos actualizar
  //la barra de vida. El primer if lo ponemos para no restaurar la vida
  //si el personaje ha sido derrotado.
  public void RestaurarSalud(float cantidad)
  {
    if (Derrotado)
    {
      return;
    }
    if(PuedeSerCurado)
    {
      Salud += cantidad;
      if (Salud > saludMax)
      {
        Salud = saludMax;
      }
    }
    
    ActualizarBarraVida(Salud, saludMax);
  }

  //Sobreescribimos el metodo PersonajeDerrotado de la VidaBase 
  //para poder lanzar el evento de EventoPersonajeDerrotado:
  protected override void PersonajeDerrotado()
  {
    //Para desactivar el collider una vez el personaje es derrotado:
    _boxCollider2D.enabled = false;
    //El personaje ha sido derrotado:
    Derrotado = true;
    //Antes de lanzar el evento, debemos asegurarnos que tiene
    //suscriptores. Es decir, que haya clases que escuchen el evento (el if).
    // if (EventoPersonajeDerrotado != null)
    // {
    //   EventoPersonajeDerrotado.Invoke();
    // }
    //Esto mismo se puede escribir:
    EventoPersonajeDerrotado?.Invoke();
  }

  //Para revivir al personaje:
  public void RestaurarPersonaje()
  {
    _boxCollider2D.enabled = true;
    Derrotado = false;
    Salud = saludInicial;
    ActualizarBarraVida(Salud, saludInicial);
  }

  //Sobreescribimos ActualizarBarraVida de la VidaBase. Podemos
  //llamar al metodo de UImanager, porque hemos puesto un 
  //singleton sencillo en UImanager:
  protected override void ActualizarBarraVida(float vidaActual, float vidaMax)
  {
    UIManager.Instance.ActualizarVidaPersonaje(vidaActual, vidaMax);
  }
}
