using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeamViewManager : MonoBehaviour
{
    [SerializeField] GameObject characterActiveTeamPod;
    [SerializeField] GameObject characterBacklogTeamPod;
    [SerializeField] GameObject attackTextDetailsView;

    GameObject teamViewScreen;

    GameObject controlPanelActiveTeamPods;

    GameObject controlPanelBacklogTeamPods;

    GameObject characterDetailsView;
    GameObject characterStatsGroupDetails;

    private bool done = false;
    private Scene scene;

    private StatsPersistenceData firstCharacterSelected = null;
    private bool firstCharacterListCheck = false;
    private StatsPersistenceData secondCharacterSelected = null;
    private bool secondCharacterListCheck = false;

    private bool switchPositionsInBattle = false;

    private EnterBattle enterBattle;

    // Update is called once per frame
    void Update()
    {
        if (scene != UnityEngine.SceneManagement.SceneManager.GetActiveScene())
        {
            done = false;
            scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }

        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Title")
        {
            teamViewScreen = GameObject.Find("TeamViewScreen");
            controlPanelActiveTeamPods = GameObject.Find("ControlPanelActiveTeamPods");
            controlPanelBacklogTeamPods = GameObject.Find("ControlPanelBacklogTeamPodsContent");

            characterDetailsView = teamViewScreen.transform.GetChild(1).gameObject;

            characterStatsGroupDetails = characterDetailsView.transform.GetChild(6).gameObject;

            characterDetailsView.transform.GetChild(8).GetComponent<Button>().onClick.AddListener(delegate {HideCharacterDetails(); });

            SetActiveTeamData();
            SetBackupTeamData();

            enterBattle = GetComponent<EnterBattle>();

            characterDetailsView.SetActive(false);

            done = true;
        }

        if(Input.GetKeyDown(KeyCode.E) && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Scene 2" && !enterBattle.OneTime)
        {
            if(teamViewScreen.GetComponent<Canvas>().enabled == false)
            {
                teamViewScreen.GetComponent<Canvas>().enabled = true;
                controlPanelBacklogTeamPods.transform.parent.gameObject.SetActive(false);

            } else
            {
                teamViewScreen.GetComponent<Canvas>().enabled = false;
                HideCharacterDetails();
            } 
        }

        if(Input.GetKeyDown(KeyCode.E) && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Shop")
        {
            if(teamViewScreen.GetComponent<Canvas>().enabled == false)
            {
                teamViewScreen.GetComponent<Canvas>().enabled = true;

            } else
            {
                teamViewScreen.GetComponent<Canvas>().enabled = false;
                HideCharacterDetails();
            } 
        }
    }


    public void SetActiveTeamData()
    {
        if(controlPanelActiveTeamPods.transform.childCount > 0)
        {
            foreach(Transform child in controlPanelActiveTeamPods.transform) Destroy(child.gameObject);
        }

        Data.Instance.CharactersTeamStats.ForEach(x => {
            var go = Instantiate(characterActiveTeamPod, controlPanelActiveTeamPods.transform.position, Quaternion.identity, controlPanelActiveTeamPods.transform);

            var pod = go.transform.GetChild(0);

            var sprite = x.CharacterPB.transform.GetChild(3);
            var spritePod = pod.GetChild(0);
        
            var spriteGo = Instantiate(sprite, spritePod.transform.position, Quaternion.identity, spritePod.transform);
            spriteGo.transform.localPosition = Vector3.zero;

            pod.GetChild(1).GetComponent<TextMeshProUGUI>().text = x.CharacterPB.name;
        
            pod.GetChild(2).GetComponent<TextMeshProUGUI>().text = "HP: " + x.Health + "/" + x.MaxHealth;
            pod.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Mana: " + x.Mana + "/" + x.MaxMana;
            
            go.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { ViewCharacterDetails(x); });
            go.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { SelectCharacter(x, true); });
        });
    }

    public void SetBackupTeamData()
    {
        if(controlPanelBacklogTeamPods.transform.childCount > 0)
        {
            foreach(Transform child in controlPanelBacklogTeamPods.transform) Destroy(child.gameObject);
        }

        Data.Instance.CharactersBackupStats.ForEach(x => {
            var go = Instantiate(characterBacklogTeamPod, controlPanelBacklogTeamPods.transform.position, Quaternion.identity, controlPanelBacklogTeamPods.transform);

            var pod = go.transform.GetChild(0);

            var sprite = x.CharacterPB.transform.GetChild(3);
            var spritePod = pod.GetChild(0);
        
            var spriteGo = Instantiate(sprite, spritePod.transform.position, Quaternion.identity, spritePod.transform);
            spriteGo.transform.localPosition = Vector3.zero;

            pod.GetChild(1).GetComponent<TextMeshProUGUI>().text = x.CharacterPB.name;

            go.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { ViewCharacterDetails(x); });
            go.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { SelectCharacter(x, false); });
        });
    }

    private void SelectCharacter(StatsPersistenceData character, bool activeTeam)
    {
        if(firstCharacterSelected != null)
        {
            if(character == firstCharacterSelected)
            {
                firstCharacterSelected = null;

                Debug.Log("Mismo personaje seleccionado " + firstCharacterSelected);
            }
            else
            {
                secondCharacterSelected = character;
                secondCharacterListCheck = activeTeam;

                Debug.Log("Segundo personaje seleccionado " + secondCharacterSelected);
            } 

        } else
        {
            firstCharacterSelected = character;
            firstCharacterListCheck = activeTeam;

            Debug.Log("Primer personaje seleccionado " + firstCharacterSelected);
        } 

        if(firstCharacterSelected != null && secondCharacterSelected != null)
        {
            Debug.Log("Cambiando posiciones en el equipo");

           SwitchPositions();
        }
    }

    private void SwitchPositions()
    {
        if(firstCharacterListCheck && secondCharacterListCheck)
        {
            Data.Instance.SwitchActiveTeamPositions(firstCharacterSelected, secondCharacterSelected);

            SetActiveTeamData();

            switchPositionsInBattle = true;

            if(switchPositionsInBattle && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Scene 2")
            {
                Debug.Log("Cambiando posiciones en batalla");

                StartCoroutine(GetComponent<EnterBattle>().RespawnAlliesInBattle());
                
                switchPositionsInBattle = false;

            } else
            {
                Debug.Log("No es necesario cambiar posiciones en batalla");
                switchPositionsInBattle = false;
            }

        } else if(!firstCharacterListCheck && !secondCharacterListCheck)
        {
            Data.Instance.SwitchBackupTeamPositions(firstCharacterSelected, secondCharacterSelected);

            SetBackupTeamData();
        } else if(firstCharacterListCheck && !secondCharacterListCheck)
        {
            Data.Instance.SwitchBetweenTeamPositionsFirstCharActiveTeam(firstCharacterSelected, secondCharacterSelected);

            SetActiveTeamData();
            SetBackupTeamData();
        } else 
        {
            Data.Instance.SwitchBetweenTeamPositionsFirstCharBackupTeam(firstCharacterSelected, secondCharacterSelected);

            SetActiveTeamData();
            SetBackupTeamData();
        }

        firstCharacterSelected = null;
        secondCharacterSelected = null;
    }

    private void ViewCharacterDetails(StatsPersistenceData character)
    {
        var sprite = character.CharacterPB.transform.GetChild(3);
        var spriteImage = characterDetailsView.transform.GetChild(0);
        
        var spriteGo = Instantiate(sprite, spriteImage.transform.position, Quaternion.identity, spriteImage.transform);
        spriteGo.transform.localPosition = Vector3.zero;

        characterDetailsView.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = character.CharacterPB.name;
        characterDetailsView.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "LVL: " + character.Level;
        characterDetailsView.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "Actual XP: " + character.CurrentXP;

        characterStatsGroupDetails.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "HP: " + character.Health + "/" + character.MaxHealth;
        characterStatsGroupDetails.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Mana: " + character.Mana + "/" + character.MaxMana;
        characterStatsGroupDetails.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "SP: " + character.SP + "/100";
        characterStatsGroupDetails.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Strenght: " + character.Strenght;
        characterStatsGroupDetails.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Physical Defense: " + character.PhysicalDefense;
        characterStatsGroupDetails.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "Magic Attack: " + character.MagicAtk;
        characterStatsGroupDetails.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "Magic Defense: " + character.MagicDef;
        characterStatsGroupDetails.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = "Critical Chance: " + character.CriticalChance;

        var actualAttackListDetails = characterDetailsView.transform.GetChild(7).gameObject;

        character.ListAtk.ForEach(a => {

            var attackName = a.name[4..].Replace("^", " ");

            var attackGO = Instantiate(attackTextDetailsView, actualAttackListDetails.transform.position, Quaternion.identity, actualAttackListDetails.transform);

            attackGO.GetComponent<TextMeshProUGUI>().text = attackName;
        });

        var charRolIconDetails = characterDetailsView.transform.GetChild(1).gameObject;

        GameObject charRolIconGO = new();

        Image charRolIcon = charRolIconGO.AddComponent<Image>();
        charRolIcon.sprite = character.CharacterPB.GetComponent<Stats>().RolIcon;

        charRolIconGO.transform.SetParent(charRolIconDetails.transform);
        charRolIconGO.transform.position = charRolIconDetails.transform.position;
        charRolIconGO.transform.localScale = new Vector3(1.5f,1.5f,1f);


        var charTypeIconDetails = characterDetailsView.transform.GetChild(2).gameObject;

        GameObject charTypeIconGO = new();

        Image charTypeIcon = charTypeIconGO.AddComponent<Image>();
        charTypeIcon.sprite = character.CharacterPB.GetComponent<Stats>().TypeIcon;

        charTypeIconGO.transform.SetParent(charTypeIconDetails.transform);
        charTypeIconGO.transform.position = charTypeIconDetails.transform.position;
        charTypeIconGO.transform.localScale = new Vector3(1.5f,1.5f,1f);

        characterDetailsView.SetActive(true);
    }

    private void HideCharacterDetails()
    {
        var oldSprite = characterDetailsView.transform.GetChild(0);
        foreach(Transform child in oldSprite) Destroy(child.gameObject);

        var actualAttackListDetails = characterDetailsView.transform.GetChild(7);
        foreach(Transform child in actualAttackListDetails) Destroy(child.gameObject);

        var charRolIconDetails = characterDetailsView.transform.GetChild(1).gameObject;
        foreach(Transform child in charRolIconDetails.transform) Destroy(child.gameObject);

        var charTypeIconDetails = characterDetailsView.transform.GetChild(2).gameObject;
        foreach(Transform child in charTypeIconDetails.transform) Destroy(child.gameObject);

        characterDetailsView.SetActive(false);
    }
}
