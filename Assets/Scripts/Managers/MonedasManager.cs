using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase para controlar el oro. Contendra dos metodos principales,
//de añadir monedas, otra eliminarlas y otro para cargar monedas.
public class MonedasManager : Singleton<MonedasManager>
{

    //Para iniciar el juego con monedas y poder testearlo:
    [SerializeField] private int monedasTest;

    //Propiedad que almacene la cantidad de monedas totales que tenemos.
    public int MonedasTotales {get; set;}

    //Para almacenar monedas, necesitamos una key:
    private string KEY_MONEDAS = "NINJARPG_MONEDAS";

    //Para evitar guardar un valor diferente de monedas cada vez que llamamos
    //a estos metodos, tenemos que deletear las KEY_MONEDAS.
    private void Start()
    {
        PlayerPrefs.DeleteKey(KEY_MONEDAS);
        CargarMonedas();
    }

    //Para cargar monedas, debemos inicializarlas al valor que tenemos
    //guardado en la KEY_MONEDAS.
    private void CargarMonedas()
    {
        MonedasTotales = PlayerPrefs.GetInt(KEY_MONEDAS, monedasTest);
    }

    //Para añadir monedas sumamos la cantidad que queramos añadir y
    //para guardarlas utilizamos setint, que utiliza una contraseña:
    public void AñadirMonedas(int cantidad)
    {
        MonedasTotales += cantidad;
        PlayerPrefs.SetInt(KEY_MONEDAS, MonedasTotales);
        PlayerPrefs.Save();
    }

    //Para eliminar monedas primero tenemos que verificar si tenemos
    //suficientes para quitar la cantidad que queramos quitar:
    public void EliminarMonedas(int cantidad)
    {
        if(cantidad > MonedasTotales)
        {
            return;
        }

        MonedasTotales -= cantidad;
        PlayerPrefs.SetInt(KEY_MONEDAS, MonedasTotales);
        PlayerPrefs.Save();
    }


}
