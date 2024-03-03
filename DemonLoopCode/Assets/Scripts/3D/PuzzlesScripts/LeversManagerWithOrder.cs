using UnityEngine;

public class LeversManagerWithOrder : MonoBehaviour
{
    [SerializeField] GameObject Door;

    string nameObjLever = "LeverWithOrder";
    string[] desiredOrder;
    
    int leverOrder;
    int leversActivated;
    int leversNumber;

    private bool leversPuzzleDone = false;

    public bool LeversPuzzleDone { get { return leversPuzzleDone; } }

    private void Start()
    {
        leversActivated = 0;
        leverOrder = 0;

        leversNumber = gameObject.transform.childCount;

        //Lo de desiredOrder y lo del for
        //lo hacemos para rellenarlo y que no sea null.
        desiredOrder = new string[leversNumber];

        //Guardamos toda las cantidades de palanacas hijas
        //por si queremos añadir mas .
        for (int i = 0; i < leversNumber; i++)
            desiredOrder[i] = nameObjLever + i;
    }

    // Incluye las palancas activadas a una lista y si ya estan todas activadas se da por finalizado el puzle.
    public void AddActivatedLever(string leverName)
    {
        //Si la palanca que pulsamos es la que corresponde al orden entrara
        //en el caso de no se reiniciara.
        if (leverOrder < desiredOrder.Length && leverName == desiredOrder[leverOrder])
        {
            leversActivated++;
             // Una vez que todas las palancas esten activas el puzle se dara por finalizado.
            if (leversActivated == desiredOrder.Length)
            {
                leversPuzzleDone = true;
                
                foreach (Transform child in gameObject.transform)
                    child.GetComponent<LeverWithOrderData>().PuzzleDone();

                Door.SetActive(false);
            }
            else
                leverOrder++;
        }
        else
            ResetPuzzle();
    }

    // Desactiva la palanca.
    public void TakeInactiveLever()
    {
        if (leversActivated > 0)
        {
            leversActivated--;
            leverOrder--;
        }
    }//Fin de TakeInactiveLever

    // Resetea el puzle.
    private void ResetPuzzle()
    {
        leversActivated = 0;
        leverOrder = 0;
    }//Fin de ResetPuzzle



}
