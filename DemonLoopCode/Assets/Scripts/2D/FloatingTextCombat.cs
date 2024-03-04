using TMPro;
using UnityEngine;

// Clase encargada de instanciar y eliminar informacion durante un combate, como daños aplicados o textos.
public class FloatingTextCombat : MonoBehaviour
{
    [SerializeField] private GameObject floatingTextPF;
    [SerializeField] private float destroyTime;

    public void ShowFloatingTextNumbers(GameObject character, float number, Color color)
    {
        var go = Instantiate(floatingTextPF, character.transform.position, Quaternion.identity, character.GetComponent<Stats>().CharFloatingTextSpaceNumbers.transform);

        var textComponent = go.GetComponent<TextMeshProUGUI>();

        textComponent.text = ((int)number).ToString();

        textComponent.color = color;

        Destroy(go, destroyTime);
    }

    public void ShowFloatingText(GameObject character, string text, Color color)
    {
         var go = Instantiate(floatingTextPF, character.transform.position, Quaternion.identity, character.GetComponent<Stats>().CharFloatingTextSpaceNumbers.transform);

        var textComponent = go.GetComponent<TextMeshProUGUI>();

        textComponent.text = text;

        textComponent.color = color;

        Destroy(go, destroyTime);
    }
}
