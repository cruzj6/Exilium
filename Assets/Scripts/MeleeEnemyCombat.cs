using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MeleeEnemyCombat : MonoBehaviour {

	Animator anim;
	bool isDead;
	bool gotHit;
	public Slider myHealth;
	// Use this for initialization
	void Start () {

		anim = gameObject.GetComponent<Animator> ();

		gotHit = false;
		isDead = false;
	}
	
	// Update is called once per frame
	void Update () {

		//If this enemy was hit do this
		if (this.gotHit) {

			anim.SetTrigger ("Hit");

			isDead = myHealth.GetComponent<EnemyHealth>().UpdateLifeTotal(50);

		}

		gotHit = false;//Set this to false after each frame

		if (isDead) {

			gameObject.GetComponent<NavMeshAgent>().Stop ();
			gameObject.GetComponent<NavMeshAgent>().velocity = new Vector3(0,0,0);

			anim.SetTrigger("Die");

			Destroy(transform.parent.gameObject, 2.02f);
		}
	}

	//Were we hit in the last frame?
	public bool GotHit{
		set{
			gotHit = value;
		}
	}
}
