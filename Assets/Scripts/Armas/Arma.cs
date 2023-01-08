using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Para definir el tipo de arma:
public enum TipoArma
{
    Magia,
    Melee
}

//Para crear el ScriptableObject de armas en Unity, dentro de esa ruta:
[CreateAssetMenu(menuName = "Personaje/Arma")]
public class Arma : ScriptableObject
{
    //Parametros para crear un arma: el icono del arma, el icono del 
    //atributo de esa arma (por ejemplo el icono libro, tendra un proyectil
    //de fuego), tipo de arma (magia o cuerpo a cuerpo) y el daño inicial.
    [Header("Config")]
    public Sprite ArmaIcono;
    public Sprite IconoSkill;
    public TipoArma Tipo;
    public float Daño;

    //Si el arma es de tipo magia, hay que determinar cuanto mana (estamina) 
    //se va a consumir. Tambien ponemos una variable para poder crear
    //con prefabs los proyectiles de las armas.
    [Header("Arma Magica")]
    public Proyectil ProyectilPrefab;
    public float ManaRequerida;

    //Queremos saber tambien el porcentaje de critico de bloqueo que añade el arma
    //a los stats del jugador.
    [Header("Stats")]
    public float ChanceCritico;
    public float ChanceBloqueo;
}
