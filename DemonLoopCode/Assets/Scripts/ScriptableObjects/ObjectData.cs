using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ObjectData : ScriptableObject
{
    [SerializeField] private Texture icon;
    [TextArea]
    [SerializeField] private string description;
}
