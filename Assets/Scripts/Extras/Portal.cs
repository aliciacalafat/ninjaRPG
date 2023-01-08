using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Para teletransportar al personaje al interior de las casas.
public class Portal : MonoBehaviour
{
    //Referencia de la posicion a la cual queremos teletransportar
    //al personaje.
    [SerializeField] private Transform nuevaPos;

    //Para mover al personaje:
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            other.transform.localPosition = nuevaPos.position;
        }
    }
}
