using UnityEngine;

public class IconStated : MonoBehaviour
{
    LibraryStates states;


    private void Start()
    {
        //states = GameObject.Find("System").GetComponent<LibraryStates>();

    }

    private void Update()
    {
        //if (!states.CheckStatus(gameObject.transform.parent.gameObject, name)) gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        //Destroy(gameObject);
    }


}
