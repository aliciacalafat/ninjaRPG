using System.Collections;
using System;
using UnityEngine;

//Controla la transicion de un estado a otro.
//Para ver la transicion en el inspector, tenemos
//que darle el atributo de:
[Serializable]
public class IATransicion
{

//Para hacer una transicion de un estado a otro, debemos conocer el valor de 
//una decision para ello definimos la variable Decision.
//Cuando la decision regrese verdadero, haremos la transicion a otro estado
//que guardaremos en la variable EstadoVerdadero, si no cambiamos de
//estado lo guardaremos en EstadoFalso
public IADecision Decision;
public IAEstado EstadoVerdadero;
public IAEstado EstadoFalso;

}
