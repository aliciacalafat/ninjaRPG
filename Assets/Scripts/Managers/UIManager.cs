using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton <UIManager>
{
    //Antes de hacer el Singleton.cs en la carpeta Extras, podriamos
    //haber hecho un Singleton Sencillo en esta clase UIManager
    //diciendole que es de tipo MonoBehaviour, de la siguiente
    //manera poniendo el Instance a mano y pasandoselo al Awake:
    //Singleton Sencillo. Debemos llamar al metodo ActualizarVidaPersonaje 
    //de esta clase, dentro de PersonajeVida.cs para poder actualizar el
    //metodo ActualizarBarraVida. Para esto, debemos acceder al metodo
    //ActualizarVidaPersonaje dentro del metodo ActualizarBarraVida.
    //Por este motivo utilizaremos este Singleton sencillo, que nos
    //permite la comunicacion entre clases, creando una instancia
    //de la clase que queremos utilizar:
    //public static UIManager Instance;

    //Para poder actualizar la barra de vida, mana (estamina) y experiencia,
    //primero necesitamos obtener una referencia a su imagen, además de una 
    //referencia de texto para poder mostrar nuestra cantidad de salud, mana o exp.
    //Tambien pondremos las referencias propias del panel de stats, panel de inventario
    //panel de las misiones y los stats. 
    //Lo del header es para hacer el codigo bonito.

    [Header("Stats")]
    [SerializeField] private PersonajeStats stats;

    [Header("Paneles")]
    [SerializeField] private GameObject panelStats;
    [SerializeField] private GameObject panelInventario;
    [SerializeField] private GameObject panelMasterMisiones;
    [SerializeField] private GameObject panelPersonajeMisiones;
    [SerializeField] private GameObject panelTienda;

    [Header("Barra")]
    [SerializeField] private Image vidaPlayer;
    [SerializeField] private Image manaPlayer;
    [SerializeField] private Image expPlayer;

    [Header("Texto")]
    [SerializeField] private TextMeshProUGUI vidaTMP;
    [SerializeField] private TextMeshProUGUI manaTMP;
    [SerializeField] private TextMeshProUGUI expTMP;
    [SerializeField] private TextMeshProUGUI nivelTMP;
    [SerializeField] private TextMeshProUGUI monedasTMP;

    //Para obtener la referencia de todas las variables del panel de stats, 
    //de los atributos y de los puntos disponibles para 
    //mejorar esos atributos:
    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI statDañoTMP;
    [SerializeField] private TextMeshProUGUI statDefensaTMP;
    [SerializeField] private TextMeshProUGUI statCriticoTMP;
    [SerializeField] private TextMeshProUGUI statBloqueoTMP;
    [SerializeField] private TextMeshProUGUI statVelocidadTMP;
    [SerializeField] private TextMeshProUGUI statNivelTMP;
    [SerializeField] private TextMeshProUGUI statExpTMP;
    [SerializeField] private TextMeshProUGUI statExpRequeridaTMP;
    [SerializeField] private TextMeshProUGUI atributoFuerzaTMP;
    [SerializeField] private TextMeshProUGUI atributoInteligenciaTMP;
    [SerializeField] private TextMeshProUGUI atributoDestrezaTMP;
    [SerializeField] private TextMeshProUGUI atributoDisponiblesTMP;

    //Para actualizar la barra de texto de vida, necesitamos su referencia
    //de la propiedad Salud y de saludMax que estan en VidaBase,
    //para ello crearemos el metodo ActualizarVidaPersonaje, que 
    //utilizara las siguientes propiedades:
    private float vidaActual;
    private float vidaMax;

    //Hacemos lo mismo para el mana, necesitamos otras dos variables para
    //almacenar la referencia de la mana:
    private float manaActual;
    private float manaMax;

    //Hacemos lo mismo para la experiencia, necesitamos otras dos variables para
    //almacenar la referencia de la exp:
    private float expActual;
    private float expRequeridaNuevoNivel;

    //Este metodo inicializa la instancia del Singleton Sencillo. 
    //Es decir, la Instance de UIManaget es la propia clase de UIManager:
    // private void Awake()
    // {
    //     Instance = this;
    // }

    //Para poder Actualizar la UIPersonaje() y el panel de stats:
    private void Update()
    {
        ActualizarUIPersonaje();
        ActualizarPanelStats();
    }

    //Para actualizar vidaPlayer, vidaTMP, manaPlayer, manaTMP, expActual, 
    //expRequeridaNuevoNivel y nivelTMP. 
    //El vidaPlayer.fillAmount es la cantidad
    //de llenado de la barra de vida que se mueve entre 0 y 1; Mathf.Lerp es la 
    //interpolacion lineal que utilizaremos para poder cambiar desde 
    //vidaPlayer.fillAmount a vidaActual/vidaMax. La division se hace porque 
    //vidaPlayer.fillAmount va desde 0 a 1; 10f*Time.deltaTime es el valor
    //por el cual hacemos esta interpolacion. Para el Mana y la Exp es lo mismo.
    private void ActualizarUIPersonaje()
    {
        //Para modificar la barra de vida:
        vidaPlayer.fillAmount = Mathf.Lerp(vidaPlayer.fillAmount,
            vidaActual/vidaMax, 10f*Time.deltaTime);
        //Para modificar el texto de la barra de vida utilizaremos el
        //stringInterpolation (el $" ").
        vidaTMP.text = $"{vidaActual}/{vidaMax}";
        //Para modificar la barra de mana:
        manaPlayer.fillAmount = Mathf.Lerp(manaPlayer.fillAmount,
            manaActual/manaMax, 10f*Time.deltaTime);
        //Para modificar el texto de la barra de mana utilizaremos el
        //stringInterpolation (el $" ").
        manaTMP.text = $"{manaActual}/{manaMax}";
        //Para modificar la barra de experiencia:
        expPlayer.fillAmount = Mathf.Lerp(expPlayer.fillAmount,
            expActual/expRequeridaNuevoNivel, 10f*Time.deltaTime);
        //Para modificar el texto de la barra de mana utilizaremos el
        //stringInterpolation (el $" "), lo multiplicamos por
        //100 para saber el porcentaje y le metemos un :F2 para
        //indicar que queremos dos unidades decimales.
        expTMP.text = $"{((expActual/expRequeridaNuevoNivel)*100):F2}%";
        //Para modificar el nivel que se ve arriba de la barra de vida:
        nivelTMP.text = $"Nivel {stats.Nivel}";
        //Para modificar las monedas de oro recogidas:
        monedasTMP.text = MonedasManager.Instance.MonedasTotales.ToString();

    }

    //Para actualizar el panel de stats y atributos. Para ello primero 
    //debemos saber si el panel esta activo, es decir si se esta mostrando. 
    //Despues actualizamos los parametros del panel de stats y atributos.
    private void ActualizarPanelStats()
    {
        if(panelStats.activeSelf == false)
        {
            return;
        }
        statDañoTMP.text = stats.Daño.ToString();   
        statDefensaTMP.text = stats.Defensa.ToString();
        statCriticoTMP.text = $"{stats.PorcentajeCritico}%";
        statBloqueoTMP.text = $"{stats.PorcentajeBloqueo}%";
        statVelocidadTMP.text = stats.Velocidad.ToString();
        statNivelTMP.text = stats.Nivel.ToString();
        statExpTMP.text = stats.ExpActual.ToString();
        statExpRequeridaTMP.text = stats.ExpRequeridaSiguienteNivel.ToString();

        atributoFuerzaTMP.text = stats.Fuerza.ToString();
        atributoInteligenciaTMP.text = stats.Inteligencia.ToString();
        atributoDestrezaTMP.text = stats.Destreza.ToString();
        atributoDisponiblesTMP.text = $"Puntos: {stats.PuntosDisponibles}";
    }

    //Para poder modificar las referencias vidaActual y vidaMax: 
    public void ActualizarVidaPersonaje(float pVidaActual, float pVidaMax)
    {
        vidaActual = pVidaActual;
        vidaMax = pVidaMax;
    }

    //Para poder modificar las referencias manaActual y manaMax: 
    public void ActualizarManaPersonaje(float pManaActual, float pManaMax)
    {
        manaActual = pManaActual;
        manaMax = pManaMax;
    }

    //Para poder modificar las referencias expActual y expRequeridaNuevoNivel: 
    public void ActualizarExpPersonaje(float pExpActual, float pExpRequerida)
    {
        expActual = pExpActual;
        expRequeridaNuevoNivel = pExpRequerida;
    }

    //Para abrir y cerrar el panel de stats, el del inventario y misiones,
    //haremos los siguientes metodos.
    //Lo de #region y #endregion sirve para ordenar el codigo, permite
    //minimizarlo.
    //panelStats.SetActive( ) quiere un booleano para activarlo (true)
    //o desactivarlo (false), para ello se le pasa panelStats.activeSelf
    //que nos dice si el panel se encuentra activo actualmente.
    //Por ejemplo, si tenemos un panel no activo, es decir, cerrado
    //y lo queremos abrir. Ya de per se el panelStats.activeSelf = false
    //porque actualmente esta cerrado, poniendole el ! lo negamos con 
    //lo cual !panelStats.activeSelf = true, de esta manera
    //panelStats.SetActive(true) = se abre el panel.
    //Ademas para asignar un panel a cada una de las 3 interacciones
    //(misiones, tienda, crafteo) que pueden hacer los NPCs, crearemos
    //el metodo AbrirPanelInteraccion().
    #region Paneles
        public void AbrirCerrarPanelStats()
        {
            panelStats.SetActive(!panelStats.activeSelf);
        }

        public void AbrirCerrarPanelInventario()
        {
            panelInventario.SetActive(!panelInventario.activeSelf);
        }

        public void AbrirCerrarPanelPersonajeMisiones()
        {
            panelPersonajeMisiones.SetActive(!panelPersonajeMisiones.activeSelf);
        }

        public void AbrirCerrarPanelMasterMisiones()
        {
            panelMasterMisiones.SetActive(!panelMasterMisiones.activeSelf);
        }

        public void AbrirCerrarPanelTienda()
        {
            panelTienda.SetActive(!panelTienda.activeSelf);
        }

        public void AbrirPanelInteraccion(InterraccionExtraNPC tipoInteraccion)
        {
            switch (tipoInteraccion)
            {
                case InterraccionExtraNPC.Misiones:
                    AbrirCerrarPanelMasterMisiones();
                    break;
                case InterraccionExtraNPC.Tienda:
                    AbrirCerrarPanelTienda();
                    break;
                case InterraccionExtraNPC.Crafting:
                    break;                                        
            }
        }

    #endregion
}
