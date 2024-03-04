using UnityEngine;

public class CubePuzzleManager : MonoBehaviour
{
    private bool leversPuzzleDone = false;

    public bool LeversPuzzleDone { get { return leversPuzzleDone;} }

    // Actua como una placa de presion que solo reacciona al cubo que hay que poner encima.
    void OnTriggerEnter(Collider other)
    {
        if(other.name == "CubeToMove")
        {
            leversPuzzleDone = true;
            other.GetComponent<Rigidbody>().isKinematic = true;
        } 
    }
}
