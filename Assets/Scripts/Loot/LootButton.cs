using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class LootButton : MonoBehaviour
{
    // Referencias del icono y del texto de los items del panel de loot:
    [SerializeField] private Image itemIcono;
    [SerializeField] private TextMeshProUGUI itemNombre;

    //Propiedad del item dropeado que falta por recoger:
    public DropItem ItemPorRecoger{get; set;}

    //Para actualizar el icono y el nombre del item que ira 
    //cargado en la tarjeta dentro del panel de loot.
    public void ConfigurarLootItem(DropItem dropItem)
    {
        ItemPorRecoger = dropItem;
        itemIcono.sprite = dropItem.Item.Icono;
        itemNombre.text = $"{dropItem.Item.Nombre} x{dropItem.Cantidad}";
    }

    //Para recoger los items del panel de loot:
    public void RecogerItem()
    {
        if(ItemPorRecoger == null)
        {
            return;
        }
        Inventario.Instance.AñadirItem(ItemPorRecoger.Item, ItemPorRecoger.Cantidad);
        ItemPorRecoger.ItemRecogido = true;
        Destroy(gameObject);
    }
}
