using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LearningAttacksManager : MonoBehaviour
{
    private GameObject learningAttacksPanel;

    private GameObject infoNewAttackPanel;
    private GameObject infoOldAttackPanel;

    private GameObject character;

    private Scene scene;
    private bool done = false;

    // General information
    private TextMeshProUGUI unitNameText;
    private TextMeshProUGUI newAttackNameText;

    // Texts about the new Attack
    private TextMeshProUGUI baseDamageTextNA;
    private TextMeshProUGUI typeTextNA;
    private TextMeshProUGUI magicOrPhysicalTextNA;
    private TextMeshProUGUI aoeTextNA;
    private TextMeshProUGUI manaCostTextNA;
    private TextMeshProUGUI berserkerAttackTextNA;
    private TextMeshProUGUI stateAsociatedTextNA;
    private TextMeshProUGUI battleModifierTextNA;

    // Texts about the old selected Attack
    private TextMeshProUGUI baseDamageAttackText;
    private TextMeshProUGUI typeText;
    private TextMeshProUGUI magicOrPhysicalText;
    private TextMeshProUGUI aoeText;
    private TextMeshProUGUI manaCostText;
    private TextMeshProUGUI berserkerAttackText;
    private TextMeshProUGUI stateAsociatedText;
    private TextMeshProUGUI battleModifierText;


    private void Update()
    {
        if (scene != UnityEngine.SceneManagement.SceneManager.GetActiveScene())
        {
            done = false;
            scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }

        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Scene 2")
        {
            LocateInterface();
            HideInterface();
            
            done = true;
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Scene 2")
        {
            done = false;
        }
    }

    private void HideInterface()
    {
        infoOldAttackPanel.SetActive(false);
        learningAttacksPanel.SetActive(false);
    }

    private void LocateInterface()
    {
        learningAttacksPanel = GameObject.Find("LearningAttacksPanel");

        infoNewAttackPanel = learningAttacksPanel.transform.GetChild(2).gameObject;
        infoOldAttackPanel = learningAttacksPanel.transform.GetChild(3).gameObject;

        unitNameText = learningAttacksPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        newAttackNameText = learningAttacksPanel.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();

        baseDamageTextNA = infoNewAttackPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        typeTextNA = infoNewAttackPanel.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        magicOrPhysicalTextNA = infoNewAttackPanel.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        aoeTextNA = infoNewAttackPanel.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        manaCostTextNA = infoNewAttackPanel.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
        berserkerAttackTextNA = infoNewAttackPanel.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>();
        stateAsociatedTextNA = infoNewAttackPanel.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>();
        battleModifierTextNA = infoNewAttackPanel.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>();


        baseDamageAttackText = infoOldAttackPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        typeText = infoOldAttackPanel.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        magicOrPhysicalText = infoOldAttackPanel.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        aoeText = infoOldAttackPanel.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        manaCostTextNA = infoOldAttackPanel.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
        berserkerAttackText = infoOldAttackPanel.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>();
        stateAsociatedText = infoOldAttackPanel.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>();
        battleModifierText = infoOldAttackPanel.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void ActiveDesactivePanel()
    {
        if(learningAttacksPanel.activeSelf)
        {
            character = null;

            learningAttacksPanel.SetActive(false);
        }

        if(!learningAttacksPanel.activeSelf)
        {
            learningAttacksPanel.SetActive(true);
        } 
    }

    public void SetNewAttackInfo(GameObject characterAsociated, AttackData newAttack)
    {
        ActiveDesactivePanel();

        character = characterAsociated;
        unitNameText.text = character.name;
        newAttackNameText.text = newAttack.name;

        baseDamageTextNA.text = newAttack.BaseDamage.ToString();
        typeTextNA.text = newAttack.Type.ToString();

        if(newAttack.PhyAttack == 1 && newAttack.MagicAttack == 0) magicOrPhysicalTextNA.text = "Physical";

        if(newAttack.PhyAttack == 0 && newAttack.MagicAttack == 1) magicOrPhysicalTextNA.text = "Magic";

        if(newAttack.PhyAttack == 0 && newAttack.MagicAttack == 0) magicOrPhysicalTextNA.text = "Heal";

        if(newAttack.IsAoeAttack)
        {
            aoeTextNA.text = "Is AOE";

        } else aoeTextNA.text = "Is NOT AOE";

        manaCostTextNA.text = newAttack.ManaCost.ToString();

        if(newAttack.Berserker)
        {
            berserkerAttackTextNA.text = "Is Berserker";

        } else berserkerAttackTextNA.text = "Is NOT Berserker";

        switch (newAttack.GenerateAState)
        {
            case ActionStates.None:
                stateAsociatedTextNA.text = "No state asociated";
            break;

            case ActionStates.Heal:
                stateAsociatedTextNA.text = "Heal State" + newAttack.StateGenerated;
            break;

            case ActionStates.Inflict:
                stateAsociatedTextNA.text = "Inflict State" + newAttack.StateGenerated + " with probability: " + newAttack.ProbabilityOfState;
            break;
        }

        if(newAttack.BattleModifierAsociated != null)
        {
            battleModifierTextNA.text = "Inflict battle modifier" + newAttack.BattleModifierAsociated.ToString();

        } else battleModifierTextNA.text = "No battle modifier asociated";
    }



}
