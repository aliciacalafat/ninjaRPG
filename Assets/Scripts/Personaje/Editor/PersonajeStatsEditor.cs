using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Esto es un editor custom para los stats del personaje, PersonajeStats. 
//Es necesario porque si uno juega y va subiendo de nivel o de experiencia o lo que sea, para
//y luego vuelve a jugar, se resetean los valores. Ademas queremos que cada vez
//que vayamos a jugar, tengamos experiencia 0.
[CustomEditor(typeof(PersonajeStats))]
public class PersonajeStatsEditor : Editor
{
    //Para poder obtener informacion de la clase PersonajeStats, debemos
    //obtener el target o objetivo de este editor, que sigue siendo esta clase.
    public PersonajeStats StatsTarget => target as PersonajeStats;

    //Para poder añadir un boton "Resetear Valores" en el inspector, 
    //con el cual resetearemos las estadisticas del personaje, tenemos que sobreescribir
    //el  OnInspectorGUI(), dentro del boton, pondremos el metodo creado en PersonajeStats.
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Resetear Valores"))
        {
            StatsTarget.ResetearValores();
        }
    }
}
