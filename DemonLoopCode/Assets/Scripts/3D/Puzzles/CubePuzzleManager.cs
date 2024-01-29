using UnityEngine;

public class CubePuzzleManager : MonoBehaviour
{
    private bool leversPuzzleDone = false;

    public bool LeversPuzzleDone { get { return leversPuzzleDone;} }

    void OnTriggerEnter(Collider other)
    {
        if(other.name == "CubeToMove")
        {
            leversPuzzleDone = true;
            other.GetComponent<Rigidbody>().isKinematic = true;
        } 
    }
}
