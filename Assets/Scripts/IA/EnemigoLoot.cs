using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoLoot : MonoBehaviour
{
    //Loot disponible que tiene un enemigo al matarlo:
    [Header("Loot")]
    [SerializeField] private DropItem[] lootDisponible;

    //Para cargar el loot de un enemigo en el panel, crearemos
    //una lista que guarde los items que si pudieron ser cargados
    //en el panel:
    private List<DropItem> lootSeleccionado = new List<DropItem>();

    //Para compartir el lootSeleccionado con el LootManager:
    public List <DropItem> LootSeleccionado => lootSeleccionado;

    private void Start()
    {
        SeleccionarLoot();
    }

    private void SeleccionarLoot()
    {
        foreach(DropItem item in lootDisponible)
        {
            float probabilidad = Random.Range(0, 100);
            if (probabilidad <= item.PorcentajeDrop)
            {
                lootSeleccionado.Add(item);
            }
        }
    }


}
