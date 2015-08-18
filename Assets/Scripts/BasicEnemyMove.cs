using UnityEngine;
using System.Collections;

public class BasicEnemyMove : MonoBehaviour {

	GameObject thePlayer;
	public float moveSpeed;
	Animator anim;

	NavMeshAgent navAgent;

	// Use this for initialization
	void Start () {

		anim = gameObject.GetComponent<Animator> ();
		thePlayer = GameObject.FindWithTag ("player");
		navAgent = GetComponent<NavMeshAgent> ();
	}

	void OnCollisionEnter(Collision collisionInfo)
	{
		Debug.Log ("Entered Collision");
		if(collisionInfo.gameObject.tag == "enemy")
		   navAgent.Stop ();


	}

	void OnCollisionStay(Collision collisionInfo)
	{
		if (collisionInfo.gameObject.tag == "player") {
			navAgent.velocity = new Vector3 (0, 0, 0);
			navAgent.Stop ();
		}
		}

	void OnCollisionExit(Collision collisionInfo)
	{

	}

	// Update is called once per frame
	void Update () {

	
		//When enemy gets this close to the player stop moving and do this
		if (Vector3.Distance (transform.position, thePlayer.transform.position) <= this.gameObject.GetComponent<SphereCollider>().radius * transform.lossyScale.x * 2.5) {
			navAgent.Stop ();
			navAgent.velocity = new Vector3(0,0,0);

			Vector3 enemyLookAt = thePlayer.transform.position;
			enemyLookAt.y = this.transform.position.y;

			transform.LookAt(enemyLookAt);
			anim.SetBool ("IsRunning",false);
			anim.SetTrigger ("Attack");
		}

		else {//Move along

			anim.SetBool("IsRunning", true);

			Vector3 playerMovTo = thePlayer.transform.position;

			playerMovTo.y = transform.position.y;

			navAgent.SetDestination (playerMovTo);
			navAgent.Resume ();
		}


	
	}



}