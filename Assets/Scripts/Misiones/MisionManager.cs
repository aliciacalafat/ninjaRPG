using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class MisionManager : Singleton<MisionManager>
{
    //Para obtener referencia del jugador, porque lo necesitamos para
    //la funcionalidad del boton de obtener recompensa:
    [Header("Personaje")]
    [SerializeField] private Personaje personaje;

    //Para cargar las misiones en el panel de misiones, debemos obtener
    //una referencia de todas las misiones en una array de tipo Mision,
    //del prefab y de donde lo cargaremos.
    [Header("Misiones")]
    [SerializeField] private Mision[] misionesDisponibles;

    [Header("Master Misiones")]
    [SerializeField] private MasterMisionDescripcion masterMisionPrefab;
    [SerializeField] private Transform masterMisionContenedor;

    
    [Header("Personaje Misiones")]
    [SerializeField] private PersonajeMisionDescripcion personajeMisionPrefab;
    [SerializeField] private Transform personajeMisionContenedor;

    //Referencia de todos los objetos del panel de mision completada para poder
    //actualizarlos:
    [Header ("Panel Mision Completada")]
    [SerializeField] private GameObject panelMisionCompletada;
    [SerializeField] private TextMeshProUGUI misionNombre;
    [SerializeField] private TextMeshProUGUI misionRecompensaOro;
    [SerializeField] private TextMeshProUGUI misionRecompensaExp;
    [SerializeField] private TextMeshProUGUI misionRecompensaItemCantidad;
    [SerializeField] private Image misionRecompensaItemIcono;

    //Para verifricar que la mision completada existe, definimos
    //la siguiente propiedad:
    public Mision MisionPorReclamar {get; private set;}

    private void Start()
    {
        CargarMisionEnMaster();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            AñadirProgreso("Mata10", 1);
            AñadirProgreso("Mata25", 1);
            AñadirProgreso("Mata50", 1);
        }
    }

    //Para cargar las misiones del master. Configuramos todos los pergaminos
    //que estamos añadiendo en cada mision.
    private void CargarMisionEnMaster()
    {
        for(int i = 0; i < misionesDisponibles.Length; i++)
        {
            MasterMisionDescripcion nuevaMision = Instantiate(masterMisionPrefab, masterMisionContenedor);
            nuevaMision.ConfigurarMisionUI(misionesDisponibles[i]);
        }
    }

    //Para instanciar el prefab del personaje:
    private void AñadirMisionPorCompletar(Mision misionPorCompletar)
    {
        PersonajeMisionDescripcion nuevaMision = Instantiate(personajeMisionPrefab, personajeMisionContenedor);
        nuevaMision.ConfigurarMisionUI(misionPorCompletar);
    }

    //Para llamar el metodo de AñadirMisionPorCompletar()
    public void AñadirMision(Mision misionPorCompletar)
    {
        AñadirMisionPorCompletar(misionPorCompletar);
    }

    //Metodo que llama el boton de obtener recompensa. Hay que verificar si tenemos
    //una mision que reclamar. Una vez verificado añadimos el oro, la exp y
    //los items. Por ultimo tenemos que desactivar el panel de mision completada
    //y reseteamos el valor de MisionPorReclamar.
    public void ReclamarRecompensa()
    {
        if(MisionPorReclamar == null)
        {
            return;
        }
        MonedasManager.Instance.AñadirMonedas(MisionPorReclamar.RecompensaOro);
        personaje.PersonajeExperiencia.AñadirExperiencia(MisionPorReclamar.RecompensaExp);
        Inventario.Instance.AñadirItem(MisionPorReclamar.RecompensaItem.Item, MisionPorReclamar.RecompensaItem.Cantidad);
        panelMisionCompletada.SetActive(false);
        MisionPorReclamar = null;
    }

    //Para añadir progreso a la mision que queremos completar:
    public void AñadirProgreso(string misionID, int cantidad)
    {
        Mision misionPorActualizar = MisionExiste(misionID);
        misionPorActualizar.AñadirProgreso(cantidad);
    }

    //Para verificar que la mision que queramos completar existe,
    //necesitamos este metodo que regrese la referencia de la mision
    //que queramios completar: misionID
    private Mision MisionExiste(string misionID)
    {
        for (int i = 0; i < misionesDisponibles.Length; i++)
        {
            if(misionesDisponibles[i].ID == misionID)
            {
                return misionesDisponibles[i];
            }
        }

        return null;
    }

    //Para actualizar todas las referencias de Panel Mision Completada:
    private void MostrarMisionCompletada(Mision misionCompletada)
    {
        panelMisionCompletada.SetActive(true);
        misionNombre.text = misionCompletada.Nombre;
        misionRecompensaOro.text = misionCompletada.RecompensaOro.ToString();
        misionRecompensaExp.text = misionCompletada.RecompensaExp.ToString();
        misionRecompensaItemCantidad.text = misionCompletada.RecompensaItem.Cantidad.ToString();
        misionRecompensaItemIcono.sprite = misionCompletada.RecompensaItem.Item.Icono;
    }

    //Para escuchar evento de que la mision ha sido completada y poder
    //mostrar el panel de recoger recompensa. Verificaremos que la
    //mision completada existe.
    private void MisionCompletadaRespuesta(Mision misionCompletada)
    {
        MisionPorReclamar = MisionExiste(misionCompletada.ID);
        if(MisionPorReclamar != null)
        {
            MostrarMisionCompletada(MisionPorReclamar);
        }
    }
    private void OnEnable()
    {
        Mision.EventoMisionCompletada += MisionCompletadaRespuesta;
    }

    private void OnDisable()
    {
        Mision.EventoMisionCompletada -= MisionCompletadaRespuesta;
    }

}
