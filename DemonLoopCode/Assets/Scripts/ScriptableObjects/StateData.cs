using UnityEngine;

[CreateAssetMenu]
public class StateData : ScriptableObject
{
    [SerializeField] Sprite icon;
    [SerializeField] float baseDamage;
    [SerializeField] float timeMoving;
    [SerializeField] int turnsDuration;

    public Sprite Icon { get { return icon; } set { icon = value; } }
    public float BaseDamage { get {  return baseDamage; } }
    public float TimeMoving { get {  return timeMoving; } }
    public float TurnsDuration { get {  return turnsDuration; } }
}
