using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	//Publics
	public GameObject moveItem;
	public float moveSpeed;
	public Camera playerCamera;

	//Privates
	NavMeshAgent navAgent;
	GameObject EnemySelected;
	GameObject currentMoveItem;
	bool isMeleeRange;
	bool isTarEnemyClose;
	// Use this for initialization
	void Start () {
		navAgent = GetComponent<NavMeshAgent> ();
		isMeleeRange = false;
		isTarEnemyClose = false;
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButton (0)) {
			UpdateMoveTarget();
		}

		MovePlayer ();
	}

	void MovePlayer()
	{
		//If there is a current move item target
		if (currentMoveItem != null) {
			
			//Full Stop the object/player when it reached its destination or is close other target
			
			try{
				isTarEnemyClose = (Vector2.Distance (new Vector2(transform.position.x, transform.position.z), new Vector2(currentMoveItem.transform.position.x, currentMoveItem.transform.position.z)) <= currentMoveItem.gameObject.GetComponent<SphereCollider>().radius * currentMoveItem.gameObject.
				                   transform.localScale.x * 2);
				Debug.Log ("Got INfo Radius = " + currentMoveItem.gameObject.GetComponent<SphereCollider>().radius.ToString());
			}
			catch{
			}

			if ( isTarEnemyClose && 
			    currentMoveItem.gameObject.tag != "moveItem" || 
			    Vector3.Distance (transform.position, currentMoveItem.transform.position) <= 0.1) {
				
				navAgent.Stop ();
				navAgent.velocity = new Vector3(0,0,0);
				//currentMoveItem = null;
				Destroy (GameObject.FindWithTag("moveItem"));

				Vector3 enemyLookAt = currentMoveItem.gameObject.transform.position;
				enemyLookAt.y = this.transform.position.y;
				
				transform.LookAt(enemyLookAt);
				isMeleeRange = true;

			}
			else{

				Vector3 enemyLookAt = currentMoveItem.gameObject.transform.position;
				enemyLookAt.y = this.transform.position.y;
				
				transform.LookAt(enemyLookAt);
				isMeleeRange = true;

				isMeleeRange = false;

				Vector3 currentMovePos = currentMoveItem.transform.position;
				currentMovePos.y = 0;

				navAgent.SetDestination (currentMovePos);

				//If no path (out of game world) delete move object and cease movement
				if(navAgent.hasPath == false)
				{

					navAgent.Stop ();
					navAgent.velocity = new Vector3(0,0,0);
					currentMoveItem = null;
					Destroy (GameObject.FindWithTag("moveItem"));
				}
				else
					navAgent.Resume();

			}
		}

	}

	void UpdateMoveTarget()
	{
		//Create a new RaycastHit container object and create ray from camera to mouse position
		RaycastHit hit = new RaycastHit();
		Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
		
		if (Physics.Raycast(ray, out hit)) {
			
			Vector3 latestTarget = hit.point;

			if(hit.collider.tag != "ground" && hit.collider.tag != "wall")
			{
				//TODO: add code for if ray from camera to click is an enemy
				try{
					Destroy (GameObject.FindWithTag("moveItem"));
				}
				catch
				{	
				} 

				currentMoveItem = hit.collider.gameObject;
				
			}
			
			//if ray collides with anything except an enemy, object, etc, create an object to move to
			else {
				Destroy (GameObject.FindWithTag("moveItem"));
				currentMoveItem = (GameObject) Instantiate(moveItem, latestTarget, new Quaternion(0,0,0,0));
			}
			
		}
	}

	public bool IsMeleeRange{
		get{
			return isMeleeRange;
		}
	}

	public GameObject GetCurrentEnemy
	{
		get{

			if(currentMoveItem != null && currentMoveItem.gameObject.tag == "enemy")
			{
				return currentMoveItem;
			}

			else
				return null;
		}

	}
}
