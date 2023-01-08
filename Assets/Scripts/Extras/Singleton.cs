using UnityEngine;

//Queremos que sea una clase muy generica para que pueda ser heredada
//por varias clases.
//Si quiero que varias clases tengan su propio Singleton, su propia instancia,
//esta debe ser heredada por cualquier clase. Por este motivo ponemos la
//<T>, es un simbolo generico, indicando que puede ser heredada por cualquier
//otra clase. Poniendo el tipo de T, le estamos diciendo que las clases que hereden 
//de este Singleton van a ser Componentes.
public class Singleton <T> : MonoBehaviour where T : Component
{
    //T hace referencia a la clase que estamos heredando.
    private static T _instance;

    //La instancia publica hace referencia a la instancia (Instance) que podemos llamar
    //de la clase (UIManager) para poder acceder a sus metodos
    //(ActualizarVidaPersonaje); por ejemplo en PersonajeVida, el metodo 
    //de ActualizarBarraVida. Aqui haremos algo parecido, con get para poder regresar
    //un valor. Nos aseguramos que tengamos la instancia, en caso contrario (primer if),
    //hay que buscarla FindObjectOfType, si sigue sin encontrar la instancia (segundo if)
    //hay que ponerla a la fuerza. Al final del get, regresamos la instancia encontrada.
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject nuevoGO = new GameObject();
                    _instance = nuevoGO.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    //Para inicializar la instancia, utilizamos como siempre el Awake:
    protected void Awake()
    {
        _instance = this as T;
    }
}
