using UnityEngine;

public class LevelManager: MonoBehaviour
{
    //Una vez derrotado el personaje y restaurado, lo queremos mover
    //a una posicion inicial donde aparezca. Para ello primero
    //debemo obtener una referencia de donde lo restauramos:
    [SerializeField] private Transform puntoReaparicion;
    //Para mover al personaje a puntoReaparicion, debemos tener tambien
    //una referencia de ese personaje:
    [SerializeField] private Personaje personaje;

    //Para mover al personaje a puntoReaparicion, debemos conocer si
    //el personaje ha sido derrotado (segundo if) y esto solo ocurrira
    //si apretamos el boton de revivir (primer if):
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(personaje.PersonajeVida.Derrotado)
            {
                personaje.transform.localPosition = puntoReaparicion.position;
                personaje.RestaurarPesonaje();
            }
        }
    }
}