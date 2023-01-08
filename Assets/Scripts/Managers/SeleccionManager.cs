using System;
using System.Collections.Generic;
using UnityEngine;

//Para seleccionar a un enemigo utilizaremos Ray Casting,
//esto es: Lanzar un rayo desde un origen hacia una
//direccion para saber si se ha colisionado con algo.
//En este caso de seleccionar enemigos, se lanzara un rayo desde
//la posicion de la camara hacia la direccion del mouse
//para saber si se clickeo sobre algun enemigo.
//Para la seleccion del enemigo crearemos dos eventos,
//un evento que notifique a que enemigo en particular se ha
//seleccionado para que este enemigo pueda mostrar su aro de
//seleccion y otro evento que notifique que no se ha seleccionado
//ningun enemigo, que se clickeo en otra zona de la escena
// de manera que este evento servira para poder perder la 
//referencia del enemigo seleccionado previamente.
public class SeleccionManager : MonoBehaviour
{
    //Eventos:
    public static Action<EnemigoInteraccion> EventoEnemigoSeleccionado;
    public static Action EventoObjetoNoSeleccionado;

    //Propiedad para almacenar el enemigo seleccionado:
    public EnemigoInteraccion EnemigoSeleccionado {get; set;}

    //Referencia de la camara:
    private Camera camara;

    private void Start()
    {
        camara = Camera.main;
    }

    private void Update()
    {
        SeleccionarEnenmigo();
    }

    //Para seleccionar a un enemigo, primero debemos verificar si estamos 
    //haciendo click, si se da el caso lo que hacemos es un RayCast en 
    //la posicion del mouse hacia el objeto que tiene un layer de Enemigo.
    //Si lo hacemos, guardamos su referencia en hit y con el condicional
    //decimos si hemos colisionado con un enemigo, entonces establecemos
    //esa referencia en la propiedad, lanzamos el evento y si no
    //lanzamos el evento de no objeto seleccionado.
    //Ademas queremos que cuando se muera el enemigo, podamos mostrar el
    //panel de loot, por eso el tercer if:
    private void SeleccionarEnenmigo()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(camara.ScreenToWorldPoint(Input.mousePosition), 
                Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Enemigo"));
            
            if (hit.collider != null)
            {
                EnemigoSeleccionado = hit.collider.GetComponent<EnemigoInteraccion>();
                EnemigoVida enemigoVida = EnemigoSeleccionado.GetComponent<EnemigoVida>();
                if (enemigoVida.Salud > 0f)
                {
                    EventoEnemigoSeleccionado?.Invoke(EnemigoSeleccionado);
                }
                else
                {
                    EnemigoLoot loot = EnemigoSeleccionado.GetComponent<EnemigoLoot>();
                    LootManager.Instance.MostrarLoot(loot);
                }
            }
            else
            {
                EventoObjetoNoSeleccionado?.Invoke();
            }
        }
    }
}
