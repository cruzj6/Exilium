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
		wantedPos.y += myEnemy.gameObject.GetComponent<SphereCollider> ().bounds.size.y + (15 * myEnemy.transform.localScale.y);
		transform.position = wantedPos;

	}

	/// <summary>
	/// Updates life bar total (Used to track enemy life also)
	/// </summary>
	/// <returns><c>true</c>, if life total was updated, <c>false</c> otherwise.</returns>
	/// <param name="amountToAdd">Amount to add.</param>
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
