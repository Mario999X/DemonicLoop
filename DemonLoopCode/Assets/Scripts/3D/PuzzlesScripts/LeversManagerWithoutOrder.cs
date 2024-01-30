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

    public void AddActivatedLever()
    {
        leversActivated++;

        if(leversActivated == leversNumber) 
        {
            foreach(Transform child in gameObject.transform)
            {
                child.GetComponent<LeverWithoutOrderData>().PuzzleDone();
            }

            leversPuzzleDone = true;
        }
    }

    public void TakeInactiveLever()
    {
        leversActivated--;
    }

}
