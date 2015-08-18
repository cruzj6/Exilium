using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MeleeEnemyCombat : MonoBehaviour {

	bool isDead;
	bool gotHit;
	public Slider myHealth;
	// Use this for initialization
	void Start () {
		gotHit = false;
		isDead = false;
	}
	
	// Update is called once per frame
	void Update () {

		//If this enemy was hit do this
		if (this.gotHit) {
			
			//Test Code
		//	gameObject.GetComponent<Renderer>().material.color = Color.red;
			isDead = myHealth.GetComponent<EnemyHealth>().UpdateLifeTotal(50);

		}
		//else
//			gameObject.GetComponent<Renderer>().material.color = Color.white;
		
		//End Test code

		gotHit = false;//Set this to false after each frame

		if (isDead) {
			Destroy(transform.parent.gameObject);
		}
	}

	//Were we hit in the last frame?
	public bool GotHit{
		set{
			gotHit = value;
		}
	}
}
