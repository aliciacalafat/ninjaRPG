using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase Base, de la cual pueden heredar el resto.
public abstract class IAAccion : ScriptableObject
{
    //Para poder ejecutar una accion debemos tener la referencia 
    //de la clase principal de un enemigo IAController. 
    public abstract void Ejecutar(IAController controller);
}
