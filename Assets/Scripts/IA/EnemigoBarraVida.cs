using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Clase para actualizar la barra de vida del enemigo.
public class EnemigoBarraVida : MonoBehaviour
{
    //Referencia de la barra de vida:
    [SerializeField] private Image barraVida;

    //Para controlar la barra del enemigo, creamos las variables
    //de valor de la salud actual y salud maxima del enemigo.
    private float saludActual;
    private float saludMax;

    private void Update()
    {
        barraVida.fillAmount = Mathf.Lerp(barraVida.fillAmount, saludActual / saludMax, 10f * Time.deltaTime);
    }

    //Para actualizar la barra de vida:
    public void ModificarSalud(float pSaludActual, float pSaludMax)
    {
        saludActual = pSaludActual;
        saludMax = pSaludMax;
    }


}
