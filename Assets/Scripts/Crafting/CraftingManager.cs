using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : Singleton<CraftingManager>
{
    //Referencia del prefab RecetaTarjeta y donde se va a crear:
    [Header("Config")]
    [SerializeField] private RecetaTarjeta recetaTarjetaPrefab;
    [SerializeField] private Transform recetaContenedor;

    //Para poder cargar las recetas en recetaContenedor, necesitamos
    //una referencia de la lista de recetas.
    [Header("Config")]
    [SerializeField] private RecetaLista recetas;

    //Para llamar al metodo de cargarRecetas:
    private void Start()
    {
        CargarRecetas();
    }

    //Para crear las recetas en el panel:
    private void CargarRecetas()
    {
        for(int i = 0; i < recetas.Recetas.Length; i++)
        {
            RecetaTarjeta receta = Instantiate(recetaTarjetaPrefab, recetaContenedor);
            receta.ConfigurarRecetaTarjeta(recetas.Recetas[i]);
        }
    }
}
