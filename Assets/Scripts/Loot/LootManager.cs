using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Definiendo esta clase como un singleton, podremos llamar a 
//metodos creados en otras clases.
public class LootManager : Singleton<LootManager>
{
    // Necesitamos un metodo que permita mostrar el panel
    //de loot y otro para obtener su referencia:
    [Header("Config")]
    [SerializeField] private GameObject panelLoot;

    //Variable del prefab del botón del panel de loot:
    [SerializeField] private LootButton lootButtonPrefab;
    //Variable de donde se va a añadir este prefab:
    [SerializeField] private Transform lootContenedor;

    //Para mostrar el panel, verificaremos si ya esta o no ocupado
    //el panel. Si lo esta, vamos a cargarnos cada hijo que
    //ya esta cargado en el panel, osea cada item que ya esta
    //cargado en ese panel esperando por ser looteado:
    public void MostrarLoot(EnemigoLoot enemigoLoot)
    {
        panelLoot.SetActive(true);
        if(ContenedorOcupado())
        {
            foreach (Transform hijo in lootContenedor.transform)
            {
                Destroy(hijo.gameObject);
            }
        }
        for (int i = 0; i < enemigoLoot.LootSeleccionado.Count; i++)
        {
            CargarLootPanel(enemigoLoot.LootSeleccionado[i]);
        }
    }

    //Para cerrar el panel de loot:
    public void CerrarPanel()
    {
        panelLoot.SetActive(false);
    }

    //Para cargar loot al panel:
    private void CargarLootPanel(DropItem dropItem)
    {
        if(dropItem.ItemRecogido)
        {
            return;
        }

        LootButton loot = Instantiate(lootButtonPrefab, lootContenedor);
        loot.ConfigurarLootItem(dropItem);
        loot.transform.SetParent(lootContenedor);
    }

    //Una vez tenemos el array de Loot Disponible lleno, habiendolo llenado
    //desde Unity añadiendo al enemigo al clase EnemigoLoot, debemos
    //compartir esa informacion con esta clase para poder cargar en el panel
    //de loot aquellos items que si han pasado el porcentaje de drop y por
    //lo tanto se pueden lootear.
    //Queremos ademas que cuando se abra el loot de un enemigo, se abra solo el
    //de ese enemigo y no se cargue o se sume junto los de otro enemigo ya dropeado.
    private bool ContenedorOcupado()
    {
        LootButton[] hijos = lootContenedor.GetComponentsInChildren<LootButton>();
        if (hijos.Length > 0)
        {
            return true;
        }
        return false;
    }
}
