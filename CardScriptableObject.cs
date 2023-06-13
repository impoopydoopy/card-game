using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 1)]
public class CardScriptableObject : ScriptableObject
{
    public string cardName;
    [TextArea]
    public string cardDescription;
    public int health, attack;

    public Sprite cardSprite;
}
