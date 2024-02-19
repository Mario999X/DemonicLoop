using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

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

    private LibraryMove library;

    // General information
    private TextMeshProUGUI postBattleText;

    // Imagenes Team
    private Image unit1;
    private Image unit2;
    private Image unit3;
    private Image unit4;

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
            library = GetComponent<LibraryMove>();

            LocateInterface();
            HideInterfacePostBattle();
            
            done = true;
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Scene 2")
        {
            done = false;
        }

        //Debug.Log(character + " " + oldAttackSelected);

    }

    private void LocateInterface()
    {
        postBattleTeamPanel = GameObject.Find("PostBattleTeamPanel");
        alliesBattleZone = GameObject.Find("AlliesBattleZone");

        postBattleText = postBattleTeamPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

        // Imagenes Team
        unit1 = postBattleTeamPanel.transform.GetChild(1).gameObject.GetComponent<Image>();
        unit2 = postBattleTeamPanel.transform.GetChild(2).gameObject.GetComponent<Image>();
        unit3 = postBattleTeamPanel.transform.GetChild(3).gameObject.GetComponent<Image>();
        unit4 = postBattleTeamPanel.transform.GetChild(4).gameObject.GetComponent<Image>();

        // Info Panels
        infoNewStatsPanel1 = postBattleTeamPanel.transform.GetChild(5).gameObject;
        infoNewStatsPanel2 = postBattleTeamPanel.transform.GetChild(6).gameObject;
        infoNewStatsPanel3 = postBattleTeamPanel.transform.GetChild(7).gameObject;
        infoNewStatsPanel4 = postBattleTeamPanel.transform.GetChild(8).gameObject;

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


        /* actualAttacksPanel = postBattleTeamPanel.transform.GetChild(6).gameObject;
         dontLearnAttackBtn = postBattleTeamPanel.transform.GetChild(4).gameObject;
         learnAttackBtn = postBattleTeamPanel.transform.GetChild(5).gameObject;*/
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
                    unit1.sprite = imagePlayer.sprite;
                    unit1Text.text = "Name: " + character.GetComponent<Stats>().name;
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
                    unit2.sprite = imagePlayer.sprite;
                    unit2Text.text = "Name: " + character.GetComponent<Stats>().name;
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
                    unit3.sprite = imagePlayer.sprite;
                    unit3Text.text = "Name: " + character.GetComponent<Stats>().name;
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
                    unit4.sprite = imagePlayer.sprite;
                    unit4Text.text = "Name: " + character.GetComponent<Stats>().name;
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
        // foreach (Transform button in actualAttacksPanel.transform) Destroy(button.gameObject);
        Debug.Log("postBattleTeamPanel "+ postBattleTeamPanel);
        postBattleTeamPanel.SetActive(false);
        infoNewStatsPanel1.SetActive(false);
        infoNewStatsPanel2.SetActive(false);
        infoNewStatsPanel3.SetActive(false);
        infoNewStatsPanel4.SetActive(false);
        unit1.gameObject.SetActive(false);
        unit2.gameObject.SetActive(false);
        unit3.gameObject.SetActive(false);
        unit4.gameObject.SetActive(false);
        //learnAttackBtn.GetComponent<Button>().interactable = false;

        // Parecia necesario, pero no lo es y evita un bug de null reference

        //character = null;
        //newAttack = null;
        //oldAttackSelected = null; 
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