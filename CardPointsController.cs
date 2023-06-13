using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPointsController : MonoBehaviour
{
    public Placeholder[] playerPlaceholders, enemyPlaceholders;
    private float timeBetweenAttacks = 0.5f;
    public static CardPointsController instance;

    private void Awake() {
        instance = this;
    }

    public void PlayerAttack(){
        StartCoroutine(PlayerAttackCoroutine());
    }

    IEnumerator PlayerAttackCoroutine(){
        yield return new WaitForSeconds(timeBetweenAttacks);

        for(int i = 0; i < playerPlaceholders.Length; i++)
        {
            if(playerPlaceholders[i].activeCard == null) continue;


            if(enemyPlaceholders[i].activeCard != null){
               enemyPlaceholders[i].activeCard.DamageCard(playerPlaceholders[i].activeCard.Attack);       
            } else {
                
                BattleController.instance.DamageEnemy(playerPlaceholders[i].activeCard.Attack);
            }  

            Card current = playerPlaceholders[i].activeCard;
            current.AttackCard();

            yield return new WaitForSeconds(timeBetweenAttacks);
        }

        BattleController.instance.NextTurn();
    }

    public void EnemyAttack(){
        StartCoroutine(EnemyAttackCoroutine());
    }

    IEnumerator EnemyAttackCoroutine(){
        yield return new WaitForSeconds(timeBetweenAttacks);
        Debug.Log("1");

        for(int i = 0; i < enemyPlaceholders.Length; i++)
        {
            if(enemyPlaceholders[i].activeCard == null) continue;



            if(playerPlaceholders[i].activeCard != null){
               playerPlaceholders[i].activeCard.DamageCard(enemyPlaceholders[i].activeCard.Attack);
            } else {
                BattleController.instance.Damageplayer(enemyPlaceholders[i].activeCard.Attack);
            }
            
            Card current = enemyPlaceholders[i].activeCard;
            current.AttackCard();
            
            yield return new WaitForSeconds(timeBetweenAttacks);
        }
        if(BattleController.instance.battleEnded == false){            
            DialogSystem.instance.sayPhrase();
        }
        BattleController.instance.NextTurn();
    }
}
