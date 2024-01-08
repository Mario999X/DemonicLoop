using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetDemo : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(temporal());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator temporal()
    {
        Data.Instance.Room=0;
        Data.Instance.SaveRoom = 0;
        Data.Instance.Floor = 0;
        Data.Instance.Money = 0f;
        yield return new WaitForSeconds(10f);
        SceneManager.Instance.LoadScene(0);

    }

}
