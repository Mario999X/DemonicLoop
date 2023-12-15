using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconStated : MonoBehaviour
{
    LibraryStates states;


    private void Start()
    {
        states=GameObject.Find("System").GetComponent<LibraryStates>();

    }

    private void Update()
    {
        if (!states.checkStatus(gameObject.transform.parent.gameObject, name)) Destroy(gameObject);
    }




}
