using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{

    public GameObject panelEndgame;

    [SerializeField]
    private DeckController playerDeck;

    [SerializeField]
    AudioManager manager;

    private int startDoubloons = 10, maxDoubloons = 20; //temp
    private int currentDoubloons;
    public int CurrentDoubloons 
    {
        get{
            return currentDoubloons;
        }
        set{
            if(value < 0) currentDoubloons = 0;
            else if (value > maxDoubloons) currentDoubloons = maxDoubloons;
            else currentDoubloons = value;
            
            doubloonsText.text = currentDoubloons.ToString();
        }
    } 
    private int startCardsAmount = 5;
    private float timeDelay = .2f;

    [SerializeField]
    private TMP_Text doubloonsText;
    [SerializeField]
    private TMP_Text playerHealthText;
    [SerializeField]
    private TMP_Text enemyHealthtext;

    public bool battleEnded = false;

    public TMP_Text result;

    public enum TurnOrder { playerActive, playerCardAttacks, enemyActive, enemyCardAttacks }
    public TurnOrder currentPhase;
    public Animator animator;

    [SerializeField]
    private int playerHealth;

    public int PlayerHealth{
        get 
        {
            return playerHealth;
        }
        set
        {
            if (value <= 0) {
                playerHealth = 0;
                battleEnded = true;
                result.text = "You are defeated. Mr. Lightwatcher won... Again.";
            }
            else {
                playerHealth = value;
            }
            playerHealthText.text = "Player's health: " + playerHealth.ToString();
        }
    }

    [SerializeField]
    private int enemyHealth;

    public int EnemyHealth{
        get{
            return enemyHealth;
        }
        set{
            if (value <= 0) {
                enemyHealth = 0;
                battleEnded = true;
                result.text = "You won! Mr. Lightwatcher is defeated...";
            }
            else {
                enemyHealth = value;
            }
            enemyHealthtext.text = "Enemy's health: " + enemyHealth.ToString();
        }
    }

    public static BattleController instance;

    private void Awake() 
    {
        instance = this;       
    }

    private void Start() {
        
        manager.PlayBGM();

        currentDoubloons = startDoubloons;
        doubloonsText.text = currentDoubloons.ToString();

        PlayerHealth = playerHealth;
        EnemyHealth = enemyHealth;

        StartCoroutine(DrawStartCards(startCardsAmount));
    }

    IEnumerator DrawStartCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            playerDeck.DrawCard();

            yield return new WaitForSeconds(timeDelay);
        }
    }
    
    public void NextTurn()
    {
        if(battleEnded){
            EndBattle();
        }

        currentPhase++;        

        if((int) currentPhase >= System.Enum.GetValues(typeof(TurnOrder)).Length)
            currentPhase = 0;

        switch (currentPhase)
        {
            case TurnOrder.playerActive:
                break;

            case TurnOrder.playerCardAttacks:       
                CardPointsController.instance.PlayerAttack();
                Debug.Log("bim bom");
                break;
            
            case TurnOrder.enemyActive:
                animator.SetTrigger("TurnToEnemy");    
                EnemyController.instance.StartAction();
                break;

            case TurnOrder.enemyCardAttacks:
                CardPointsController.instance.EnemyAttack();
                animator.SetTrigger("TurnToPlayer"); 
                break;

            default:
                throw new System.Exception("Unexpected error");
        }
    }

    public void EndPlayerTurn()
    {
        if(currentPhase != TurnOrder.playerActive) return;        
        NextTurn();
        Debug.Log(currentPhase);
    }

    public void Damageplayer(int damageAmount){
        AudioManager.instance.PlaySFX(6);
        PlayerHealth -= damageAmount;
    }

    public void DamageEnemy(int damageAmount){
        AudioManager.instance.PlaySFX(5);
        EnemyHealth -= damageAmount;
    }

    void EndBattle(){
        Time.timeScale = 0f;
        panelEndgame.SetActive(true);
    }

    private void OnMouseOver() {
        if(Input.GetMouseButtonDown(0))
            BattleController.instance.EndPlayerTurn();
    }
}
