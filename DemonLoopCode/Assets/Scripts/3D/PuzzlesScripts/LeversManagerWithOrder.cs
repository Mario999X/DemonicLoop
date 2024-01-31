using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LeversManagerWithOrder : MonoBehaviour
{
    string[] desiredOrder;
    int leverOrder;
    string nameObjLever = "LeverWithOrder";

    private int leversActivated;

    private int leversNumber;

    private bool leversPuzzleDone = false;

    public bool LeversPuzzleDone { get { return leversPuzzleDone; } }

    private void Start()
    {
        leversActivated = 0;

        leversNumber = gameObject.transform.childCount;

        leverOrder = 0;

        //Lo de desiredOrder y lo del for
        //lo hacemos para rellenarlo y que no sea null
        desiredOrder = new string[leversNumber];

        //Guardamos toda las cantidades de palanacas hijas
        //por si queremos añadir mas 
        for (int i = 0; i < leversNumber; i++)
        {
            desiredOrder[i] = nameObjLever + i;
        }

    }

    public void AddActivatedLever(string leverName)
    {
        //Si la palanca que pulsamos es la que corresponde al orden entrara
        //en el caso de no se reiniciara
        if (leverOrder < desiredOrder.Length && leverName == desiredOrder[leverOrder])
        {
            leversActivated++;

            if (leversActivated == desiredOrder.Length)
            {
                leversPuzzleDone = true;
                foreach (Transform child in gameObject.transform)
                {
                    child.GetComponent<LeverWithOrderData>().PuzzleDone();
                }
                Debug.Log("Puzzle completo");
            }
            else
            {
                leverOrder++;
            }
        }
        else
        {
            ResetPuzzle();
        }

    }

    public void TakeInactiveLever()
    {
        if (leversActivated > 0)
        {
            leversActivated--;
            leverOrder--;
        }
    }//Fin de TakeInactiveLever

    private void ResetPuzzle()
    {
        Debug.Log("Reseteo el puzzle");
        leversActivated = 0;
        leverOrder = 0;
    }//Fin de ResetPuzzle



}
