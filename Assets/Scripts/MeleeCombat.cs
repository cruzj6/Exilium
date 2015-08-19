using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MeleeCombat : MonoBehaviour {

	public float attackSpeed;
	float AttackTimer;
	Movement movementScript;
	Queue hitsToMe;
	public Slider myHealth;
	// Use this for initialization
	void Start () {
		movementScript = GetComponent <Movement>();
		//Set initial Attack timer to 0
		AttackTimer = 0;
		hitsToMe = new Queue ();
	}
	
	// Update is called once per frame
	void Update () {

		AttackInfoContainer thisFramesAttackInfo = null;
		
		if(hitsToMe.Count > 0){
			thisFramesAttackInfo = (AttackInfoContainer) hitsToMe.Dequeue ();
			takeAttack (thisFramesAttackInfo);
		}

		GameObject currentTargetEnemy = getTargetEnemy ();
		if (IsInMeleeRangeSC () && currentTargetEnemy != null) {

			//Get the script for the enemy we are attacking
			MeleeEnemyCombat enemyScript = currentTargetEnemy.GetComponent<MeleeEnemyCombat>();
			if (Input.GetMouseButton (0) || Input.GetMouseButton(0)) {

				//If it's time to attack perform an attack
				//TODO: THIS IS WHERE MELEE ABILITY INFO WILL GO, IT WILL BE CARRIED BY the AttackInfoContainer to 
				//the enemycombat script
				if(AttackTimer <= 0)
				{
					//TODO: PH Test combat damage and crit ideas
					float damageNum = Random.Range(25,50);

					if(CriticalStrike(20)){
						damageNum = damageNum * 2.0f;
						Debug.Log ("Crit!");
					}

					enemyScript.queueHit(new AttackInfoContainer(damageNum, true));

					//Basic Attack Animation
					BasicAttack ();

					AttackTimer = attackSpeed;
				}
			}
		}

		//Count Down till next attack
		AttackTimer -= Time.deltaTime;
	}

	/// <summary>
	/// TEMPORARY: Testing ideas for implementing crits
	/// </summary>
	/// <returns><c>true</c>, if strike was criticaled, <c>false</c> otherwise.</returns>
	bool CriticalStrike(float critChance)
	{
		if (Random.Range (1, 100) <= critChance)
			return true;
		else
			return false;
	}

	public void queueHit(AttackInfoContainer theContainer)
	{
		hitsToMe.Enqueue ((object)theContainer);
	}


	void takeAttack(AttackInfoContainer theContainer)
	{
		if (theContainer != null && theContainer.DidHit) {
			myHealth.GetComponent<PlayerHealth>().UpdateLifeTotal(theContainer.DamageNum);
			Debug.Log("Player hit for " + theContainer.DamageNum);
		}
	}

	/// <summary>
	/// Sets trigger for basic attack animation to occur
	/// </summary>
	void BasicAttack()
	{
		movementScript.playerModel.GetComponent<Animator>().SetTrigger("Attack");
	}

	/// <summary>
	/// Checks if is in melee range if Enemy has a sphere collider
	/// </summary>
	/// <returns><c>true</c> if this instance is in melee range S; otherwise, <c>false</c>.</returns>
	bool IsInMeleeRangeSC()
	{
		return movementScript.IsMeleeRangeSC;
	}


	/// <summary>
	/// Get the target enemy from the movementScript (null if currentMoveItem isnt an enemy)
	/// </summary>
	/// <returns>The target enemy.</returns>
	GameObject getTargetEnemy()
	{
		return movementScript.GetCurrentEnemy;
	}

}
