using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    public CardScriptableObject cardSO;
    private int health;
    public int Health 
    {
        get 
        {
            return health;
        }
        set
        {
            if (value <= 0) {                
                placeholder.activeCard = null;
                AudioManager.instance.PlaySFX(2);
                Destroy(gameObject);
            }
            else health = value;

            healthText.text = health.ToString();
        }
    }
    private int attack;
    public int Attack
    {
        get 
        {
            return attack;
        }
        set
        {
            if (attack < 0) attack = 0;
            else attack = value;

            attackText.text = attack.ToString();
        }
    }

    [SerializeField]
    private TMP_Text healthText, attackText, nameText, descriptionText;

    [SerializeField]
    private Image spriteCard;

    protected Vector3 targetPoint;
    protected Quaternion targetRotation;

    [SerializeField]
    private float moveSpeed = 5f, rotateSpeed = 540f;

    public bool isInHand = false, isSelected = false, isOverPlaceholder;
    public int positionInHand = -1;

    protected HandController handController;
    public Placeholder placeholder;
    private Collider col;

    public LayerMask DesktopLayer, PlacementLayer;
    
    
    void Start()
    {
        SetUpCard();
        handController = FindObjectOfType<HandController>();
        col = GetComponent<Collider>();
    }

    public void SetUpCard()
    {
        health = cardSO.health;
        attack = cardSO.attack;
        nameText.text = cardSO.cardName;
        descriptionText.text = cardSO.cardDescription;

        spriteCard.sprite = cardSO.cardSprite;

        healthText.text = health.ToString();
        attackText.text = attack.ToString();
    }
    
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed*Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        if(isSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100f, DesktopLayer))
            {
                MoveToPoint(hit.point + new Vector3(0f, 1f, 0), Quaternion.identity);
                isOverPlaceholder = false;
            }     

            if(Physics.Raycast(ray, out hit, 100f, PlacementLayer) && BattleController.instance.currentPhase == BattleController.TurnOrder.playerActive)
            {
                isOverPlaceholder = true;
                placeholder = hit.collider.GetComponent<Placeholder>();                
            }      
        }
    }

    public void MoveToPoint(Vector3 pointToMoveTo, Quaternion rotation)
    {
        targetPoint = pointToMoveTo;
        targetRotation = rotation;
    }

    protected virtual void PlaceCard(Placeholder placeholder)
    {
        AudioManager.instance.PlaySFX(4);
        isInHand = false;

        MoveToPoint(placeholder.transform.position, Quaternion.identity);
        placeholder.activeCard = this;
        handController.RemovecCardFromHand(this);
    }

    public void DamageCard(int damageAmount){        
        Health -= damageAmount;
        animator.SetTrigger("Hurt");
    }

    public void AttackCard(){
        AudioManager.instance.PlaySFX(1);
        animator.SetTrigger("Attack");
    }

    #region Mouse Interaction

    private void OnMouseOver() 
    {
        if(isInHand)
        {
            MoveToPoint(handController.cardPositions[positionInHand] + new Vector3(0f, .5f, .6f), handController.cardQuaternion[positionInHand]);
        }
    }

    private void OnMouseExit() 
    {
        if(isInHand)
        {
            MoveToPoint(handController.cardPositions[positionInHand], handController.cardQuaternion[positionInHand]);
        }
    }

    private void OnMouseDown() 
    {
        if(isInHand && BattleController.instance.currentPhase == BattleController.TurnOrder.playerActive) 
        {
            isSelected  = true;
            col.enabled = false;
        }
    }

    private void OnMouseUp() 
    {
        if(isInHand) 
        {
            isSelected  = false;
            col.enabled = true;
            if(isOverPlaceholder && placeholder.activeCard == null && placeholder.isPlayerPoint)
            {
                PlaceCard(placeholder);                
            }
            isOverPlaceholder = false;
        }
    }
    #endregion
}