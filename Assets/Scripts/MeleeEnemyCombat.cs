using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MeleeEnemyCombat : MonoBehaviour {

	Animator anim;
	bool isDead;
	public Slider myHealth;//SLider item that contains info on enemy health and UI of healthBar
	Queue hitsToMe;


	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		hitsToMe = new Queue ();
		isDead = false;
	}
	
	// Update is called once per frame
	void Update () {

		//Check for and take hits that were recieved this frame (or are in queue to be recieved)
		AttackInfoContainer thisFramesAttackInfo = null;

		if(hitsToMe.Count > 0){
			thisFramesAttackInfo = (AttackInfoContainer) hitsToMe.Dequeue ();
		}

		takeAttack (thisFramesAttackInfo);

		//If this enemy is dead do the following
		//TODO: Make it stop looking at the player in the "BasicEnemyMove" script
		//when dead
		if (isDead) {
			gameObject.GetComponent<NavMeshAgent>().Stop ();
			gameObject.GetComponent<NavMeshAgent>().velocity = new Vector3(0,0,0);

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
