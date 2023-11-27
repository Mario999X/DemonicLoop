using TMPro;
using UnityEngine;

public class FloatingTextCombat : MonoBehaviour
{
    [SerializeField] GameObject floatingTextPF;
    [SerializeField] float destroyTime;

    public void ShowFloatingTextNumbers(GameObject character, float number, Color color)
    {
        var go = Instantiate(floatingTextPF, character.transform.position, Quaternion.identity, character.GetComponent<Stats>().CharFloatingTextSpaceNumbers.transform);

        var textComponent = go.GetComponent<TextMeshProUGUI>();

        textComponent.text = ((int)number).ToString();

        textComponent.color = color;

        Destroy(go, destroyTime);
    }
}
