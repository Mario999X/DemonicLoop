using UnityEngine;

public class LeverWithoutOrderData : MonoBehaviour
{
    private bool isActivated = false;
    private bool isPuzzleDone = false;

    private LeversManagerWithoutOrder manager;

    private void Start()
    {
        manager = transform.parent.GetComponent<LeversManagerWithoutOrder>();
    }

    public void ActivateDesactivateLever()
    {
        if(!isPuzzleDone)
        {
            isActivated = !isActivated;

            if(isActivated)
            {
                Debug.Log("manager.gameObject.name" + manager.gameObject.name);
                manager.AddActivatedLever(); Debug.Log("Lever Activated");
            } 

            if(!isActivated)
            {
                manager.TakeInactiveLever(); Debug.Log("Lever Desactivated");
            } 

        } else Debug.Log("Puzzle finished");
    }

    public void PuzzleDone()
    {
        isPuzzleDone = true;
    }
}
