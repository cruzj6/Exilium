using UnityEngine;
using System.Collections;

public class MeleeCombat : MonoBehaviour {

	public float AttackTimer;
	float attackSpeed;

	// Use this for initialization
	void Start () {
		//Set the attack speed to the initial attackTimer
		attackSpeed = AttackTimer;
	}
	
	// Update is called once per frame
	void Update () {

		GameObject currentTargetEnemy = getTargetEnemy ();
		if (IsInMeleeRange () && currentTargetEnemy != null) {

			//Get the script for the enemy we are attacking
			MeleeEnemyCombat enemyScript = currentTargetEnemy.GetComponent<MeleeEnemyCombat>();
			if (Input.GetMouseButton (0) || Input.GetMouseButton(0)) {

				//If it's time to attack perform an attack
				if(AttackTimer <= 0)
				{
					//TODO:Cause Attack Animation here
					Debug.Log ("Attack!");

					enemyScript.GotHit = true;

					AttackTimer = attackSpeed;
				}
					
			}
		}

		//Count Down till next attack
		AttackTimer -= Time.deltaTime;
	}

	bool IsInMeleeRange()
	{
		Movement movementScript = GetComponent <Movement>();
		//Debug.Log (movementScript.IsMeleeRange);
		return movementScript.IsMeleeRange;
	}

	GameObject getTargetEnemy()
	{

		Movement movementScript = GetComponent <Movement>();
		try{
		Debug.Log (movementScript.GetCurrentEnemy.tag);
		}
		catch{
		}
		return movementScript.GetCurrentEnemy;

	}

}
