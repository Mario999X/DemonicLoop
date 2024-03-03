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

    // Activa y desactiva la palanca.
    public void ActivateDesactivateLever()
    {
        if(!isPuzzleDone)
        {
            isActivated = !isActivated;

            // Cambia su estado a activado.
            if (isActivated)
                manager.AddActivatedLever(); Debug.Log("Lever Activated");

            // Cambia su estado a desactivado.
            if (!isActivated)
                manager.TakeInactiveLever(); Debug.Log("Lever Desactivated");
        }
    }

    // Cambia el estado del puzle a completado.
    public void PuzzleDone()
    {
        isPuzzleDone = true;
    }
}
