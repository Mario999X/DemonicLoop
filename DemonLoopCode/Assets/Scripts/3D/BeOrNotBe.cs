using UnityEngine;

public class BeOrNotBe : MonoBehaviour
{
    [Header("Porcentaje para que el objeto para aparecer")]
    [Range(1, 100)]
    [SerializeField] int rarity = 1;

    // Start is called before the first frame update
    void Start()
    {
        float precent = (float) rarity / 100;

        if (Random.value >= precent)
            Destroy(gameObject);
    }
}
