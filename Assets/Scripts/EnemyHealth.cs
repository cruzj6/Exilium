using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//TODO: Invisible health bars until hit
public class EnemyHealth : MonoBehaviour {

	public GameObject myEnemy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		UpdatePosition ();

	}

	void UpdatePosition()
	{
		Vector3 wantedPos = Camera.main.WorldToScreenPoint (myEnemy.transform.position);
		wantedPos.y += (myEnemy.GetComponent<SphereCollider>().radius * myEnemy.transform.localScale.x) + (30 * myEnemy.transform.localScale.x);
		transform.position = wantedPos;

	}


	public bool UpdateLifeTotal(float amountToAdd)
	{

		Slider lifeBar = GetComponent<Slider> ();
		lifeBar.value -= amountToAdd;


		if (lifeBar.value <= 0) {
			return true;
		} else 
			return false;
	}
}
