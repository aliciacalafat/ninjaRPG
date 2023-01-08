using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoVida : VidaBase
{
    // Referencia del prefab de la barra de vida del enemigo:
    [Header("Vida")]
    [SerializeField] private EnemigoBarraVida barraVidaPrefab;
    [SerializeField] private Transform barraVidaPosicion;

    //Referencia a un GameObject con el sprite de la cruz de loot/rastros:
    [Header("Rastros")]
    [SerializeField] private GameObject rastros;

    //Referencia de la barra del enemigo que esta siendo instanciada,
    //porque cuando matemos al enemigo también hay que desactivar esa barra
    //para no verla.
    private EnemigoBarraVida _enemigoBarraVidaCreada;

    //Para desactivar al enemigo una vez derrotado:
    private EnemigoInteraccion _enemigoInteraccion;
    private EnemigoMovimiento _enemigoMovimiento;

    //Para obtener una referencia del Sprite Render y Collider del enemigo,
    //porque cuando lo derrotamos queremos desactivar su collider para no
    //colisionar con el y tambien queremos dejar de verlo.
    //Cuando derrotemos a un enemigo vamos a mostrar una cruz en el
    //suelo, que al hacer click allí se verá el loot que ha dejado.
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;

    //Para cortar el comportamiento del enemigo una vez derrotado:
    private IAController _controller;

    private void Awake()
    {
        _enemigoInteraccion = GetComponent<EnemigoInteraccion>();
        _enemigoMovimiento = GetComponent<EnemigoMovimiento>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _controller = GetComponent<IAController>();
    }

    //Sobreescribimos el metodo Start porque ya está en la clase VidaBase:
    protected override void Start()
    {
        base.Start();
        CrearBarraVida();
    }

    //Para crear la barra en la posición del enemigo justo encima
    //de su cabeza (que queremos que desaparezca una vez el enemigo este
    //derrotado):
    private void CrearBarraVida ()
    {
        _enemigoBarraVidaCreada = Instantiate(barraVidaPrefab, barraVidaPosicion);
        ActualizarBarraVida(Salud, saludMax);
    }

    //Sobreescribimos el metodo de BarraVida porque ya está en la clase VidaBase:
    protected override void ActualizarBarraVida(float vidaActual, float vidaMax)
    {
        _enemigoBarraVidaCreada.ModificarSalud(vidaActual, vidaMax);
    }

    //Sobreescribimos el metodo de PersonajeDerrotado que ocurre cuando el 
    //enemigo se queda sin vida, que hay dentro de la clase VidaBase:
    protected override void PersonajeDerrotado()
    {
        DesactivarEnemigo();
    }

    //Para desactivar el enemigo una vez derrotado:
    private void DesactivarEnemigo()
    {
        rastros.SetActive(true);
        _enemigoBarraVidaCreada.gameObject.SetActive(false);
        _spriteRenderer.enabled = false;
        _enemigoMovimiento.enabled = false;
        _controller.enabled = false;
        _boxCollider2D.isTrigger = true;
        _enemigoInteraccion.DesactivarSpritesSeleccion();
    }

}
