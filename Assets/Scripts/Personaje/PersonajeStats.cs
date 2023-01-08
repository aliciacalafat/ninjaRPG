using UnityEngine;

[CreateAssetMenu(menuName = "Stats")]
public class PersonajeStats : ScriptableObject
{
    [Header("Stats")]
    //Ponemos los stats con valores by default, 
    //seran necesarios para el reseteo:
    public float Daño = 5f;
    public float Defensa = 2f;
    public float Velocidad = 5f;
    public float Nivel;
    public float ExpActual;
    public float ExpRequeridaSiguienteNivel;
    [Range(0f, 100f)] public float PorcentajeCritico;
    [Range(0f, 100f)] public float PorcentajeBloqueo;

    //Ponemos los atributos:
    [Header("Atributos")]
    public int Fuerza;
    public int Inteligencia;
    public int Destreza;

    //Ponemos los puntos disponibles que quiero que se vean en el 
    //inspector del ScriptableObjects, no en el panel.
    [HideInInspector] public int PuntosDisponibles;

    //Cuando añadimos un atributo de fuerza aumentamos:
    public void AñadirBonusPorAtributoFuerza()
    {
        Daño += 2f;
        Defensa += 1f;
        PorcentajeBloqueo += 0.03f;
    }

    //Cuando añadimos un atributo de inteligencia aumentamos:
    public void AñadirBonusPorAtributoInteligencia()
    {
        Daño += 3f;
        PorcentajeCritico += 0.2f;
    }

    //Cuando añadimos un atributo de destreza aumentamos:
    public void AñadirBonusPorAtributoDestreza()
    {
        Velocidad += 0.1f;
        PorcentajeBloqueo += 0.05f;
    }

    //Para añadir el porcentaje de critico, de bloqueo y daño que tiene un arma
    //a los stats del personaje, cuando la equipamos:
    public void AñadirBonusPorArma(Arma arma)
    {
        Daño += arma.Daño;
        PorcentajeCritico += arma.ChanceCritico;
        PorcentajeBloqueo += arma.ChanceBloqueo;
    }

    //Para añadir el porcentaje de critico, de bloqueo y daño que tiene un arma
    //a los stats del personaje, cuando la removemos:
    public void RemoverBonusPorArma(Arma arma)
    {
        Daño -= arma.Daño;
        PorcentajeCritico -= arma.ChanceCritico;
        PorcentajeBloqueo -= arma.ChanceBloqueo;
    }

    //Para resetear los stats y atributos del personaje en la 
    //clase/editor PersonajeStatsEditor:
    public void ResetearValores()
    {
        Daño = 5f;
        Defensa = 2f;
        Velocidad = 5f;
        Nivel = 1;
        ExpActual = 0f;
        ExpRequeridaSiguienteNivel = 0f;
        PorcentajeBloqueo = 0f;
        PorcentajeCritico = 0f;

        Fuerza = 0;
        Inteligencia = 0;
        Destreza = 0;

        PuntosDisponibles = 0;
    }
}
