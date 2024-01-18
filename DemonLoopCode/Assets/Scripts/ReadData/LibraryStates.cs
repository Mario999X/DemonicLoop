using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActualStateData
{
    private GameObject character = null;
    private string state;
    private int turn = 0;

    public string State {  get { return state; } }
    public int Turn { get { return turn; } set { turn = value; } }
    public GameObject Character {  get { return character; } }

    public ActualStateData(string state, GameObject character)
    {
        this.state = state;
        this.character = character;
    }

    public ActualStateData(string state)
    {
        this.state = state;
    }
}

public class LibraryStates : MonoBehaviour
{
    [SerializeField] GameObject Burnt;
    [SerializeField] GameObject Poison;
    private Dictionary<string, StateData> states = new();

    private List<ActualStateData> characterStates = new();

    public List<ActualStateData> CharacterStates { get { return characterStates;} }

    private FloatingTextCombat floatingText;

    private PlayerMove player;
    private EnterBattle enterBattle;
    DamageVisualEffect damageVisualEffect;

    bool done = false;

    GameObject[] party;

    Scene scene;

    void Start()
    {
        enterBattle = GetComponent<EnterBattle>();
        floatingText = GetComponent<FloatingTextCombat>();
            
        LoadStates();
    }

    private void FixedUpdate()
    {
        if (scene != UnityEngine.SceneManagement.SceneManager.GetActiveScene())
        {
            done = false;
            scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }

        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Scene 2")
        {
            player = GameObject.Find("Player").GetComponent<PlayerMove>();
            damageVisualEffect = GameObject.Find("Global Volume").GetComponent<DamageVisualEffect>();

            done = true;
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Scene 2")
        {
            done = false;
        }


        if (party == null || party.Length == 0)
            party = GameObject.FindGameObjectsWithTag("Player");
        else
        {
            bool stateDetacted = false;

            foreach (ActualStateData state in characterStates)
            {
                    if (party.Contains(state.Character))
                        stateDetacted = true;
            }

            if (!enterBattle.OneTime && stateDetacted)
            {
                damageVisualEffect.Auch();
            }
        }
    }

    private void LoadStates()
    {
        StateData[] stateDatas = Resources.LoadAll<StateData>("Data/States");

        foreach (StateData stateData in stateDatas)
        {
            states.Add(stateData.name.Substring(4, stateData.name.Length - 4).ToUpper(), stateData);
        }
    }

    public IEnumerator StateEffectIndividual(GameObject target, string state){

        if(states.ContainsKey(state.ToUpper()))
        {
            ActualStateData actualState = new(state.ToUpper(), target);
            characterStates.Add(actualState);

            var targetST = target.GetComponent<Stats>();

            var stateData = states[state.ToUpper()];

            float time = 0;
            int lastTurn = 0;

            do
            {
                if (!enterBattle.OneTime) // 3D individual
                {
                    if (player.Movement)
                    {
                        time += Time.deltaTime;
                    }

                    if (time >= stateData.TimeMoving)
                    {
                        if (targetST.Health > 1)
                            {
                                var targetHealthAfterDamage = targetST.Health - stateData.BaseDamage;

                                if (targetHealthAfterDamage < 1)
                                {
                                    targetST.Health = 1;
                                } else targetST.Health = targetHealthAfterDamage;
                            }

                        actualState.Turn++;
                        time = 0;
                    }
                }

                else // 2D individual
                {

                    if (lastTurn != actualState.Turn)
                    {

                        IconState();

                        if (targetST.Health > 0)
                        {
                            targetST.Health -= stateData.BaseDamage;

                            floatingText.ShowFloatingTextNumbers(target, -stateData.BaseDamage, Color.magenta);

                            if(targetST.Health <= 0)
                                {
                                    if(targetST.gameObject.CompareTag("Enemy")) GameObject.Find("System").GetComponent<CombatFlow>().DeleteEnemyFromList(targetST.gameObject);
                                    else GameObject.Find("System").GetComponent<CombatFlow>().DeleteAllieFromArray(targetST.gameObject);

                                    actualState.Turn = 100; // Finaliza el bucle de los hilos.
                                }
                        }
                        
                        lastTurn = actualState.Turn;
                    }
                }

                yield return new WaitForSeconds(0.000000001f);
            } while (actualState.Turn <= stateData.TurnsDuration);

            characterStates.Remove(actualState);
        }
    }

    public IEnumerator StateEffectGroup(string group, string state)
    {
        if (states.ContainsKey(state.ToUpper()))
        {
            ActualStateData actualState = new(state.ToUpper());
            characterStates.Add(actualState);

            Stats[] stats = GameObject.Find(group).GetComponentsInChildren<Stats>();
            StateData stateData = states[state.ToUpper()];
            float time = 0;
            int lastTurn = -1;

            do
            {
                if (!enterBattle.OneTime) // 3D grupo
                {
                    if (player.Movement)
                    {
                        time += Time.deltaTime;
                    }

                    if (time >= stateData.TimeMoving)
                    {
                        foreach (Stats character in stats)
                        {
                            if (character.Health > 1)
                            {
                                var targetHealthAfterDamage = character.Health - stateData.BaseDamage;

                                if (targetHealthAfterDamage < 1)
                                {
                                    character.Health = 1;
                                } else character.Health = targetHealthAfterDamage;
                            }
                        }

                        actualState.Turn++;
                        time = 0;
                    }
                }
                else // 2D grupo
                {
                    IconState();

                    if (lastTurn != actualState.Turn)
                    {
                        foreach (Stats character in stats)
                        {

                            if (character.Health > 0)
                            {
                                character.Health -= stateData.BaseDamage;

                                floatingText.ShowFloatingTextNumbers(character.gameObject, -stateData.BaseDamage, Color.magenta);

                                if(character.Health <= 0)
                                {
                                    if(character.gameObject.CompareTag("Enemy")) GameObject.Find("System").GetComponent<CombatFlow>().DeleteEnemyFromList(character.gameObject);
                                    else GameObject.Find("System").GetComponent<CombatFlow>().DeleteAllieFromArray(character.gameObject);

                                    actualState.Turn = 100; // Finaliza el bucle de los hilos.
                                }
                            }
                        }

                        lastTurn = actualState.Turn;
                    }
                }

                yield return new WaitForSeconds(0.000000001f);
            } while (actualState.Turn <= stateData.TurnsDuration);

            characterStates.Remove(actualState);
        }
    }

    // Esta funcion se encarga de buscar las entidades con un estado y les pone el icono de estado.
    public void IconState()
    {
        foreach (ActualStateData actualState in characterStates)
        {
            if (actualState.State.ToUpper() == "BURNT")
            {
                var iconExists = actualState.Character.transform.Find("Burnt(Clone)");

                if(iconExists == null)
                {
                    //Debug.Log("BURNT " + actualState.State.ToUpper());

                    GameObject icon = Instantiate(Burnt, actualState.Character.transform.position, Quaternion.identity);
                    icon.transform.SetParent(actualState.Character.transform);

                    icon.transform.localScale = new Vector3(1f, 1f, 1f);
                }
            }
            else
            {
                var iconExists = actualState.Character.transform.Find("Poison(Clone)");

                if(iconExists == null)
                {
                    //Debug.Log("POISON " + actualState.State.ToUpper());
                
                    GameObject icon = Instantiate(Poison, actualState.Character.transform.position, Quaternion.identity);
                    icon.transform.SetParent(actualState.Character.transform);
                    
                    icon.transform.localScale = new Vector3(1f, 1f, 1f);
                }
            }
        }
    }

    public void RemoveCharacterWithState(GameObject character, string state)
    {
        if(states.ContainsKey(state.ToUpper()))
        {
            var stateCharacter = characterStates.Find(x => x.Character == character && x.State == state);

            if(stateCharacter != null)
            {
  
                Debug.Log("Character found with that State, removing...");
                stateCharacter.Turn = 100; // Finaliza el bucle de los hilos.
                characterStates.Remove(stateCharacter);
            } else Debug.Log("Character not found with that State");
        }
    }

    public void ResetTurnsOfCharacterState(GameObject character, string state)
    {
        if(states.ContainsKey(state.ToUpper()))
        {
            var stateCharacter = characterStates.Find(x => x.Character == character && x.State == state);

            if(stateCharacter != null)
            {
                stateCharacter.Turn = 0;
            } else Debug.Log("Character not found with that State");
        }
    }

    public bool CheckStatus(GameObject target, string state)
    {
        //Recorrer toda la lista de los estados guardados
        foreach (ActualStateData character in characterStates)
        {
            string stateLimp = state.Replace("(Clone)", "").Trim();

            //Debug.Log("stateLimp " + stateLimp);
            if (character.State.ToUpper() == stateLimp.ToUpper() && character.Character == target)
            {
                //Debug.Log("Sigue con el estado");
                return true;
            }
        }

        return false;
    }

}
