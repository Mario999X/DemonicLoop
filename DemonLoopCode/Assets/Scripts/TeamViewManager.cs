using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeamViewManager : MonoBehaviour
{
    [SerializeField] GameObject characterActiveTeamPod;
    [SerializeField] GameObject characterBacklogTeamPod;

    GameObject teamViewScreen;

    GameObject controlPanelActiveTeamPods;

    GameObject controlPanelBacklogTeamPods;

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

            SetActiveTeamData();
            SetBackupTeamData();

            enterBattle = GetComponent<EnterBattle>();

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
}
