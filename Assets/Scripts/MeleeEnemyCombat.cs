using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MeleeEnemyCombat : MonoBehaviour {

	Animator anim;
	bool isDead;
	public Slider myHealth;//SLider item that contains info on enemy health and UI of healthBar
	Queue hitsToMe;
	public float basicAttackSpeed;
	public float basAtkMin;
	public float basAtkMax;
	float AttackTimer;
	BasicEnemyMove movementScript;

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		movementScript = gameObject.GetComponent<BasicEnemyMove> ();
		hitsToMe = new Queue ();
		isDead = false;
		AttackTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//Check for and take hits that were recieved this frame (or are in queue to be recieved)
		AttackInfoContainer thisFramesAttackInfo = null;

		if(hitsToMe.Count > 0){
			thisFramesAttackInfo = (AttackInfoContainer) hitsToMe.Dequeue ();
			takeAttack (thisFramesAttackInfo);
		}

		if(movementScript.isInMeleeRangeOf(movementScript.thePlayer)){
			if(AttackTimer <= 0)
			{
				//TODO: PH Test combat damage and crit ideas
				float damageNum = Random.Range(basAtkMin,basAtkMax);
				
				if(CriticalStrike(5)){
					damageNum = damageNum * 2.0f;
					Debug.Log ("Crit!");
				}

				//Basic Attack Animation
				BasicAttack (new AttackInfoContainer(damageNum, true));
				
				AttackTimer = basicAttackSpeed;
			}
		}
		//Count Down till next attack
		AttackTimer -= Time.deltaTime;

		//If this enemy is dead do the following
		//TODO: Make it stop looking at the player in the "BasicEnemyMove" script
		//when dead
		if (isDead) {
			gameObject.GetComponent<NavMeshAgent>().Stop ();
			gameObject.GetComponent<NavMeshAgent>().velocity = new Vector3(0,0,0);

			anim.speed = 1;
			anim.SetTrigger("Die");

			Destroy(transform.parent.gameObject, 2.02f);
		}
	}

	/// <summary>
	/// Function to make enemy take this attack based on theAttack container
	/// info and if it hit or not
	/// </summary>
	/// <param name="theAttack">The attack.</param>
	public void takeAttack(AttackInfoContainer theAttack)
	{
		if (theAttack != null && theAttack.DidHit) {
			anim.SetTrigger ("Hit");
			isDead = myHealth.GetComponent<EnemyHealth>().UpdateLifeTotal(theAttack.DamageNum);
			Debug.Log(theAttack.DamageNum);
		}
	}

	//TODO: Get Animations lined up with damage taken by player
	void BasicAttack(AttackInfoContainer theContainer)
	{
		anim.speed = 6/ basicAttackSpeed;
		anim.SetTrigger ("Attack");
		movementScript.thePlayer.GetComponent<MeleeCombat>().queueHit(theContainer);
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

	//TODO: create an "AttackInfoContainer" to store attack damage, status effect etc. and replace the float
	//param with it
	/// <summary>
	/// queues a hit to the enemy, pass true if hit and AttackInfo in the second param TODO:(float for now)
	/// </summary>
	/// <returns><c>true</c>, if hit was caused, <c>false</c> otherwise.</returns>
	/// <param name="wasHit">If set to <c>true</c> was hit.</param>
	/// <param name="damageNum">Damage number.</param>
	public void queueHit(AttackInfoContainer theAttackInfo)
	{
		hitsToMe.Enqueue ((object)theAttackInfo);
	}

}
