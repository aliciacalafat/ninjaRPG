using System;
using System.Collections.Generic;
using UnityEngine;

//Para detectar los enemigos cuando tenemos un arma de melee, 
//en vez de hacerlo con ray casting, lo haremos utilizando
//los metodos de OnTriggerEnter y OnTriggerExit.
public class PersonajeDetector : MonoBehaviour
{
    //Para lanzar un evento cuando se ha detectado un enemigo de
    //tipo melee:
    public static Action<EnemigoInteraccion> EventoEnemigoDetectado;
    
    //Para lanzar un evento cuando no se ha detectado un enemigo de
    //tipo melee. El evento dira que hemos perdido la
    //deteccion del enemigo cuando sale de su collider:
    public static Action EventoEnemigoPerdido;

    //Para guardar la referencia del enemigo que estamos
    //detectando:
    public EnemigoInteraccion EnemigoDetectado{get; private set;}

    //Primero verificamos que estemos colisionando con un objeto
    //que tiene el tag de Enemigo.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemigo"))
        {
            EnemigoDetectado = other.GetComponent<EnemigoInteraccion>();
            if(EnemigoDetectado.GetComponent<EnemigoVida>().Salud > 0)
            {
                EventoEnemigoDetectado?.Invoke(EnemigoDetectado);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Enemigo"))
        {
            EventoEnemigoPerdido?.Invoke();
        }
    }
}
