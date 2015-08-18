using UnityEngine;
using System.Collections;

/*Joey: Reorganized code 8-18-15
 * 
 */
public class Movement : MonoBehaviour {

	#region Properties

	//Public Vars
	public GameObject moveItem;
	public float moveSpeed;
	public Camera playerCamera;
	public GameObject playerModel;

	//Private Vars
	NavMeshAgent navAgent;
	GameObject EnemySelected;
	GameObject currentMoveItem;
	bool isMeleeRange;

	Animator anim;

	#endregion Properties


	// Use this for initialization
	void Start () {

		/*Get our objects*/
		anim = playerModel.GetComponent<Animator> ();
		navAgent = GetComponent<NavMeshAgent> ();

		/*Set our private global variables*/
		isMeleeRange = false;
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButton (0) || Input.GetMouseButtonDown(0)) {
			UpdateMoveTarget();
		}

		MovePlayer ();
	}

	/// <summary>
	/// Moves the player based on the currentMoveItem
	/// </summary>
	void MovePlayer()
	{
		//If there is a current move item target
		if (currentMoveItem != null) {
			
			//Full Stop the object/player when it reached its destination or is close other target


			if (IsMeleeRangeSC || ReachedDestination()) {

				navAgent.Stop ();
				navAgent.velocity = new Vector3(0,0,0);
				DestroyMoveItem();

				Vector3 enemyLookAt = currentMoveItem.gameObject.transform.position;
				enemyLookAt.y = this.transform.position.y;

				anim.SetBool ("isRunning", false);

				transform.LookAt(enemyLookAt);

			}
			else{

				anim.SetBool ("isRunning", true);

				//Look at where we're moving
				Vector3 enemyLookAt = currentMoveItem.gameObject.transform.position;
				enemyLookAt.y = this.transform.position.y;
				transform.LookAt(enemyLookAt);

				//Move to position in 2D plane
				Vector3 currentMovePos = currentMoveItem.transform.position;
				currentMovePos.y = 0;
				navAgent.SetDestination (currentMovePos);

				//If no path (out of game world) delete move object and cease movement
				if(navAgent.hasPath == false)
				{
					navAgent.Stop ();
					navAgent.velocity = new Vector3(0,0,0);
					currentMoveItem = null;
					DestroyMoveItem();
				}
				else
					navAgent.Resume();

			}
		}

	}

	/// <summary>
	/// Casts ray to mouse position in game world and updates the current move target
	/// deleting the old one if it was a moveItem
	/// </summary>
	void UpdateMoveTarget()
	{
		//Create a new RaycastHit container object and create ray from camera to mouse position
		RaycastHit hit = new RaycastHit();
		Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
		
		if (Physics.Raycast(ray, out hit)) {
			
			Vector3 latestTarget = hit.point;

			//If this is an interactable object set it as movement target "currentMoveItem"
			if(CheckIfInteractable(hit.collider.gameObject))
			{

				DestroyMoveItem();
				currentMoveItem = hit.collider.gameObject;
				
			}
			//if ray collides with anything except an enemy, object, etc, create an object to move to
			else {
				DestroyMoveItem();

				//Create an object to move to and set as "currentMoveItem"
				currentMoveItem = (GameObject) Instantiate(moveItem, latestTarget, new Quaternion(0,0,0,0));
			}
			
		}
	}

	//TODO: Update this function as necessary
	/// <summary>
	/// Checks if the object is an "interactable" such as an enemy, chest item, etc..
	/// </summary>
	/// <returns><c>true</c>, if if interactable was checked, <c>false</c> otherwise.</returns>
	/// <param name="theObject">The object.</param>
	private bool CheckIfInteractable(GameObject theObject)
	{
		string[] theInteractableTags = {"enemy"};//Array of "Interactable tags" object may contain
		bool ObjectIsInteractable = false;
		
		for (int i = 0; i < theInteractableTags.Length; i++) {
			if(theInteractableTags[i].Equals(theObject.tag)){
				ObjectIsInteractable = true;
				break;
			}
		}
		return ObjectIsInteractable;
	}

	/// <summary>
	/// If the current move item is a "moveItem (Added object for movement) destroy it
	/// </summary>
	private void DestroyMoveItem()
	{
		if(currentMoveItem != null && currentMoveItem.gameObject.tag == "moveItem")
			Destroy (currentMoveItem);
	}

	/// <summary>
	/// Returns true if the currentMoveItem is in melee range (only if currentMoveItem has a SphereCollider)
	/// Uses the Enemies melee range to deternmine this
	/// </summary>
	/// <value><c>true</c> if this instance is melee range; otherwise, <c>false</c>.</value>
	public bool IsMeleeRangeSC{
		get{

			/*bool isTarEnemyClose = false;

			try{

				//Get Distance from player to "currentMoveItem" ignoring y-axis
				float theDistance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(currentMoveItem.transform.position.x, 
																															currentMoveItem.transform.position.z));
				//Get the Enemie's melee range
				float enemyMeleeRange = currentMoveItem.gameObject.GetComponent<SphereCollider>().radius * currentMoveItem.gameObject.transform.lossyScale.x + 1;
				isTarEnemyClose = (theDistance <= enemyMeleeRange);
			}
			catch{
			}

			if(isTarEnemyClose && currentMoveItem.gameObject.tag != "moveItem")

				isMeleeRange = true;

			else {
				isMeleeRange = false;
			}

			return isMeleeRange;*/

			try{
				BasicEnemyMove theEnemyScript = currentMoveItem.GetComponent<BasicEnemyMove>();
				return theEnemyScript.isInMeleeRangeOf(this.gameObject);
			}
			catch{
				return false;
			}
		}
	}

	/// <summary>
	/// Has the player reached the "currentMoveItem"?
	/// </summary>
	/// <returns><c>true</c>, if destination was reacheded, <c>false</c> otherwise.</returns>
	public bool ReachedDestination()
	{
		if (Vector3.Distance (transform.position, currentMoveItem.transform.position) <= 0.1) {
			return true;
		} else
			return false;
	}

	/// <summary>
	/// Returns current target enemy, if currentMoveItem is an enemy, otherwise returns null
	/// </summary>
	/// <value>The get current enemy.</value>
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
