using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Clase encargada de manejar el panel de aprendizaje de ataques. 
public class LearningAttacksManager : MonoBehaviour
{
    [SerializeField] private GameObject buttonRef;

    private GameObject learningAttacksPanel;

    private GameObject infoNewAttackPanel;
    private GameObject infoOldAttackPanel;

    private GameObject character = null;
    private AttackData newAttack = null;
    private AttackData oldAttackSelected = null;

    private GameObject dontLearnAttackBtn;
    private GameObject learnAttackBtn;

    private Scene scene;
    private bool done = false;

    private LibraryMove library;

    // General information
    private TextMeshProUGUI unitNameText;
    private TextMeshProUGUI newAttackNameText;
    private GameObject actualAttacksPanel;

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
    private TextMeshProUGUI baseDamageText;
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
            library = GetComponent<LibraryMove>();

            LocateInterface();
            HideInterface();
            
            done = true;
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Scene 2")
        {
            done = false;
        }

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


        baseDamageText = infoOldAttackPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        typeText = infoOldAttackPanel.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        magicOrPhysicalText = infoOldAttackPanel.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        aoeText = infoOldAttackPanel.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        manaCostText = infoOldAttackPanel.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
        berserkerAttackText = infoOldAttackPanel.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>();
        stateAsociatedText = infoOldAttackPanel.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>();
        battleModifierText = infoOldAttackPanel.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>();

        actualAttacksPanel = learningAttacksPanel.transform.GetChild(6).gameObject;
        dontLearnAttackBtn = learningAttacksPanel.transform.GetChild(4).gameObject;
        learnAttackBtn = learningAttacksPanel.transform.GetChild(5).gameObject;
    }


    public void HideInterface()
    {
        foreach(Transform button in actualAttacksPanel.transform) Destroy(button.gameObject);

        infoOldAttackPanel.SetActive(false);
        learningAttacksPanel.SetActive(false);
        learnAttackBtn.GetComponent<Button>().interactable = false;

    }

    public void FinishOperationNoNewAttack()
    {
        GetComponent<CombatFlow>().CheckIfMoreCharactersWantNewAttack();
    }

    public void ForgetOldAttackAndLearnNewAttack()
    {
        character.GetComponent<Stats>().ForgetAttack(oldAttackSelected);
        character.GetComponent<Stats>().SetAttack(newAttack);

        FinishOperationNoNewAttack();
    }

    public void ActivatePanel()
    {
        learningAttacksPanel.SetActive(true);
    }

    // Funcion para poner la informacion del ataque que se quiera olvidar sobre el panel correspondiente. 
    // Activamos el boton de olvidar y aprender con el ataque antiguo seleccionado. 
    private void SetOldAttackToForget(string movement)
    {
        oldAttackSelected = library.CheckAttack(movement);

        baseDamageText.text = "Base Damage: " + oldAttackSelected.BaseDamage;
        typeText.text = "Type: " + oldAttackSelected.Type.ToString();

        if(oldAttackSelected.PhyAttack == 1 && oldAttackSelected.MagicAttack == 0) magicOrPhysicalText.text = "Physical";

        if(oldAttackSelected.PhyAttack == 0 && oldAttackSelected.MagicAttack == 1) magicOrPhysicalText.text = "Magic";

        if(oldAttackSelected.PhyAttack == 0 && oldAttackSelected.MagicAttack == 0) magicOrPhysicalText.text = "Heal";

        if(oldAttackSelected.IsAoeAttack)
        {
            aoeText.text = "Is AOE";

        } else aoeText.text = "Is NOT AOE";

        manaCostText.text = "Mana cost: " + oldAttackSelected.ManaCost.ToString();

        if(oldAttackSelected.Berserker)
        {
            berserkerAttackText.text = "Is Berserker";

        } else berserkerAttackText.text = "Is NOT Berserker";

        switch (oldAttackSelected.GenerateAState)
        {
            case ActionStates.None:
                stateAsociatedText.text = "No state asociated";
            break;

            case ActionStates.Heal:
                stateAsociatedText.text = "Heal State: " + oldAttackSelected.StateGenerated;
            break;

            case ActionStates.Inflict:
                stateAsociatedText.text = "Inflict State " + oldAttackSelected.StateGenerated + " | Probability: " + oldAttackSelected.ProbabilityOfState;
            break;
        }

        if(oldAttackSelected.BattleModifierAsociated != null)
        {
            battleModifierText.text = "Battle modifier: " + oldAttackSelected.ObtainBattleModifierName();

        } else battleModifierText.text = "No battle modifier asociated";

        infoOldAttackPanel.SetActive(true);

        learnAttackBtn.GetComponent<Button>().interactable = true;
        learnAttackBtn.GetComponent<Button>().onClick.AddListener(delegate { ForgetOldAttackAndLearnNewAttack(); });
    }

    // Funcion para poner la informacion del ataque nuevo que se este tratando de aprender.
    public void SetNewAttackInfo(GameObject characterAsociated, AttackData newAttackAsociated)
    {
        character = characterAsociated;
        newAttack = newAttackAsociated;

        unitNameText.text = character.name.Remove(character.name.IndexOf("("));;
        newAttackNameText.text = newAttackAsociated.name.Substring(4, newAttackAsociated.name.Length - 4).Replace("^", " ").ToUpper();

        baseDamageTextNA.text = "Base Damage: " + newAttackAsociated.BaseDamage;
        typeTextNA.text = "Type: " + newAttackAsociated.Type.ToString();

        if(newAttackAsociated.PhyAttack == 1 && newAttackAsociated.MagicAttack == 0) magicOrPhysicalTextNA.text = "Physical";

        if(newAttackAsociated.PhyAttack == 0 && newAttackAsociated.MagicAttack == 1) magicOrPhysicalTextNA.text = "Magic";

        if(newAttackAsociated.PhyAttack == 0 && newAttackAsociated.MagicAttack == 0) magicOrPhysicalTextNA.text = "Heal";

        if(newAttackAsociated.IsAoeAttack)
        {
            aoeTextNA.text = "Is AOE";

        } else aoeTextNA.text = "Is NOT AOE";

        manaCostTextNA.text = "Mana cost: " + newAttackAsociated.ManaCost.ToString();

        if(newAttackAsociated.Berserker)
        {
            berserkerAttackTextNA.text = "Is Berserker";

        } else berserkerAttackTextNA.text = "Is NOT Berserker";

        switch (newAttackAsociated.GenerateAState)
        {
            case ActionStates.None:
                stateAsociatedTextNA.text = "No state asociated";
            break;

            case ActionStates.Heal:
                stateAsociatedTextNA.text = "Heal State: " + newAttackAsociated.StateGenerated;
            break;

            case ActionStates.Inflict:
                stateAsociatedTextNA.text = "Inflict State " + newAttackAsociated.StateGenerated + " | Probability: " + newAttackAsociated.ProbabilityOfState;
            break;
        }

        if(newAttackAsociated.BattleModifierAsociated != null)
        {
            battleModifierTextNA.text = "Battle modifier: " + newAttackAsociated.ObtainBattleModifierName();

        } else battleModifierTextNA.text = "No battle modifier asociated";

        characterAsociated.GetComponent<Stats>().ListNameAtk.ForEach(name => {
            GameObject bt = Instantiate(buttonRef, actualAttacksPanel.transform.position, Quaternion.identity);
            bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;

            bt.GetComponent<Button>().onClick.AddListener(delegate { SetOldAttackToForget(name); });
            bt.transform.SetParent(actualAttacksPanel.transform);

            bt.transform.localScale = new Vector3(1f,1f,1f);
        });

        dontLearnAttackBtn.GetComponent<Button>().onClick.AddListener(delegate { FinishOperationNoNewAttack(); });
    }

}
