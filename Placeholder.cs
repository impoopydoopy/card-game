using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeholder : MonoBehaviour
{
    public Card activeCard;
    public bool isPlayerPoint;
    private SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void OnMouseEnter() {
        spriteRenderer.color = Color.white;
    }

    private void OnMouseExit() {
        spriteRenderer.color = Color.grey;
    }
    
}
