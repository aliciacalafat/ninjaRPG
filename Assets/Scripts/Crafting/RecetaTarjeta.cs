using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecetaTarjeta : MonoBehaviour
{
    //Para actualizar el nombre e icono de las tarjetas, obtenemos
    //sus referencias:
    [SerializeField] private Image recetaIcono;
    [SerializeField] private TextMeshProUGUI recetaNombre;

    //Para guardar referencia de la tarjeta que esta siendo cargada:
    public Receta RecetaCargada{get; private set;}

    //Para actualizar informacion de las tarjetas:
    public void ConfigurarRecetaTarjeta(Receta receta){
        RecetaCargada = receta;
        recetaIcono.sprite = receta.ItemResultado.Icono;
        recetaNombre.text = receta.ItemResultado.Nombre;
    }
}
