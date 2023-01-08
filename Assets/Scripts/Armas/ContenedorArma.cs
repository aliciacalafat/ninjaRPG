using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Para tener acceso a la clase ItemArma le decimos que sea un Singleton.
public class ContenedorArma : Singleton<ContenedorArma>
{
    //Referencia del icono del arma y de su skill, para poder
    //actualizarlos cuando equipemos el arma o la quitemos.
    [SerializeField] private Image armaIcono;
    [SerializeField] private Image armaSkillIcono;

    //Propiedad que almacena que arma tenemos equipada:
    public ItemArma ArmaEquipada {get; set;}

    //Para equipar un arma:
    public void EquiparArma(ItemArma itemArma)
    {
        ArmaEquipada = itemArma;
        armaIcono.sprite = itemArma.Arma.ArmaIcono;
        armaIcono.gameObject.SetActive(true);
        if(itemArma.Arma.Tipo == TipoArma.Magia)
        {
            armaSkillIcono.sprite = itemArma.Arma.IconoSkill;
            armaSkillIcono.gameObject.SetActive(true);
        }
        Inventario.Instance.Personaje.PersonajeAtaque.EquiparArma(itemArma);
    }

    //Para quitar un arma equipada:
    public void RemoverArma()
    {
        armaIcono.gameObject.SetActive(false);
        armaSkillIcono.gameObject.SetActive(false);
        ArmaEquipada = null;
        Inventario.Instance.Personaje.PersonajeAtaque.RemoverArma();
    }
}
