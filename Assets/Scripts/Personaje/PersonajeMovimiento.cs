using System;
using UnityEngine;

public class PersonajeMovimiento : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    //En Edit>ProjectSettings>InputManager>Axes>Size>Horizontal y Vertical, 
    //veremos los controles del Personaje: ad y ws (o con las flechas).
    private  Vector2 _input; 

    //Para saber en que direccion nos queremos mover:
    private Vector2 _direccionMovimiento;

    //Queremos utilizar la variable anterior en la clase PersonajeAnimaciones.
    //En vez de hacer que pase a ser publica, es aconsejable crear
    //la misma pero publica. Una opcion "antigua" para referenciarla era la siguiente:
    // public Vector2 DireccionMovimiento
    // {
    //     get{
    //         return _direccionMovimiento;
    //     }
    // }
    //Pero es mas corto hacerlo de la siguiente manera:
    public Vector2 DireccionMovimiento => _direccionMovimiento;

    //Velocidad del Personaje. Para poder definir esta variable en el Inspector,
    //siendo privada, se le pone el atributo de [SerializableField].
    //La veriamos de igual manera en el inspector siendo publica y sin el atributo,
    //pero mejor la ponemos en privado (y para que se vea en el Inspector, con el 
    //atributo) para que no sea accesible desde otras clases y
    //no la podamos cambiar sin querer. Si en un futuro, quieres acceder
    //a este valor desde otra clase, no cambies a public esta privada, crea
    //otra propiedad que regrese este valor, con: public float Velocidad => velocidad;
    [SerializeField] private float velocidad;

    //Para que el personaje, despues de apretar las teclas de movimiento, se quede
    //en la ultima animacion de la tecla que se pulso. ".magnitude" nos da la
    //longitud del vector. Si el personaje esta parado, el vector 
    //_direccionMovimiento = 0, no tiene longitud y EnMovimiento sera falso.
    //Si el personaje se mueve, el vector tiene longitud y EnMovimiento sera true.
    public bool EnMovimiento => _direccionMovimiento.magnitude > 0f;

    // Para obtener referencia de _rigidbody2D:
    private void Awake() 
    {
        _rigidbody2D = GetComponent <Rigidbody2D>(); 
    }

    //Para obtener los controles de "Horizontal" con ad y "Vertical" con ws.
    private void Update()
    {
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //Direccion movimiento: x.
        if(_input.x > 0.1f) //Si nos movemos un valor pequeno hacia la derecha
        {
            _direccionMovimiento.x = 1f; // 1f = positivo
        }
        else if (_input.x < 0f) //Si nos movemos hacia la izquierda
        {
            _direccionMovimiento.x = -1f; // -1f = negativo
        }
        else
        {
            _direccionMovimiento.x = 0f; // No nos movemos en Horizontal
        }

        //Direccion movimiento: y.
        if(_input.y > 0.1f) //Si nos movemos un valor pequeno hacia arriba
        {
            _direccionMovimiento.y = 1f; // 1f = positivo
        }
        else if (_input.y < 0f) //Si nos movemos hacia abajo
        {
            _direccionMovimiento.y = -1f; // -1f = negativo
        }
        else
        {
            _direccionMovimiento.y = 0f; // No nos movemos en Vertical
        }
    }

    // Para mover al Personaje, que es un rigidBody, lo mueves:
    // desde la posicion actual _rigidbody2D.position
    // hacia la _direccionMovimiento que obtuvimos en base a las teclas
    // de movimiento que apretamos, multiplicado por la velocidad
    // definida en el Inspector y por Time.fixedDeltaTime.
    private void FixedUpdate()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + _direccionMovimiento * velocidad * Time.fixedDeltaTime);
    }
    
}