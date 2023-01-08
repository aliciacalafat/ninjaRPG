using System.Collections;
using System;
using UnityEngine;

//Para saber si el personaje es enemigo o jugador y asi tener una logica conjunta
//para mostrar el daño enemigo:
public enum TipoPersonaje
{
    Player,
    IA
}

//Personaje FX = Personaje Efectos Especiales.
public class PersonajeFX : MonoBehaviour
{
    //Referencia del ObjectPooler:
    [Header("Pooler")]
    [SerializeField] private ObjectPooler pooler;

    //Obtenemos una referencia de nuestro prefab de canvas
    //texto animacion:
    [Header("Config")]
    [SerializeField] private GameObject canvasTextoAnimacionPrefab;
    //Para especificar en donde queremos mostrar el texto:
    [SerializeField] private Transform canvasTextoPosicion;

    //Si es enemigo o personaje y asi mostrar el daño:
    [Header("Tipo")]
    [SerializeField] private TipoPersonaje tipoPersonaje;

    private EnemigoVida _enemigoVida;

    private void Awake()
    {
        _enemigoVida = GetComponent<EnemigoVida>();
    }

    private void Start()
    {
        pooler.CrearPooler(canvasTextoAnimacionPrefab);
    }

    //Para mostrar el texto, cambiar el daño que estamos recibiendo, posicionarlo...,
    //primero debemos obtener una instancia del pooler en nuevoTextoGO.
    //Para que el texto se mueva con la posicion del personaje utilizamos el SetParent.
    //Para regresar el texto al pooler lo haremos esperar 1s.
    private IEnumerator IEMostrarTexto(float cantidad, Color color)
    {
        GameObject nuevoTextoGO = pooler.ObtenerInstancia();
        TextoAnimacion texto = nuevoTextoGO.GetComponent<TextoAnimacion>();
        texto.EstablecerTexto(cantidad, color);
        nuevoTextoGO.transform.SetParent(canvasTextoPosicion);
        nuevoTextoGO.transform.position = canvasTextoPosicion.position;
        nuevoTextoGO.SetActive(true);

        yield return new WaitForSeconds(1f);
        nuevoTextoGO.SetActive(false);
        nuevoTextoGO.transform.SetParent(pooler.ListaContenedor.transform);
    }

    //Para responder al evento creado dentro de IAController:
    private void RespuestaDañoRecibidoHaciaPlayer(float daño)
    {
        if(tipoPersonaje == TipoPersonaje.Player){
            StartCoroutine(IEMostrarTexto(daño, Color.black));
        }
    }

    private void RespuestaDañoEnemigo(float daño, EnemigoVida enemigoVida)
    {
        if(tipoPersonaje == TipoPersonaje.IA && _enemigoVida == enemigoVida){
            StartCoroutine(IEMostrarTexto(daño, Color.red));
        }
    }

    //Escucha el daño realizado hacia el personaje y hacia el enemigo.
    private void OnEnable()
    {
        IAController.EventoDañoRealizado += RespuestaDañoRecibidoHaciaPlayer;
        PersonajeAtaque.EventoEnemigoDañado += RespuestaDañoEnemigo;
    }
    private void OnDisable()
    {
        IAController.EventoDañoRealizado -= RespuestaDañoRecibidoHaciaPlayer;
        PersonajeAtaque.EventoEnemigoDañado -= RespuestaDañoEnemigo;
    }
}
