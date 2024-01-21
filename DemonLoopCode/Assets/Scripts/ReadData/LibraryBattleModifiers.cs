using System.Collections.Generic;
using UnityEngine;

public class ActualBattleModifiers
{
    private Stats character = null;
    private string modifierName;
    private int actualTurn = 0;
    private bool isStatLessThanZero = false;
    private float storedStat = 0;

    public bool IsStatLessThanZero { get { return isStatLessThanZero; } set { isStatLessThanZero = value; }}
    public float StoredStat { get { return storedStat; } set { storedStat = value; }}
    public string ModifierName {  get { return modifierName; } }
    public int ActualTurn { get { return actualTurn; } set { actualTurn = value; } }
    public Stats Character {  get { return character; } }
    
    public ActualBattleModifiers()
    {

    }
    
    public ActualBattleModifiers(string modifierName, Stats character)
    {
        this.modifierName = modifierName;
        this.character = character;
    }

    public ActualBattleModifiers(string modifierName, Stats character, bool isStatLessThanZero, float storedStat)
    {
        this.modifierName = modifierName;
        this.character = character;
        this.isStatLessThanZero = isStatLessThanZero;
        this.storedStat = storedStat;
    }
}


public class LibraryBattleModifiers : MonoBehaviour
{   
    private Dictionary<string, BattleModifiers> battleModifiersCache = new();
    private List<ActualBattleModifiers> battleModifiersActive = new();

    private void Start()
    {
        LoadBattleModifiers();
    }

    private void LoadBattleModifiers()
    {
        BattleModifiers[] battleModifiers = Resources.LoadAll<BattleModifiers>("Data/BattleModifiers");

        foreach (BattleModifiers bm in battleModifiers)
        {
            //string modifierName = bm.name.Substring(4, bm.name.Length - 4).Replace("^", " ").ToUpper();

            battleModifiersCache.Add(bm.name, bm);
        }
    }

    public void ActiveBattleModifier(GameObject target, BattleModifiers modifier)
    {
        var activeBattleModifier = SearchActiveBattleActiveModifier(target, modifier);

        if(activeBattleModifier != null)
        {
            Debug.Log($"{target.name} ya cuenta con el modificador {modifier.name}");

            activeBattleModifier.ActualTurn = 0;

        } else
        {
            Debug.Log($"{target.name} se le agrega el modificador {modifier.name}");

            ActualBattleModifiers actualModifier = new();

            var targetST = target.GetComponent<Stats>();

            var buffOrDebuff = "+";
            if(modifier.IsADebuff)
            {
                buffOrDebuff = "-";
            }
            var effectToApply = buffOrDebuff + modifier.BaseEffectNumber;

            switch(modifier.ModifierEffect)
            {
                case ModifierEffects.PhysicalDefense:
                    targetST.PhysicalDefense += int.Parse(effectToApply);

                    
                    if(targetST.PhysicalDefense < 0)
                    {   
                        var storedStatTemp = targetST.PhysicalDefense;

                        //Debug.Log(targetST.PhysicalDefense);

                        targetST.PhysicalDefense = 0;
                        
                        actualModifier = new(modifier.name, target.GetComponent<Stats>(), true, storedStatTemp);

                    } else actualModifier = new(modifier.name, target.GetComponent<Stats>());
                break;

                case ModifierEffects.MagicDefense:
                    targetST.MagicDef += int.Parse(effectToApply);

                    if(targetST.MagicDef < 0)
                    {
                        var storedStatTemp = targetST.MagicDef;

                        //Debug.Log(targetST.MagicDef);

                        targetST.MagicDef = 0;
                        
                        actualModifier = new(modifier.name, target.GetComponent<Stats>(), true, storedStatTemp);

                    } else actualModifier = new(modifier.name, target.GetComponent<Stats>());
                break;

                case ModifierEffects.PhysicalAttack:
                    targetST.Strenght += int.Parse(effectToApply);

                    if(targetST.Strenght < 0)
                    {
                        var storedStatTemp = targetST.Strenght;

                        //Debug.Log(targetST.Strenght);

                        targetST.Strenght = 0;
                        
                        actualModifier = new(modifier.name, target.GetComponent<Stats>(), true, storedStatTemp);

                    } else actualModifier = new(modifier.name, target.GetComponent<Stats>());
                break;

                case ModifierEffects.MagicAttack:
                    targetST.MagicAtk += int.Parse(effectToApply);

                    if(targetST.MagicAtk < 0)
                    {
                        var storedStatTemp = targetST.MagicAtk;

                        //Debug.Log(targetST.MagicAtk);

                        targetST.MagicAtk = 0;
                        
                        actualModifier = new(modifier.name, target.GetComponent<Stats>(), true, storedStatTemp);

                    } else actualModifier = new(modifier.name, target.GetComponent<Stats>());
                break;

                case ModifierEffects.CriticalChance:
                    targetST.CriticalChance += int.Parse(effectToApply);

                    if(targetST.CriticalChance < 0)
                    {
                        var storedStatTemp = targetST.CriticalChance;

                        //Debug.Log(targetST.CriticalChance);

                        targetST.CriticalChance = 0;
                        
                        actualModifier = new(modifier.name, target.GetComponent<Stats>(), true, storedStatTemp);

                    } else actualModifier = new(modifier.name, target.GetComponent<Stats>());
                break;
            }

            battleModifiersActive.Add(actualModifier);
        }
        
    }

    public void PassTurnOfModifiers()
    {
        for(int i = 0; i < battleModifiersActive.Count; i++)
        {
            battleModifiersActive[i].ActualTurn++;

            var modifierData = battleModifiersCache[battleModifiersActive[i].ModifierName];

            if(battleModifiersActive[i].ActualTurn >= modifierData.TurnDuration)
            {
                var modifier = battleModifiersActive[i];
                var targetST = battleModifiersActive[i].Character;

                var buffOrDebuffReturnToNormal = "-";

                if(modifierData.IsADebuff)
                {
                    buffOrDebuffReturnToNormal = "+";
                }

                var returnStat = battleModifiersActive[i].IsStatLessThanZero;

                var effectToApply = buffOrDebuffReturnToNormal + modifierData.BaseEffectNumber;

                switch(modifierData.ModifierEffect)
                {
                    case ModifierEffects.PhysicalDefense:
                        targetST.PhysicalDefense += int.Parse(effectToApply);

                        //Debug.Log(effectToApply);

                        //Debug.Log(buffOrDebuffReturnToNormal);
                        
                        if(returnStat)
                        {
                            //Debug.Log(modifier.StoredStat);

                            targetST.PhysicalDefense += modifier.StoredStat;
                        }
                    break;

                    case ModifierEffects.MagicDefense:
                        targetST.MagicDef += int.Parse(effectToApply);

                        if(returnStat)
                        {
                            targetST.MagicDef += modifier.StoredStat;
                        }
                    break;

                    case ModifierEffects.PhysicalAttack:
                        targetST.Strenght += int.Parse(effectToApply);

                        if(returnStat)
                        {
                            targetST.Strenght += modifier.StoredStat;
                        }
                    break;

                    case ModifierEffects.MagicAttack:
                        targetST.MagicAtk += int.Parse(effectToApply);

                        if(returnStat)
                        {
                            targetST.MagicAtk += modifier.StoredStat;
                        }
                    break;

                    case ModifierEffects.CriticalChance:
                        targetST.CriticalChance += int.Parse(effectToApply);

                        if(returnStat)
                        {
                            targetST.CriticalChance += modifier.StoredStat;
                        }
                    break;
                }

                Debug.Log("Quitando modificador" + battleModifiersActive[i].ModifierName);
                battleModifiersActive.RemoveAt(i);
                i--;
            }
        }
    }

    public void RemoveAllBattleModifiers()
    {
        for(int i = 0; i < battleModifiersActive.Count; i++)
        {   
            var modifierData = battleModifiersCache[battleModifiersActive[i].ModifierName];

            var modifier = battleModifiersActive[i];
            var targetST = battleModifiersActive[i].Character;

            var buffOrDebuffReturnToNormal = "-";

            if(modifierData.IsADebuff)
            {
                buffOrDebuffReturnToNormal = "+";
            }

            var returnStat = battleModifiersActive[i].IsStatLessThanZero;

            var effectToApply = buffOrDebuffReturnToNormal + modifierData.BaseEffectNumber;

            switch(modifierData.ModifierEffect)
            {
                case ModifierEffects.PhysicalDefense:
                    targetST.PhysicalDefense += int.Parse(effectToApply);
                    
                    if(returnStat)
                    {
                        targetST.PhysicalDefense += modifier.StoredStat;
                    }
                break;

                case ModifierEffects.MagicDefense:
                    targetST.MagicDef += int.Parse(effectToApply);

                    if(returnStat)
                    {
                        targetST.MagicDef += modifier.StoredStat;
                    }
                break;

                case ModifierEffects.PhysicalAttack:
                    targetST.Strenght += int.Parse(effectToApply);

                    if(returnStat)
                    {
                        targetST.Strenght += modifier.StoredStat;
                    }
                break;

                case ModifierEffects.MagicAttack:
                    targetST.MagicAtk += int.Parse(effectToApply);

                    if(returnStat)
                    {
                        targetST.MagicAtk += modifier.StoredStat;
                    }
                break;

                case ModifierEffects.CriticalChance:
                    targetST.CriticalChance += int.Parse(effectToApply);

                    if(returnStat)
                    {
                        targetST.CriticalChance += modifier.StoredStat;
                    }
                break;
            }

            Debug.Log("Quitando modificador al final de batalla" + battleModifiersActive[i].ModifierName);
            battleModifiersActive.RemoveAt(i);
            i--;
        }
    }

    public ActualBattleModifiers SearchActiveBattleActiveModifier(GameObject target, BattleModifiers modifier)
    {
        ActualBattleModifiers battleModifierSearch = null;

        for (int i = 0; i < battleModifiersActive.Count; i++)
        {
            if(modifier.name == battleModifiersActive[i].ModifierName && target.name == battleModifiersActive[i].Character.name) battleModifierSearch = battleModifiersActive[i];
        }

        return battleModifierSearch;
    }

}

