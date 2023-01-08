using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeMana : MonoBehaviour
{
    //Definimos lo que sera la manaInicial, manaMax y la regeneracion por segundo.
    [SerializeField] private float manaInicial;
    [SerializeField] private float manaMax;
    [SerializeField] private float regeneracionPorSegundo;

    //Para modificar el mana, al igual que hicimos con la salud,
    //debemos definir una propiedad. Solo vamos a modificarla
    //dentro de esta clase, por eso el private set.
    public float ManaActual {get; private set;} 

    //Referencia que nos servira en ItemPocionMana para saber si podemos
    //restaurar mana.
    public bool SePuedeRestaurar => ManaActual < manaMax;

    //Para regenerar Mana, debemos antes saber si el personaje tiene vida.
    //Para ello necesitamos una referencia de la clase PersonajeVida.
    private PersonajeVida _personajeVida;

    //Necesitamos obtener el componente de PersonajeVida:
    private void Awake()
    {
        _personajeVida = GetComponent <PersonajeVida>();
    }

    //Inicializamos el Mana. Al iniciar el juego tambien debemos
    //actualizar la barra de Mana. Para regenerar la barra de mana
    //podriamos hacerlo en el metodo Update, pero para no
    //forzar el sistema, lo hacemos con el metodo InvokeRepeating().
    //Este metodo lo que hace es llamar a otro y repetirlo las veces
    //que uno quiera (el segundo 1) por segundo (el primer 1).
    private void Start()
    {
        ManaActual = manaInicial;
        ActualizarBarraMana();
        InvokeRepeating(nameof(RegenerarMana), 1, 1);
    }

    //Para probar si estamos usando Mana, al igual que cuando hicimos
    //con la Salud:
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            UsarMana(10f);
        }
    }

    //Metodo para usar el Mana segun cierta cantidad que queramos.
    //Para ello primero deberemos verificar que tenemos la suficiente
    //Mana para gastar. Despues deberemos actualizar la barra de Mana.
    public void UsarMana(float cantidad)
    {
        if(ManaActual >= cantidad)
        {
            ManaActual -= cantidad;
            ActualizarBarraMana();
        }
    }

    //Para poder restaurar el mana:
    public void RestaurarMana(float cantidad)
    {
        if(ManaActual >= manaMax)
        {
            return;
        }

        ManaActual += cantidad;

        if(ManaActual > manaMax)
        {
            ManaActual = manaMax;
        }

        UIManager.Instance.ActualizarManaPersonaje(ManaActual, manaMax);
    }

    //Para regenerar Mana, debemos saber si el personaje esta con vida
    //y si no tiene el Mana al maximo. Por ultimo hay que actualizar
    //la barra de Mana.
    private void RegenerarMana()
    {   
        if(_personajeVida.Salud > 0F && ManaActual < manaMax)
        {
            ManaActual += regeneracionPorSegundo;
            ActualizarBarraMana();
        }
    }

    //Para que cuando al revivir, el personaje ademas de coseguir
    //full Salud, consiga full Mana:
    public void RestablecerMana()
    {
        ManaActual = manaInicial;
        ActualizarBarraMana();
    }

    //Para actualizar la barra de Mana:
    private void ActualizarBarraMana()
    {
        UIManager.Instance.ActualizarManaPersonaje(ManaActual, manaMax);
    }
}
