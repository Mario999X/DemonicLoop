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
    private bool firstCharacterListCheck = false;
    private bool secondCharacterListCheck = false;
    private bool switchPositionsInBattle = false;
   
    private Scene scene;

    private StatsPersistenceData firstCharacterSelected = null;
    private StatsPersistenceData secondCharacterSelected = null;
    private EnterBattle enterBattle;

    // Update is called once per frame
    void Update()
    {
        if (scene != UnityEngine.SceneManagement.SceneManager.GetActiveScene())
        {
            done = false;
            scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }

        // Configura y guarda los componentes.
        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Title" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "VideoScene")
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

            GameObject.Find("TeamBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Scene 2") ShowOnlyActiveTeamView();
                else ShowCompleteTeamView();
            });

            teamViewScreen.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => ShowCompleteTeamView());

            done = true;
        }
    }

    // Muestra solo el equipo actual.
    public void ShowOnlyActiveTeamView()
    {
        if(teamViewScreen.GetComponent<Canvas>().enabled == false)
        {
            teamViewScreen.GetComponent<Canvas>().enabled = true;
            controlPanelBacklogTeamPods.transform.parent.gameObject.SetActive(false);

            Time.timeScale = 0f;

        } else
        {
            teamViewScreen.GetComponent<Canvas>().enabled = false;
            HideCharacterDetails();

            ResetBothCharacterSelected();

            Time.timeScale = 1f;
        } 
    }

    // Muestra el equipo actual y los que estuvieran en el banquillo.
    public void ShowCompleteTeamView()
    {
        if(teamViewScreen.GetComponent<Canvas>().enabled == false)
        {
            teamViewScreen.GetComponent<Canvas>().enabled = true;

            Time.timeScale = 0f;
        } else
        {
            teamViewScreen.GetComponent<Canvas>().enabled = false;
            
            HideCharacterDetails();
            ResetBothCharacterSelected();

            Time.timeScale = 1f;
        }
    }

    // Muestra los datos del equipo actual.
    public void SetActiveTeamData()
    {
        // Elimina los ya existentes para evitar duplicados y actualizarlo a los nuevos datos.
        if(controlPanelActiveTeamPods.transform.childCount > 0)
            foreach(Transform child in controlPanelActiveTeamPods.transform) Destroy(child.gameObject);

        // Genera el equipo.
        Data.Instance.CharactersTeamStats.ForEach(x => {
            var go = Instantiate(characterActiveTeamPod, controlPanelActiveTeamPods.transform.position, Quaternion.identity, controlPanelActiveTeamPods.transform);
            var pod = go.transform.GetChild(0);

            var sprite = x.CharacterPB.transform.GetChild(3);
            var spritePod = pod.GetChild(0);
        
            var spriteGo = Instantiate(sprite, spritePod.transform.position, Quaternion.identity, spritePod.transform);
            if (spriteGo.name != "demonio_menor_3(Clone)") spriteGo.transform.localScale = new Vector3(20f, 15f, 1f);
            else spriteGo.transform.localScale = new Vector3(300f, 150f, 1f);
            spriteGo.transform.localPosition = Vector3.zero;

            pod.GetChild(1).GetComponent<TextMeshProUGUI>().text = x.CharacterPB.name;
            pod.GetChild(2).GetComponent<TextMeshProUGUI>().text = "HP: " + x.Health + "/" + x.MaxHealth;
            pod.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Mana: " + x.Mana + "/" + x.MaxMana;
            
            go.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { ViewCharacterDetails(x); });
            go.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { SelectCharacter(x, true); });
        });
    }

    // Muestra los datos de lo que se encuentran en el banquillo.
    public void SetBackupTeamData()
    {
        // Elimina los ya existentes para evitar duplicados y actualizarlo a los nuevos datos.
        if (controlPanelBacklogTeamPods.transform.childCount > 0)
            foreach(Transform child in controlPanelBacklogTeamPods.transform) Destroy(child.gameObject);

        // Genera el banquillo.
        Data.Instance.CharactersBackupStats.ForEach(x => {
            var go = Instantiate(characterBacklogTeamPod, controlPanelBacklogTeamPods.transform.position, Quaternion.identity, controlPanelBacklogTeamPods.transform);

            var pod = go.transform.GetChild(0);

            var sprite = x.CharacterPB.transform.GetChild(3);
            var spritePod = pod.GetChild(0);
        
            var spriteGo = Instantiate(sprite, spritePod.transform.position, Quaternion.identity, spritePod.transform);
            if (spriteGo.name != "demonio_menor_3(Clone)") spriteGo.transform.localScale = new Vector3(20f, 15f, 1f);
            else spriteGo.transform.localScale = new Vector3(200f, 150f, 1f);
            spriteGo.transform.localPosition = Vector3.zero;

            pod.GetChild(1).GetComponent<TextMeshProUGUI>().text = x.CharacterPB.name;

            go.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { ViewCharacterDetails(x); });
            go.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { SelectCharacter(x, false); });
        });
    }

    // Cambia las posiciones tanto del equipo, como del banquillo.
    private void SelectCharacter(StatsPersistenceData character, bool activeTeam)
    {
        if(firstCharacterSelected != null)
        {
            if(character == firstCharacterSelected)
                firstCharacterSelected = null;
            else
            {
                secondCharacterSelected = character;
                secondCharacterListCheck = activeTeam;
            } 
        } else
        {
            firstCharacterSelected = character;
            firstCharacterListCheck = activeTeam;
        } 

        if(firstCharacterSelected != null && secondCharacterSelected != null)
           SwitchPositions();
    }

    // Hace el intercambio teniendo en cuenta su posicion.
    private void SwitchPositions()
    {
        if(firstCharacterListCheck && secondCharacterListCheck)
        {
            Data.Instance.SwitchActiveTeamPositions(firstCharacterSelected, secondCharacterSelected);

            SetActiveTeamData();
            switchPositionsInBattle = true;

            if(switchPositionsInBattle && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "SceneName 2")
            {
                StartCoroutine(GetComponent<EnterBattle>().RespawnAlliesInBattle());
                switchPositionsInBattle = false;
            } else
                switchPositionsInBattle = false;

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

        ResetBothCharacterSelected();
    }

    // Deselecciona los seleccionados para un intercambio.
    private void ResetBothCharacterSelected()
    {
        firstCharacterSelected = null;
        secondCharacterSelected = null;
    }

    // Genera la visualizacion del aliado seleccionado.
    private void ViewCharacterDetails(StatsPersistenceData character)
    {
        var sprite = character.CharacterPB.transform.GetChild(3);
        var spriteImage = characterDetailsView.transform.GetChild(0);
        
        var spriteGo = Instantiate(sprite, spriteImage.transform.position, Quaternion.identity, spriteImage.transform);
        
        // Ajusta la escala de la imagen.
        if (spriteGo.name != "demonio_menor_3(Clone)") spriteGo.transform.localScale = new Vector3(20f, 15f, 1f);
        else spriteGo.transform.localScale = new Vector3(200f, 150f, 1f);
        
        spriteGo.transform.localPosition = new Vector3(0f, -150f, 0f);

        // Estadisticas.
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

        // Lista de ataques.
        character.ListAtk.ForEach(a => {

            var attackName = a.name[4..].Replace("^", " ");

            var attackGO = Instantiate(attackTextDetailsView, actualAttackListDetails.transform.position, Quaternion.identity, actualAttackListDetails.transform);

            attackGO.GetComponent<TextMeshProUGUI>().text = attackName;
        });

        var charRolIconDetails = characterDetailsView.transform.GetChild(1).gameObject;

        GameObject charRolIconGO = new();
        
        // Imagen del rol.
        Image charRolIcon = charRolIconGO.AddComponent<Image>();

        charRolIcon.sprite = character.CharacterPB.GetComponent<Stats>().RolIcon;
        charRolIconGO.transform.SetParent(charRolIconDetails.transform);
        charRolIconGO.transform.position = charRolIconDetails.transform.position;
        charRolIconGO.transform.localScale = new Vector3(1.5f,1.5f,1f);

        var charTypeIconDetails = characterDetailsView.transform.GetChild(2).gameObject;

        GameObject charTypeIconGO = new();

        // Imagen de tipo.
        Image charTypeIcon = charTypeIconGO.AddComponent<Image>();
        
        charTypeIcon.sprite = character.CharacterPB.GetComponent<Stats>().TypeIcon;
        charTypeIconGO.transform.SetParent(charTypeIconDetails.transform);
        charTypeIconGO.transform.position = charTypeIconDetails.transform.position;
        charTypeIconGO.transform.localScale = new Vector3(1.5f,1.5f,1f);

        characterDetailsView.SetActive(true);
    }

    // Oculta la informacion del aliado seleccionado previamente.
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
