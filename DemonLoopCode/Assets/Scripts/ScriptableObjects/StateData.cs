using UnityEngine;

[CreateAssetMenu]
public class StateData : ScriptableObject
{
    [SerializeField] float baseDamage;
    [SerializeField] float timeMoving;
    [SerializeField] int turnsDuration;

    public float BaseDamage { get {  return baseDamage; } }
    public float TimeMoving { get {  return timeMoving; } }
    public float TurnsDuration { get {  return turnsDuration; } }
}
