using UnityEngine;

public class ExistoNoExisto : MonoBehaviour
{
    [Header("Porcentaje para que el objeto para aparecer")]
    [Range(1, 100)]
    [SerializeField] int rarety = 1;

    // Start is called before the first frame update
    void Start()
    {
        float precent = (float) rarety / 100;

        if (Random.value <= precent)
            Destroy(gameObject);
    }
}
