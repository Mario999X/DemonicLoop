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
    
    public ActualBattleModifiers(){}
    
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
    private BattleModifierIconsCombat battleModifierIconsCombat;

    private void Start()
    {
        LoadBattleModifiers();
        battleModifierIconsCombat = GetComponent<BattleModifierIconsCombat>();
    }

    // Cargamos los modificadores de la batalla
    private void LoadBattleModifiers()
    {
        BattleModifiers[] battleModifiers = Resources.LoadAll<BattleModifiers>("Data/BattleModifiers");
        foreach (BattleModifiers bm in battleModifiers)
        {
            battleModifiersCache.Add(bm.name, bm);
        }
    }

    // Activamos los modificadores en la batalla de los personajes
    public void ActiveBattleModifier(GameObject target, BattleModifiers modifier)
    {
        // Buscamos si el dificador ya esya activo
        var activeBattleModifier = SearchActiveBattleActiveModifier(target, modifier);

        // En el caso de que esta ya activo, actualiza el contador de turnos
        if(activeBattleModifier != null)
        {
            activeBattleModifier.ActualTurn = 0;
        } 
        else
        {
            // En el caso de que no se agrega el modificador
            battleModifierIconsCombat.ShowBattleModifierIcon(target, modifier);
            ActualBattleModifiers actualModifier = new();

            var targetST = target.GetComponent<Stats>();
            var buffOrDebuff = "+";
            if(modifier.IsADebuff)
            {
                buffOrDebuff = "-";
            }
            var effectToApply = buffOrDebuff + modifier.BaseEffectNumber;

            // Aplicar el efecto del modificador segun su tipo
            switch(modifier.ModifierEffect)
            {
                case ModifierEffects.PhysicalDefense:
                    targetST.PhysicalDefense += int.Parse(effectToApply);
                    if(targetST.PhysicalDefense < 0)
                    {   
                        var storedStatTemp = targetST.PhysicalDefense;
                        targetST.PhysicalDefense = 0;
                        actualModifier = new(modifier.name, target.GetComponent<Stats>(), true, storedStatTemp);

                    } else actualModifier = new(modifier.name, target.GetComponent<Stats>());
                break;

                case ModifierEffects.MagicDefense:
                    targetST.MagicDef += int.Parse(effectToApply);
                    if(targetST.MagicDef < 0)
                    {
                        var storedStatTemp = targetST.MagicDef;
                        targetST.MagicDef = 0;
                        actualModifier = new(modifier.name, target.GetComponent<Stats>(), true, storedStatTemp);

                    } else actualModifier = new(modifier.name, target.GetComponent<Stats>());
                break;

                case ModifierEffects.PhysicalAttack:
                    targetST.Strenght += int.Parse(effectToApply);
                    if(targetST.Strenght < 0)
                    {
                        var storedStatTemp = targetST.Strenght;
                        targetST.Strenght = 0;
                        actualModifier = new(modifier.name, target.GetComponent<Stats>(), true, storedStatTemp);

                    } else actualModifier = new(modifier.name, target.GetComponent<Stats>());
                break;

                case ModifierEffects.MagicAttack:
                    targetST.MagicAtk += int.Parse(effectToApply);
                    if(targetST.MagicAtk < 0)
                    {
                        var storedStatTemp = targetST.MagicAtk;
                        targetST.MagicAtk = 0;
                        actualModifier = new(modifier.name, target.GetComponent<Stats>(), true, storedStatTemp);

                    } else actualModifier = new(modifier.name, target.GetComponent<Stats>());
                break;

                case ModifierEffects.CriticalChance:
                    targetST.CriticalChance += int.Parse(effectToApply);
                    if(targetST.CriticalChance < 0)
                    {
                        var storedStatTemp = targetST.CriticalChance;
                        targetST.CriticalChance = 0;
                        actualModifier = new(modifier.name, target.GetComponent<Stats>(), true, storedStatTemp);

                    } else actualModifier = new(modifier.name, target.GetComponent<Stats>());
                break;
            }
            // Agregamos el modificador activo a la lista de modificadores activos
            battleModifiersActive.Add(actualModifier);
        }
    }

    // Pasa turno de los modificadores
    public void PassTurnOfModifiers()
    {
        // Incrementa el contador de turnos del modificador activo
        for(int i = 0; i < battleModifiersActive.Count; i++)
        {
            // Icrementar el contador de tunos del modificador
            battleModifiersActive[i].ActualTurn++;

            // Obtiene los datos del modificador
            var modifierData = battleModifiersCache[battleModifiersActive[i].ModifierName];

            // Comprobamos si los datos han llegado a los turnos maximos
            if(battleModifiersActive[i].ActualTurn >= modifierData.TurnDuration)
            {
                var modifier = battleModifiersActive[i];
                var targetST = battleModifiersActive[i].Character;

                // Eliminamos el icono del modificador de la parte de la interfaz del combate
                battleModifierIconsCombat.DeleteBattleModifierIcon(targetST.transform.gameObject, modifierData);

                // Determinamos si el modificador es un debuff o un buff
                var buffOrDebuffReturnToNormal = "-";

                if(modifierData.IsADebuff)
                {
                    buffOrDebuffReturnToNormal = "+";
                }

                var returnStat = battleModifiersActive[i].IsStatLessThanZero;
                var effectToApply = buffOrDebuffReturnToNormal + modifierData.BaseEffectNumber;

                // Revertimos el efecto del modificador dependiendo de su tipo
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
                // Quitamos los modificadores
                battleModifiersActive.RemoveAt(i);
                i--;
            }
        }
    }

    // Removemos todos los modificadores de la batalla
    public void RemoveAllBattleModifiers()
    {
        for(int i = 0; i < battleModifiersActive.Count; i++)
        {   
            var modifierData = battleModifiersCache[battleModifiersActive[i].ModifierName];

            var modifier = battleModifiersActive[i];
            var targetST = battleModifiersActive[i].Character;

            // Elimina el icono del modificador de la interfaz del combate
            battleModifierIconsCombat.DeleteBattleModifierIcon(targetST.transform.gameObject, modifierData);

            // Se determina si es un modificador de un buff o un debuff 
            var buffOrDebuffReturnToNormal = "-";

            if(modifierData.IsADebuff)
            {
                buffOrDebuffReturnToNormal = "+";
            }

            var returnStat = battleModifiersActive[i].IsStatLessThanZero;

            var effectToApply = buffOrDebuffReturnToNormal + modifierData.BaseEffectNumber;

            // Revertimos el modificador segun su tipo
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
            // Quitamos los modificadores al final de la batalla
            battleModifiersActive.RemoveAt(i);
            i--;
        }
    }

    public ActualBattleModifiers SearchActiveBattleActiveModifier(GameObject target, BattleModifiers modifier)
    {
        ActualBattleModifiers battleModifierSearch = null;

        for (int i = 0; i < battleModifiersActive.Count; i++)
        {
            // Comprobamos que el nombre del modificador y
            // el nombre del objetivo coinciden con los del modificador acyivo
            if (modifier.name == battleModifiersActive[i].ModifierName && target.name == battleModifiersActive[i].Character.name)
            {
                battleModifierSearch = battleModifiersActive[i];
                break;
            } 
        }
        // Devuelve el modificador de batalla que puede llegar a ser null
        return battleModifierSearch;
    }

}

