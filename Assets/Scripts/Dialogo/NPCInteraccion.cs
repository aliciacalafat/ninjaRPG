using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraccion : MonoBehaviour
{
    //Referencia de la Imagen que sale cuando te acercas a un NPC para conversar:
    [SerializeField] private GameObject npcButtonInteractuar;

    //Referencia del ScriptableObject que contiene el NPC, en referencia al 
    //dialogo en el cual contiene toda su info. Creamos su propiedad publica
    //para acceder desde otras clases.
    [SerializeField] private NPCDialogo npcDialogo;
    public NPCDialogo Dialogo => npcDialogo;

    //Para saber si estamos cerca del personaje y debemos colisionar con el,
    //para que nos salga la Imagen del bocadillo para conversar, definimos
    //los metodos OnTriggerEnter2D y OnTriggerExit2D.
    //Con el if, le decimos que si estamos colisionando con el 
    //player, entonces debemos interactuar (enter) o ocultar (exit).
    //Cuando el player entre en contacto, queremos que se cargue
    //la info del npc en su propio panel, por eso lo del this.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            DialogoManager.Instance.NPCDisponible = this;
            npcButtonInteractuar.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            DialogoManager.Instance.NPCDisponible = null;
            npcButtonInteractuar.SetActive(false);
        }
    }
}
