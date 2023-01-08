using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    //Array[] de puntos que conforman la ruta que seguira un NPC
    [SerializeField] private Vector3[] puntos;
    public Vector3[] Puntos => puntos; 
    
    //Propiedad para guardar la posicion del personaje en cada momento:
    public Vector3 PosicionActual {get; set;}

    //Mientras no estemos jugando, mientras no estemos en el modo del play,
    //los puntos deben tener en cuenta la posicion del NPC. Para ello,
    //definimos el siguiente booleano:
    private bool juegoIniciado;

    private void Start()
    {
        juegoIniciado = true;
        PosicionActual = transform.position;
    }

    //Para conocer la posicion del punto al cual nos queremos mover,
    //y asi poder utilizarlo en la clase WaypointMovimiento:
    public Vector3 ObtenerPosicionMovimiento(int index)
    {
        return PosicionActual + puntos[index];
    }

    //Para visualizar la ruta, nos interesa dibujar los puntos 
    //y una linea entre cada punto.
    //Para que al dejar de jugar, los puntos que definen
    //la ruta del NPC, empiecen desde el NPC, tenemos el primer if.
    //Alli lo que hacemos es: Mientras no estemos en play mode y 
    //mientras estemos cambiando la posicion de nuestro NPC,
    //entonces vamos a ir actualizando su posicion actual a 
    //la posicion de su transform.
    //Antes de dibujar nada, debemos verificar si el Array de puntos
    //es nulo o si tenemos puntos que dibujar.
    //En el caso en que tengamos puntos, con un for los recorremos
    //toodos. Dentro queremos visualizarlos ademas de añadir una 
    //raya. Pero antes debemos definir que color queremos utilizar.
    //A continuacion definimos una esfera en cada punto
    //con su centro y radio. Para dibujar las lineas, primero
    //verificamos que no nos excedamos de los puntos que tenemos en el 
    //array (el if dentro del for), despues le decimos que tengan 
    //color gris y definimos las lineas con la primera y siguiente
    //posicion.
    private void OnDrawGizmos()
    {
        if(juegoIniciado == false && transform.hasChanged)
        {
            PosicionActual = transform.position;
        }

        if(puntos == null || puntos.Length <= 0)
        {
            return;
        }

        for(int i = 0; i < puntos.Length; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(puntos[i] + PosicionActual, 0.5f);
            if(i < puntos.Length -1)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(puntos[i] + PosicionActual, puntos[i+1] + PosicionActual);
            }
        }   
    }
}
