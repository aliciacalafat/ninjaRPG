using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeExperiencia : MonoBehaviour
{
    //Para coger las referencias del panel:
    [Header("Stats")]
    [SerializeField] private PersonajeStats stats;

    //Algunas Variables para definir la Experiencia,
    //como el nivelmaximo (nivelMax), cuanta experiencia se necesita
    //para subir de nivel (expBase), coeficiente que incrementa
    //exponencialmente la experiencia por cada nivel (valorIncremental)
    [Header("Config")]
    [SerializeField] private int nivelMax;
    [SerializeField] private int expBase;
    [SerializeField] private int valorIncremental;

    //Variables para controlar la experiencia:
    private float expActual;
    private float expActualTemp;
    private float expRequeridaSiguienteNivel;

    //El nivel por el cual empezamos, la exp requerida para el 
    //siguiente nivel y esta misma exp en el panel:
    private void Start()
    {
        stats.Nivel = 1;
        expRequeridaSiguienteNivel = expBase;
        stats.ExpRequeridaSiguienteNivel = expRequeridaSiguienteNivel;
        ActualizarBarraExp();
    }

    //Para probar la logica de Experiencia:
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            AñadirExperiencia(2f);
        }
    }

    //Para añadir experiencia segun cierta cantidad que acabamos
    //de obtener, esta debe ser mayor a 0 (primer if). Definimos
    //la exp restante para pasar de nivel como la exp que requerimos
    //para pasar menos la que tenemos actual. Para subir de nivel
    //debemos asegurarnos que la experiencia obtenida es mayor que 
    //la que nos queda para pasar ese nivel (segundo if). Despues
    //tendremos que actualizar el nivel y volver a hacer
    //el metodo pero con la experiencia actualizada. 
    //Si no es el caso, hacemos el else.
    //Por ultimo actualizamos la barra de exp.
    public void AñadirExperiencia(float expObtenida)
    {
        if(expObtenida > 0f)
        {
            float expRestanteNuevoNivel = expRequeridaSiguienteNivel - expActualTemp;
            if(expObtenida >= expRestanteNuevoNivel)
            {
                expObtenida -= expRestanteNuevoNivel;
                expActual += expObtenida;
                ActualizarNivel();
                AñadirExperiencia(expObtenida);
            }
            else
            {
                expActual += expObtenida;
                expActualTemp += expObtenida;
                if(expActualTemp == expRequeridaSiguienteNivel)
                {
                    ActualizarNivel();
                }
            }
        }
        stats.ExpActual = expActual;
        ActualizarBarraExp();
    }

    //Para subir de nivel, actualizaremos la exp requerida para subir, el propio
    //nivel, la exp actual temporal, actualizar el valor en el panel y los
    //puntos disponibles a gastar en atributos;
    //solo haremos esto con la condicion siguiente:
    private void ActualizarNivel()
    {
        if(stats.Nivel < nivelMax)
        {
            stats.Nivel++;
            expActualTemp =0f;
            expRequeridaSiguienteNivel *= valorIncremental;
            stats.ExpRequeridaSiguienteNivel = expRequeridaSiguienteNivel;
            stats.PuntosDisponibles += 3;
        }
    }

    //Para actualizar la barra de exp:
    private void ActualizarBarraExp()
    {
        UIManager.Instance.ActualizarExpPersonaje(expActualTemp, expRequeridaSiguienteNivel);
    }
}
