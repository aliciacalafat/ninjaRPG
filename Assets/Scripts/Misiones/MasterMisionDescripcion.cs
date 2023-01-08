using TMPro;
using UnityEngine;
public class MasterMisionDescripcion : MisionDescripcion
{
    [SerializeField] private TextMeshProUGUI misionRecompensa;
    
    //Sobreescibe el metodo ConfigurarMisionUI de la clase
    //MisionDescripcion. Para ello con base.Config...()
    //llamamos lo que esta dentro de ese metodo.
    //Definimos como queremos mostrar las recompensas en 
    //el panel del Master, actualizando la referencia
    //del texto que muestra las recompensas misionRescompensa.
    public override void ConfigurarMisionUI(Mision mision)
    {
        base.ConfigurarMisionUI(mision);
        misionRecompensa.text = $"-{mision.RecompensaOro} oro" +
                                $"\n -{mision.RecompensaExp} exp" +
                                $"\n -{mision.RecompensaItem.Item.Nombre} x{mision.RecompensaItem.Cantidad}";
    }

    //Aceptar mision y que salga en el panel de mision del player.
    //Primeo debemos verificar que el pergamino de la mision esta creada en
    //el panel del Master. Una vez añadida nuestra mision en el panel del
    //player, tenemos que borrarla en el panel del master.
    public void AceptarMision()
    {
        if(MisionPorCompletar == null)
        {
            return;
        }

        MisionManager.Instance.AñadirMision(MisionPorCompletar);
        gameObject.SetActive(false);
    }

}
