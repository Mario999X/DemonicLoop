using UnityEngine;
using UnityEngine.UI;

public class BattleModifierIconsCombat : MonoBehaviour
{

    public void ShowBattleModifierIcon(GameObject character, BattleModifiers battleModifier)
    {
        GameObject iconObject = new(character.name + battleModifier.Icon.name);
        
        Image iconImage = iconObject.AddComponent<Image>();

        iconImage.sprite = battleModifier.Icon;

        iconObject.transform.localScale = new(0.6f,0.6f,1f);

        iconObject.transform.SetParent(character.GetComponent<Stats>().CharFloatingBattleModifierIconSpace.transform);
    }

    public void DeleteBattleModifierIcon(GameObject character, BattleModifiers battleModifier)
    {
        var name = character.name + battleModifier.Icon.name;

        Destroy(GameObject.Find(name));
    }
}
