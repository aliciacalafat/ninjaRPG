using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase Base, de la cual pueden heredar el resto.
public abstract class IADecision : ScriptableObject
{
    //Para tomar una accion debemos tener la referencia 
    //de la clase principal de un enemigo IAController.
    public abstract bool Decidir(IAController controller);

}
