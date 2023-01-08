using UnityEditor;
using UnityEngine;

//Para seleccionar las esferas de las rutas de los NPCs
//con el mouse, en vz de hacerlo con los puntos del 
//Waypoint, creamos este editor.
//Con esta clase podremos poner Handles representados en rojo
//en cada punto de  nuestra ruta para poder moverlos con el mouse.
//Es decir, posiciona un Handle en la posicion de cada punto,
//si lo movemos guarda la nueva posicion hacia donde lo
//estemos moviendo y una vez que soltamos los cambios
//y establece la nueva posicion en cada punto de la ruta.
[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor
{

    Waypoint WaypointTarget => target as Waypoint;

    private void OnSceneGUI()
    {
        Handles.color = Color.red;
        if(WaypointTarget.Puntos == null)
        {
            return;
        }

        for (int i = 0; i < WaypointTarget.Puntos.Length; i++)
        {
            EditorGUI.BeginChangeCheck();

            //Crear Handle
            Vector3 puntoActual = WaypointTarget.PosicionActual + WaypointTarget.Puntos[i];
            Vector3 nuevoPunto = Handles.FreeMoveHandle(puntoActual, Quaternion.identity, 0.7f, new Vector3(0.3f, 0.3f, 0.3f), Handles.SphereHandleCap);
            
            //Crear Texto
            GUIStyle texto = new GUIStyle();
            texto.fontStyle = FontStyle.Bold;
            texto.fontSize = 16;
            texto.normal.textColor = Color.black;
            Vector3 alineamiento = Vector3.down * 0.3f + Vector3.right *0.3f;
            Handles.Label(WaypointTarget.PosicionActual + WaypointTarget.Puntos[i] + alineamiento, $"{i + 1}", texto);
           
            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Free Monve Handle");
                WaypointTarget.Puntos[i] = nuevoPunto - WaypointTarget.PosicionActual;
            }
        }

    }
}
