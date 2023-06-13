using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public List<CardScriptableObject> useDeck = new List<CardScriptableObject>();
    public List<CardScriptableObject> activeDeck = new List<CardScriptableObject>();
    public Card cardToSpawn;
    public Transform spawnpoint;

    private void Awake() 
    {
        SetUpDeck();
    }

    public void SetUpDeck()
    {
        activeDeck.Clear(); 

        List<CardScriptableObject> tempDeck = new List<CardScriptableObject>();
        tempDeck.AddRange(useDeck);

        int iterations = 0;

        while(tempDeck.Count > 0 && iterations < 26)
        {
            int selected = UnityEngine.Random.Range(0, tempDeck.Count);

            activeDeck.Add(tempDeck[selected]);
            tempDeck.RemoveAt(selected);

            iterations++;
        }        
    }

    public void DrawCard()
    {
        //Debug.Log("0");
        if(activeDeck.Count == 0) return;

        AudioManager.instance.PlaySFX(3);

        Card newCard = Instantiate(cardToSpawn, spawnpoint.position, spawnpoint.rotation);
        newCard.cardSO = activeDeck[0];
        newCard.SetUpCard();

        activeDeck.RemoveAt(0);
        HandController.instance.AddCardToHand(newCard);
    }

    private void OnMouseOver() {
        if(Input.GetMouseButtonDown(0))
            DrawCard();
    }

    public Card SpawnCard(Vector3 position, Quaternion rotation){
        Card newCard = Instantiate(cardToSpawn, spawnpoint.position, spawnpoint.rotation);
        newCard.cardSO = activeDeck[0];
        newCard.SetUpCard();
        activeDeck.RemoveAt(0);
        newCard.MoveToPoint(position, rotation);

        return newCard;
    }
}
