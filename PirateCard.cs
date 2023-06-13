using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PirateCard : Card
{
    [SerializeField]
    private TMP_Text costText;
    [SerializeField]
    private int costDoubloons;
    public int DoubloonCost 
    {
        get 
        {
            return costDoubloons;
        }
        set
        {
            if (value < 0) costDoubloons = 0;
            else costDoubloons = value;

            costText.text = costDoubloons.ToString();
        }
    }

    protected override void PlaceCard(Placeholder placeholder)
    {
        if(costDoubloons > BattleController.instance.CurrentDoubloons) return;
        else
        {
            isInHand = false;

            MoveToPoint(placeholder.transform.position, Quaternion.identity);
            placeholder.activeCard = this;
            handController.RemovecCardFromHand(this);
            
            BattleController.instance.CurrentDoubloons -= costDoubloons;
        }
        
    }
}
