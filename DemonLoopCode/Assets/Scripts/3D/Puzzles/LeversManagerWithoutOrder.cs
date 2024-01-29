using System.Linq;
using UnityEngine;

public class LeversManagerWithoutOrder : MonoBehaviour
{
    private GameObject[] leversGroup;

    private int leversActivated;

    private int leversNumber;

    private bool leversPuzzleDone = false;

    public bool LeversPuzzleDone { get { return leversPuzzleDone;} }

    // Start is called before the first frame update
    private void Start()
    {
        leversActivated = 0;

        leversNumber = gameObject.transform.childCount;

        leversGroup = new GameObject[leversNumber];

        int i = 0;
        foreach(Transform child in gameObject.transform)
        {
            leversGroup[i] = child.gameObject;

            i++;
        }
    }

    public void AddActivatedLever()
    {
        leversActivated++;

        if(leversActivated == leversNumber) 
        {
            leversGroup.ToList().ForEach(x => x.GetComponent<LeverWithoutOrderData>().PuzzleDone());

            leversPuzzleDone = true;
        }
    }

    public void TakeInactiveLever()
    {
        leversActivated--;
    }

}
