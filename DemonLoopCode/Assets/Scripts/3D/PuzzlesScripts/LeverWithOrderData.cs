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

    // Se encarga de establecer el estado actual de la palanca.
    public void ActivateDesactivateLever()
    {
        if (!isPuzzleDone)
        {
            isActivated = !isActivated;

            // Cambia su estado a activado.
            if (isActivated)
                manager.AddActivatedLever(gameObject.name);

            // Cambia su estado a desactivado.
            if (!isActivated)
                manager.TakeInactiveLever();
        }
    }
    
    // Se da por finalizado el puzle
    public void PuzzleDone()
    {
        isPuzzleDone = true;
    }
}
