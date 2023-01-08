using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class DialogoManager : Singleton<DialogoManager>
{
    //Referencia del panel de dialogo,icono, nombre
    //y texto donde se pondra la conversacion con
    //el NPC, para actualizarlo.
    [SerializeField] private GameObject panelDialogo;
    [SerializeField] private Image npcIcono;
    [SerializeField] private TextMeshProUGUI npcNombreTMP;
    [SerializeField] private TextMeshProUGUI npcConversacionTMP;

    //Para interaccionar con el NPC:
    public NPCInteraccion NPCDisponible {get; set;}

    //Para almacenar todos los dialogos en secuencia, utilizaremos una queue:
    private Queue<string> dialogosSecuencia;

    //Para mostrar el saludo con una animacion letra por letra, primero
    //referenciamos la siguiente variable:
    private bool dialogoAnimado;

    //Para mostrar la despedida una vez que hayamos acabado las oraciones:
    private bool despedidaMostrada;

    //Para inicializar la Queue:
    private void Start()
    {
        dialogosSecuencia = new Queue<string>();
    }

    //Queremos que el panel del dialogo se abra cuando le damos a la
    //tecla E, el saludo, y despues si le damos a espacio, el resto
    //de oraciones. Primero comprobaremos si tenemos un NPCDisponible
    //por el cual debamos cargar su info.
    //En el bucle del espacio, primero hay que comprobar si ya se
    //mostro la despedida. Si se mostro, hay que cerrar el panel y
    //resetear la variable despedidaMostrada para mas adelante poder
    //hablar con otro NPC y luego regresar para no continuar con 
    //este codigo. Tambien hay que verificar si el NPC tiene
    //algun tipo de interaccion extra, para que salga las opciones
    //de esa interaccion despues de darle a espacio.
    private void Update()
    {
        if(NPCDisponible == null)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            ConfigurarPanel(NPCDisponible.Dialogo);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(despedidaMostrada)
            {
                AbrirCerrarPanelDialogo(false);
                despedidaMostrada = false;
                return;
            }

            if(NPCDisponible.Dialogo.ContieneInteraccionExtra)
            {
                UIManager.Instance.AbrirPanelInteraccion(NPCDisponible.Dialogo.InteraccionExtra);
                AbrirCerrarPanelDialogo(false);
                return;
            }

            if(dialogoAnimado)
            {
                ContinuarDialogo();
            }
        }
    }

    //Para mostrar el panel y actualizar el icono y el nombre del NPC,
    //primero necesitaremos un metodo que nos permita abrir y cerrar
    //el panel del dialogo.
    public void AbrirCerrarPanelDialogo(bool estado)
    {
        panelDialogo.SetActive(estado);
    }

    //Para configurar un panel, debemos pasar como parametro un 
    //NPCDialogo para acceder al nombre y al icono.
    //Primero abrimos el panel de dialogo.
    //Para configurar lo que nos tiene que decir el NPC,
    //creamos el metodo CargarDialogosSecuencia.
    //Despues actualizamos el icono y el nombre.
    //Por ultimo para animar el texto llamamos al metodo que llama
    //a la corutina.
    public void ConfigurarPanel(NPCDialogo npcDialogo)
    {
        AbrirCerrarPanelDialogo(true);
        CargarDialogosSecuencia(npcDialogo);
        npcIcono.sprite = npcDialogo.Icono;
        npcNombreTMP.text = npcDialogo.Nombre;
        MostrarTextoConAnimacion(npcDialogo.Saludo);
    }

    //Para configurar lo que nos tiene que decir el NPC, haremos lo siguiente.
    //Primero verificamos si tenemos conversacion que cargar, con el primer if.
    //Si tenemos conversacion que mostrar, vamos al for que recorrera toda
    //la conver llamando al queue para cada iteracion.
    private void CargarDialogosSecuencia(NPCDialogo npcDialogo)
    {
        if(npcDialogo.Conversacion == null || npcDialogo.Conversacion.Length <= 0)
        {
            return;
        }

        for(int i = 0; i < npcDialogo.Conversacion.Length; i++)
        {
            dialogosSecuencia.Enqueue(npcDialogo.Conversacion[i].Oracion);
        }
    }

    //Para que siga con las oraciones despues del saludo y la despedida,
    //ambas animadas. Para mostrar la despedida debemos verificar si
    //ya no tenemos dialogos dentro del queue dialogosSecuencia,
    //porque en ese caso simplemente mostraremos la despedida.
    private void ContinuarDialogo()
    {
        if(NPCDisponible == null)
        {
            return;
        }

        if(despedidaMostrada)
        {
            return;
        }

        if(dialogosSecuencia.Count == 0)
        {
            string despedida = NPCDisponible.Dialogo.Despedida;
            MostrarTextoConAnimacion(despedida);
            despedidaMostrada = true;
            return;
        }

        string siguienteDialogo = dialogosSecuencia.Dequeue();
        MostrarTextoConAnimacion(siguienteDialogo);
    }

    //Para animar el saludo letra por letra de un NPC, crearemos
    //la siguiente corutina.
    //Debemos obtener la cantidad de letras que tenemos en la oracion.
    //Para ello ponemos todos los caracteres de la oracion en un array.
    //Con el for recorremos cada letra de la oracion y dentro le decimos
    //que rellene la conversacion letra por letra.
    //Por ultimo esperamos unos segundos.
    private IEnumerator AnimarTexto(string oracion)
    {
        dialogoAnimado = false;
        npcConversacionTMP.text = "";
        char[] letras = oracion.ToCharArray();
        for (int i = 0; i < letras.Length; i++)
        {
            npcConversacionTMP.text += letras[i];
            yield return new WaitForSeconds(0.03f);
        }
        dialogoAnimado = true;
    }

    //Para llamar a la corutina de AnimarTexto, creamos el siguiente metodo:
    private void MostrarTextoConAnimacion(string oracion)
    {
        StartCoroutine(AnimarTexto(oracion));
    }
}
