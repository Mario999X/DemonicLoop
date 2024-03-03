using UnityEngine;

public class LeversManagerWithoutOrder : MonoBehaviour
{
    private int leversActivated;
    private int leversNumber;

    private bool leversPuzzleDone = false;

    public bool LeversPuzzleDone { get { return leversPuzzleDone; } }

    private void Start()
    {
        leversActivated = 0;

        leversNumber = gameObject.transform.childCount;
    }

    // Cambia el estado de la palanca a activa y comprueba si ya han sido activadas las demas.
    public void AddActivatedLever()
    {
        leversActivated++;

        // Si todas las palancas han sido activadas se da por finalizado el puzle.
        if (leversActivated == leversNumber) 
        {
            foreach(Transform child in gameObject.transform)
                child.GetComponent<LeverWithoutOrderData>().PuzzleDone();

            leversPuzzleDone = true;
        }
    }

    // Cambia el estado de la palanca a desactivado.
    public void TakeInactiveLever()
    {
        leversActivated--;
    }

}
