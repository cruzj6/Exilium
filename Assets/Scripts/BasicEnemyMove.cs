using UnityEngine;
using System.Collections;

public class BasicEnemyMove : MonoBehaviour {

	public GameObject thePlayer;
	public float moveSpeed;
	Animator anim;
	NavMeshAgent navAgent;

	// Use this for initialization
	void Start () {
		//Get our needed objects
		anim = gameObject.GetComponent<Animator> ();
		thePlayer = GameObject.FindWithTag ("player");
		navAgent = GetComponent<NavMeshAgent> ();
	}

	void OnCollisionEnter(Collision collisionInfo)
	{
		if(collisionInfo.gameObject.tag == "enemy")
		   navAgent.velocity = new Vector3(0,0,0);//To smooth out collisions and "shoving" of others
	}

	void OnCollisionStay(Collision collisionInfo)
	{
		if (collisionInfo.gameObject.tag == "player") {
			navAgent.velocity = new Vector3 (0, 0, 0);
			navAgent.Stop ();
		}
	}

	// Update is called once per frame
	void Update () {
		//When enemy gets this close to the player stop moving and do this
		if (isInMeleeRangeOf (thePlayer)) {
			CeaseMovement ();
		}

		else {//Move along
			MoveTowardsPlayer();
		}
	}

	/// <summary>
	/// Sets bool for running animation to true and 
	/// tells navAgent to move to the player
	/// </summary>
	void MoveTowardsPlayer()
	{
		anim.speed = 1;
		anim.SetBool("IsRunning", true);
		Vector3 playerMovTo = thePlayer.transform.position;
		playerMovTo.y = transform.position.y;
		navAgent.SetDestination (playerMovTo);
		navAgent.Resume ();
	}

	/// <summary>
	/// Checks if this enemy is in melee range of the provided GameObject
	/// </summary>
	/// <returns><c>true</c>, if in melee range of was ised, <c>false</c> otherwise.</returns>
	/// <param name="theObject">The object.</param>
	public bool isInMeleeRangeOf(GameObject theObject){

		if (Vector3.Distance (transform.position, theObject.transform.position) 
		    <= this.gameObject.GetComponent<SphereCollider> ().radius * transform.lossyScale.x * 2.5)
			return true;
		else 
			return false;
	}

	/// <summary>
	/// Animates this enemies' basic attack and ceases movement to do so
	/// TODO: Needs to do damage
	/// </summary>
	void CeaseMovement()
	{
		navAgent.Stop ();
		navAgent.velocity = new Vector3(0,0,0);
		
		Vector3 enemyLookAt = thePlayer.transform.position;
		enemyLookAt.y = this.transform.position.y;
		
		transform.LookAt(enemyLookAt);
		anim.SetBool ("IsRunning",false);
	}

}