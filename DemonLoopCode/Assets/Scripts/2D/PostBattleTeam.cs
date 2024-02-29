using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PostBattleTeam : MonoBehaviour
{
    GameObject postBattleTeamPanel;

    private GameObject infoNewStatsPanel1;
    private GameObject infoNewStatsPanel2;
    private GameObject infoNewStatsPanel3;
    private GameObject infoNewStatsPanel4;

    private GameObject character = null;

    private Scene scene;
    private bool done = false;

    // Imagenes Team
    private GameObject unit1;
    private GameObject unit2;
    private GameObject unit3;
    private GameObject unit4;

    // Texts about the new Stats unit1
    private TextMeshProUGUI unit1Text;
    private TextMeshProUGUI lvL1Text;
    private TextMeshProUGUI hp1Text;
    private TextMeshProUGUI mana1Text;
    private TextMeshProUGUI sp1Text;
    private TextMeshProUGUI strenght1Text; //fuerza
    private TextMeshProUGUI physicalDefense1Text;
    private TextMeshProUGUI magicAtk1Text;
    private TextMeshProUGUI magicDef1Text;
    private TextMeshProUGUI criticalChance1Text;


    // Texts about the new Stats unit2
    private TextMeshProUGUI unit2Text;
    private TextMeshProUGUI lvL2Text;
    private TextMeshProUGUI hp2Text;
    private TextMeshProUGUI mana2Text;
    private TextMeshProUGUI sp2Text;
    private TextMeshProUGUI strenght2Text; //fuerza
    private TextMeshProUGUI physicalDefense2Text;
    private TextMeshProUGUI magicAtk2Text;
    private TextMeshProUGUI magicDef2Text;
    private TextMeshProUGUI criticalChance2Text;

    // Texts about the new Stats unit3
    private TextMeshProUGUI unit3Text;
    private TextMeshProUGUI lvL3Text;
    private TextMeshProUGUI hp3Text;
    private TextMeshProUGUI mana3Text;
    private TextMeshProUGUI sp3Text;
    private TextMeshProUGUI strenght3Text; //fuerza
    private TextMeshProUGUI physicalDefense3Text;
    private TextMeshProUGUI magicAtk3Text;
    private TextMeshProUGUI magicDef3Text;
    private TextMeshProUGUI criticalChance3Text;

    // Texts about the new Stats unit4
    private TextMeshProUGUI unit4Text;
    private TextMeshProUGUI lvL4Text;
    private TextMeshProUGUI hp4Text;
    private TextMeshProUGUI mana4Text;
    private TextMeshProUGUI sp4Text;
    private TextMeshProUGUI strenght4Text; //fuerza
    private TextMeshProUGUI physicalDefense4Text;
    private TextMeshProUGUI magicAtk4Text;
    private TextMeshProUGUI magicDef4Text;
    private TextMeshProUGUI criticalChance4Text;


    GameObject alliesBattleZone;
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
            HideInterfacePostBattle();
            
            done = true;
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Scene 2")
        {
            done = false;
        }


    }

    private void LocateInterface()
    {
        postBattleTeamPanel = GameObject.Find("PostBattleTeamPanel");
        alliesBattleZone = GameObject.Find("AlliesBattleZone");

        // Imagenes Team
        unit1 = GameObject.Find("Unit1");
        unit2 = GameObject.Find("Unit2");
        unit3 = GameObject.Find("Unit3");
        unit4 = GameObject.Find("Unit4");

        // Info Panels
        infoNewStatsPanel1 = postBattleTeamPanel.transform.GetChild(4).gameObject;
        infoNewStatsPanel2 = postBattleTeamPanel.transform.GetChild(5).gameObject;
        infoNewStatsPanel3 = postBattleTeamPanel.transform.GetChild(6).gameObject;
        infoNewStatsPanel4 = postBattleTeamPanel.transform.GetChild(7).gameObject;

        // Unidad 1

        unit1Text = infoNewStatsPanel1.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        lvL1Text = infoNewStatsPanel1.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        hp1Text = infoNewStatsPanel1.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        mana1Text = infoNewStatsPanel1.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        sp1Text = infoNewStatsPanel1.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
        strenght1Text = infoNewStatsPanel1.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>();
        physicalDefense1Text = infoNewStatsPanel1.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>();
        magicAtk1Text = infoNewStatsPanel1.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>();
        magicDef1Text = infoNewStatsPanel1.transform.GetChild(8).gameObject.GetComponent<TextMeshProUGUI>();
        criticalChance1Text = infoNewStatsPanel1.transform.GetChild(9).gameObject.GetComponent<TextMeshProUGUI>();


        // Unidad 2

        unit2Text = infoNewStatsPanel2.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        lvL2Text = infoNewStatsPanel2.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        hp2Text = infoNewStatsPanel2.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        mana2Text = infoNewStatsPanel2.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        sp2Text = infoNewStatsPanel2.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
        strenght2Text = infoNewStatsPanel2.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>();
        physicalDefense2Text = infoNewStatsPanel2.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>();
        magicAtk2Text = infoNewStatsPanel2.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>();
        magicDef2Text = infoNewStatsPanel2.transform.GetChild(8).gameObject.GetComponent<TextMeshProUGUI>();
        criticalChance2Text = infoNewStatsPanel2.transform.GetChild(9).gameObject.GetComponent<TextMeshProUGUI>();


        // Unidad 3

        unit3Text = infoNewStatsPanel3.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        lvL3Text = infoNewStatsPanel3.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        hp3Text = infoNewStatsPanel3.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        mana3Text = infoNewStatsPanel3.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        sp3Text = infoNewStatsPanel3.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
        strenght3Text = infoNewStatsPanel3.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>();
        physicalDefense3Text = infoNewStatsPanel3.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>();
        magicAtk3Text = infoNewStatsPanel3.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>();
        magicDef3Text = infoNewStatsPanel3.transform.GetChild(8).gameObject.GetComponent<TextMeshProUGUI>();
        criticalChance3Text = infoNewStatsPanel3.transform.GetChild(9).gameObject.GetComponent<TextMeshProUGUI>();

        // Unidad 4

        unit4Text = infoNewStatsPanel4.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        lvL4Text = infoNewStatsPanel4.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        hp4Text = infoNewStatsPanel4.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        mana4Text = infoNewStatsPanel4.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        sp4Text = infoNewStatsPanel4.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
        strenght4Text = infoNewStatsPanel4.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>();
        physicalDefense4Text = infoNewStatsPanel4.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>();
        magicAtk4Text = infoNewStatsPanel4.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>();
        magicDef4Text = infoNewStatsPanel4.transform.GetChild(8).gameObject.GetComponent<TextMeshProUGUI>();
        criticalChance4Text = infoNewStatsPanel4.transform.GetChild(9).gameObject.GetComponent<TextMeshProUGUI>();

        /////////////////////////////////////////////////////////////////////////////////////////////////////
    }


    public IEnumerator InfoPanelTeam(GameObject[] players)
    {

        for (int i = 0; i < players.Length; i++)
        {
            GameObject playerPrefab=alliesBattleZone.transform.GetChild(i).gameObject.transform.GetChild(3).gameObject;
            Image imagePlayer = playerPrefab.GetComponent<Image>();

            Debug.Log("id: " + i+" nombre: " + players[i].name);
            switch (i)
            {
                case 0:
                    infoNewStatsPanel1.SetActive(true);
                    unit1.gameObject.SetActive(true);
                   
                    character = players[0];
                    var spriteGoPlayers1 = Instantiate(playerPrefab, unit1.transform.position, Quaternion.identity, unit1.transform);
                    unit1Text.text = "Name: " + character.name.Remove(character.name.IndexOf("("));
                    lvL1Text.text = "Lvl: " + character.GetComponent<Stats>().Level;
                    hp1Text.text = "Max HP: " + character.GetComponent<Stats>().MaxHealth;
                    mana1Text.text = "Max Mana: " + character.GetComponent<Stats>().MaxMana;
                    sp1Text.text = "Max SP: " + character.GetComponent<Stats>().MaxSP;
                    strenght1Text.text = "Strenght: " + character.GetComponent<Stats>().Strenght; //fuerza
                    physicalDefense1Text.text = "Physical Defense: " + character.GetComponent<Stats>().PhysicalDefense;
                    magicAtk1Text.text = "Magic ATK: " + character.GetComponent<Stats>().MagicAtk;
                    magicDef1Text.text = "Magic Def: " + character.GetComponent<Stats>().MagicDef;
                    criticalChance1Text.text = "Critical Chance: " + character.GetComponent<Stats>().CriticalChance;
                    break;

                case 1:
                    infoNewStatsPanel2.SetActive(true);
                    unit2.gameObject.SetActive(true);

                    character = players[1];
                    var spriteGoPlayers2 = Instantiate(playerPrefab, unit2.transform.position, Quaternion.identity, unit2.transform);
                    unit2Text.text = "Name: " + character.name.Remove(character.name.IndexOf("("));
                    lvL2Text.text = "Lvl: " + character.GetComponent<Stats>().Level;
                    hp2Text.text = "Max HP: " + character.GetComponent<Stats>().MaxHealth;
                    mana2Text.text = "Max Mana: " + character.GetComponent<Stats>().MaxMana;
                    sp2Text.text = "Max SP: " + character.GetComponent<Stats>().MaxSP;
                    strenght2Text.text = "Strenght: " + character.GetComponent<Stats>().Strenght; //fuerza
                    physicalDefense2Text.text = "Physical Defense: " + character.GetComponent<Stats>().PhysicalDefense;
                    magicAtk2Text.text = "Magic ATK: " + character.GetComponent<Stats>().MagicAtk;
                    magicDef2Text.text = "Magic Def: " + character.GetComponent<Stats>().MagicDef;
                    criticalChance2Text.text = "Critical Chance: " + character.GetComponent<Stats>().CriticalChance;
                    break;

                case 2:
                    infoNewStatsPanel3.SetActive(true);
                    unit3.gameObject.SetActive(true);

                    character = players[2];
                    var spriteGoPlayers3 = Instantiate(playerPrefab, unit3.transform.position, Quaternion.identity, unit3.transform);
                    unit3Text.text = "Name: " + character.name.Remove(character.name.IndexOf("("));
                    lvL3Text.text = "Lvl: " + character.GetComponent<Stats>().Level;
                    hp3Text.text = "Max HP: " + character.GetComponent<Stats>().MaxHealth;
                    mana3Text.text = "Max Mana: " + character.GetComponent<Stats>().MaxMana;
                    sp3Text.text = "Max SP: " + character.GetComponent<Stats>().MaxSP;
                    strenght3Text.text = "Strenght: " + character.GetComponent<Stats>().Strenght; //fuerza
                    physicalDefense3Text.text = "Physical Defense: " + character.GetComponent<Stats>().PhysicalDefense;
                    magicAtk3Text.text = "Magic ATK: " + character.GetComponent<Stats>().MagicAtk;
                    magicDef3Text.text = "Magic Def: " + character.GetComponent<Stats>().MagicDef;
                    criticalChance3Text.text = "Critical Chance: " + character.GetComponent<Stats>().CriticalChance;
                    break;

                case 3:
                    infoNewStatsPanel4.SetActive(true);
                    unit4.gameObject.SetActive(true);

                    character = players[3];
                    var spriteGoPlayers4 = Instantiate(playerPrefab, unit4.transform.position, Quaternion.identity, unit4.transform);
                    unit4Text.text = "Name: " + character.name.Remove(character.name.IndexOf("("));
                    lvL4Text.text = "Lvl: " + character.GetComponent<Stats>().Level;
                    hp4Text.text = "Max HP: " + character.GetComponent<Stats>().MaxHealth;
                    mana4Text.text = "Max Mana: " + character.GetComponent<Stats>().MaxMana;
                    sp4Text.text = "Max SP: " + character.GetComponent<Stats>().MaxSP;
                    strenght4Text.text = "Strenght: " + character.GetComponent<Stats>().Strenght; //fuerza
                    physicalDefense4Text.text = "Physical Defense: " + character.GetComponent<Stats>().PhysicalDefense;
                    magicAtk4Text.text = "Magic ATK: " + character.GetComponent<Stats>().MagicAtk;
                    magicDef4Text.text = "Magic Def: " + character.GetComponent<Stats>().MagicDef;
                    criticalChance4Text.text = "Critical Chance: " + character.GetComponent<Stats>().CriticalChance;
                    break;
            }
        }

        yield return new WaitForSeconds(3);
        HideInterfacePostBattle();
    }

    public void HideInterfacePostBattle()
    {

        postBattleTeamPanel.SetActive(false);
        infoNewStatsPanel1.SetActive(false);
        infoNewStatsPanel2.SetActive(false);
        infoNewStatsPanel3.SetActive(false);
        infoNewStatsPanel4.SetActive(false);
        unit1.gameObject.SetActive(false);
        unit2.gameObject.SetActive(false);
        unit3.gameObject.SetActive(false);
        unit4.gameObject.SetActive(false);

    }


    public void ActivatePanel()
    {
        postBattleTeamPanel.SetActive(true);
        infoNewStatsPanel1.SetActive(true);
        infoNewStatsPanel2.SetActive(true);
        infoNewStatsPanel3.SetActive(true);
        infoNewStatsPanel4.SetActive(true);
    }


}
