using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PersonajeMisionDescripcion : MisionDescripcion
{
    //Referencia de el texto de recompensa oro, experiencia,
    //recompensa de cantidad del item, el sprite del item,
    //tarea objetivo para poder actualizarlos.
    [SerializeField] private TextMeshProUGUI recompensaOro;
    [SerializeField] private TextMeshProUGUI recompensaExp;
    [SerializeField] private TextMeshProUGUI tareaObjetivo;

    [Header("Item")]
    [SerializeField] private Image recompensaItemIcono;
    [SerializeField] private TextMeshProUGUI recompensaItemCantidad;

    private void Update()
    {
        if(MisionPorCompletar.MisionCompletadaCheck)
        {
            return;
        }
        tareaObjetivo.text = $"{MisionPorCompletar.CantidadActual}/{MisionPorCompletar.CantidadObjetivo}";
    }

    //Sobreescribimos el metodo ConfigurarMisionUI para actualizar
    //el panel de mision del player (es mas visual con imagen que
    //el del Master).
    public override void ConfigurarMisionUI(Mision mision)
    {
        base.ConfigurarMisionUI(mision);
        recompensaOro.text = mision.RecompensaOro.ToString();
        recompensaExp.text = mision.RecompensaExp.ToString();
        tareaObjetivo.text = $"{mision.CantidadActual}/{mision.CantidadObjetivo}";
        recompensaItemIcono.sprite = mision.RecompensaItem.Item.Icono;
        recompensaItemCantidad.text = mision.RecompensaItem.Cantidad.ToString();
    }

    //Para actualizar la ultima cantidad de las misiones, definiremos
    //los metodos OnEnable y OnDisable. Primero deberemos
    //verificar si la mision que acaba de ser completada es la
    //misma mision cargada en el Update, para asi evitar actualizar
    //otras misiones que no tengan nada que ver.
    //Una vez actualizada la mision hay que eliminarla del panel.
    //En el OnEnable vamos a verificar que si la mision por
    //completar ya fue completada, entonces desactivamos.
    private void MisionCompletadaRespuesta(Mision misionCompletada)
    {
        if(misionCompletada.ID == MisionPorCompletar.ID)
        {
            tareaObjetivo.text = $"{MisionPorCompletar.CantidadActual}/{MisionPorCompletar.CantidadObjetivo}";
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if(MisionPorCompletar.MisionCompletadaCheck)
        {
            gameObject.SetActive(false);
        }

        Mision.EventoMisionCompletada += MisionCompletadaRespuesta;
    }

    private void OnDisable()
    {
        Mision.EventoMisionCompletada -= MisionCompletadaRespuesta;
    }

}
