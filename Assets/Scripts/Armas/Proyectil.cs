using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//La logica de este juego esta pensada para que solo las armas
//magicas tengan proyectiles, esto lo puedes ver en el metodo
//EquiparArma de la clase PersonajeAtaque.
public class Proyectil : MonoBehaviour
{
    //Velocidad del proyectil:
    [Header("Config")]
    [SerializeField] private float velocidad;

    //Para llamar al metodo de ObtenerDaño (del enemigo) definido en
    //la clase PersonajeAtaque, dentro de OnTriggerEnter2D, debemos
    //tener una referencia de la clase PersonajeAtaque:
    public PersonajeAtaque PersonajeAtaque {get; private set;}

    //Variables para la referencia del rigi body, para la direccion
    //a la que se debe mover el proyectil y a que enemigo debe seguir:
    private Rigidbody2D _rigidbody2D;
    private Vector2 direccion;
    private EnemigoInteraccion enemigoObjetivo;

    //Para obtener la referencia del rigid body
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    //Para mover el proyectil:
    private void FixedUpdate()
    {
        if(enemigoObjetivo == null)
        {
            return;
        }
        MoverProyectil();
    }
    private void MoverProyectil()
    {
        direccion = enemigoObjetivo.transform.position - transform.position;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angulo, Vector3.forward);
        _rigidbody2D.MovePosition(_rigidbody2D.position + direccion.normalized * velocidad * Time.fixedDeltaTime);
    }

    //Para coger la referencia de la variable enemigoObjetivo de ese proyectil:
    public void InicializarProyectil(PersonajeAtaque ataque)
    {
        PersonajeAtaque = ataque;
        enemigoObjetivo = ataque.EnemigoObjetivo;
    }

    //Para que el enemigo reciba daño y luego destruir los proyectiles una vez 
    //que colisionan con el creamos este metodo. Además invocamos una referencia
    //al daño que se esta realizando para poder mostrarlo:
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemigo"))
        {
            float daño = PersonajeAtaque.ObtenerDaño();
            EnemigoVida enemigoVida = enemigoObjetivo.GetComponent<EnemigoVida>();
            enemigoVida.RecibirDaño(daño);
            PersonajeAtaque.EventoEnemigoDañado?.Invoke(daño, enemigoVida);
            gameObject.SetActive(false);
        }
    }

}
