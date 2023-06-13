using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public List<Card> heldCards = new List<Card>();
    public List<Vector3> cardPositions = new List<Vector3>();
    public List<Quaternion> cardQuaternion = new List<Quaternion>();
    public Transform minPos, maxPos, midPos;  
    private Vector3 distanceBetweenPositions = Vector3.zero;
    private float stepRotation = 0f;      
    public static HandController instance;

    private void Awake() 
    {
        instance = this;
    }

    void Start()
    {
        SetCardPosInHands();
    }

    public void SetCardPosInHands()
    {
        cardPositions.Clear();
        cardQuaternion.Clear();

        int bound = heldCards.Count/2;

        if(heldCards.Count > 0 && heldCards.Count < 5)
        {
            distanceBetweenPositions = new Vector3(0.97f, midPos.position.y - minPos.position.y, midPos.position.z - minPos.position.z);
            stepRotation = (maxPos.rotation.y - minPos.rotation.y) / (heldCards.Count - 1);
        }
        else if(heldCards.Count >= 5) //calculate distance
        {
            distanceBetweenPositions = (midPos.position - minPos.position) / (heldCards.Count/2);
            stepRotation = (maxPos.rotation.y - minPos.rotation.y) / (heldCards.Count - 1);
        }
        

        Vector3 oppositeDistance = new Vector3(distanceBetweenPositions.x, -distanceBetweenPositions.y, -distanceBetweenPositions.z);        

        for(int i = bound; i > 0; i--) //setPosition
        {            
            cardPositions.Add(midPos.position - distanceBetweenPositions*i);
        }

        if(heldCards.Count % 2 == 0)
        {
            for(int i = 1; i <= bound; i++) //setPosition
            {            
                cardPositions.Add(midPos.position + oppositeDistance*i);
            }
        }
        else
        {
            for(int i = 0; i <= bound; i++) //setPosition
            {            
                cardPositions.Add(midPos.position + oppositeDistance*i);
            }

        }

        for(int i = 1; i <= bound; i++) //setPosition
        {            
            cardPositions.Add(midPos.position + oppositeDistance*i);
        }

        for(int i = 0; i < heldCards.Count; i++) //setRotaion
        {            
            cardQuaternion.Add(new Quaternion(minPos.rotation.x, (minPos.rotation.y + stepRotation*i), minPos.rotation.z + 0.05f, 1)); 
        }

        for(int i = 0; i < heldCards.Count; i++) //move Cards to positions
        {
            Card card = heldCards[i];
            card.MoveToPoint(cardPositions[i], cardQuaternion[i]);

            card.isInHand = true;
            card.positionInHand = i;
        }        
    }

    public void RemovecCardFromHand(Card cardToRemove)
    {
        heldCards.RemoveAt(cardToRemove.positionInHand);
        SetCardPosInHands();
    }

    public void AddCardToHand(Card card)
    {
        heldCards.Add(card);
        SetCardPosInHands();
    }
}
