using UnityEngine;

public enum ModifierEffects { PhysicalDefense, MagicDefense, PhysicalAttack, MagicAttack, CriticalChance }

[CreateAssetMenu]
public class BattleModifiers : ScriptableObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private float baseEffectNumber;
    [SerializeField] private bool isADebuff;
    [SerializeField] private ModifierEffects modifierEffect;
    [SerializeField] [Range(1,5)] private int turnDuration;

    public Sprite Icon { get { return icon; } }
    public float BaseEffectNumber { get { return baseEffectNumber; } }
    public bool IsADebuff { get { return isADebuff ; } }
    public ModifierEffects ModifierEffect { get { return modifierEffect; } }
    public int TurnDuration { get { return turnDuration; } }
}

