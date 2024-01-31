using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverWithOrderData : MonoBehaviour
{
    private bool isActivated = false;
    private bool isPuzzleDone = false;

    private LeversManagerWithOrder manager;

    private void Start()
    {
        manager = transform.parent.GetComponent<LeversManagerWithOrder>();
    }

    public void ActivateDesactivateLever()
    {
        if (!isPuzzleDone)
        {
            isActivated = !isActivated;

            if (isActivated)
            {
                Debug.Log("Nombre del hijo tocado " + gameObject.name);
                manager.AddActivatedLever(gameObject.name); Debug.Log("Lever Activated");
            }

            if (!isActivated)
            {
                manager.TakeInactiveLever(); Debug.Log("Lever Desactivated");
            }

        }
        else Debug.Log("Puzzle finished");
    }

    public void PuzzleDone()
    {
        isPuzzleDone = true;
    }
}
