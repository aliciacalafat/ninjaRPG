using UnityEngine;
using Cinemachine;

//Con esta clase podremos hacer transiciones de camara de una zona a otra.
//Es decir, cuando pasemos de zona, se activara la camara de la nueva zona
//y se desactivara la de donde salgamos.
public class ZonaConfiner : MonoBehaviour
{
    //Referencia de la camara virtual (CM vcam1) asociada a cada confiner
    //para poder activarla o desactivarla.
    [SerializeField] private CinemachineVirtualCamera camara;

    //Para poder detectar si el personaje esta saliendo (false) o entrando (true)
    //del confiner, utilizaremos los dos metodos OnTrigger.
    //OnTriggerEnter2D: Se utiliza cuando un objeto X entra en colision
    //con nuestro confiner.
    //OnTriggerExit2D: Se utiliza cuando un objeto X sale de colision
    //con nuestro confiner.
    //Por este motivo, primero nos interesara saber si el personaje
    //esta entrando o saliendo.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            camara.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            camara.gameObject.SetActive(false);
        }
        
    }
}
