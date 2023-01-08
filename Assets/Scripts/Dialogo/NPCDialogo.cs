using System;
using System.Collections.Generic;
using UnityEngine;

//Clase con el cuadro de dialogo que nos dice el NPC con su saludo y despedida.
//Queremos hacer una enumeracion que nos diga si el NPC que abre el dialogo
//tiene algun tipo de interaccion extra. Por ejemplo, el tabernero
//que nos abre el panel de misiones con las misiones. Para ello:
public enum InterraccionExtraNPC
{
    Misiones,
    Tienda,
    Crafting
}

//Para poder crear esta clase en nuestras carpetas:
[CreateAssetMenu] 
public class NPCDialogo : ScriptableObject
{
    //Referenciamos el nombre del NPC, icono del NPC, interaccion
    //extra del NPC, si tiene este extra que tipo de interaccion
    //tiene
    [Header("Info")]
    public string Nombre;
    public Sprite Icono;
    public bool ContieneInteraccionExtra;
    public InterraccionExtraNPC InteraccionExtra;

    //Definiremos lo que dice el NPC en su saludo, despedida
    //y conversacion.
    [Header("Saludo")]
    [TextArea] public string Saludo;
    [Header("Chat")]
    public DialogoTexto[] Conversacion;
    [Header("Despedida")]
    [TextArea] public string Despedida;

}

//Para la conversacion del NPC en si mismo, vamos a crear esta nueva clase.
//Para ver esta clase en el inspector le anadimos el siguiente atributo
[Serializable]
public class DialogoTexto
{
    [TextArea] public string Oracion;
}
