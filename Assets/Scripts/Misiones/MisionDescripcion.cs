using System.Collections;
using TMPro;
using UnityEngine;

//Clase Base.
public class MisionDescripcion : MonoBehaviour
{
    //Referencia de nombre y descripcion para poder actualizarlo.
    [SerializeField] private TextMeshProUGUI misionNombre;
    [SerializeField] private TextMeshProUGUI misionDescripcion;

    //Para crear la logica de aceptar una mision, hay que verificar
    //que este en el panel del master y para ello creamos
    //la siguiente propiedad:
    public Mision MisionPorCompletar {get; set;}

    //Para actualizar las referencias.Habra dos formas visualizar 
    //las misisones, una desde el Master con una descripcion 
    //textual y otra desde el Player con una descripcion de imagen. 
    //Con lo cual necesitaremos sobreescribir este metodo, haciendo
    //que la clase MasterMisionDescripcion lo herede.
    public virtual void ConfigurarMisionUI(Mision mision)
    {
        MisionPorCompletar = mision;
        misionNombre.text = mision.Nombre;
        misionDescripcion.text = mision.Descripcion;
    }

}
