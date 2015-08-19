using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//TODO: Invisible health bars until hit
public class PlayerHealth : MonoBehaviour {

	float playerMaxHealth;
	public GameObject thePlayer;
	Slider lifeBar;
	// Use this for initialization
	void Start () {
		lifeBar = GetComponent<Slider> ();

		//TODO: PH health total
		playerMaxHealth = 2000;
		lifeBar.maxValue = playerMaxHealth;
		lifeBar.value = playerMaxHealth;

	}
	
	// Update is called once per frame
	void Update () {


	}

	/// <summary>
	/// Updates life bar total (Used to track enemy life also)
	/// </summary>
	/// <returns><c>true</c>, if life total was updated, <c>false</c> otherwise.</returns>
	/// <param name="amountToAdd">Amount to add.</param>
	public bool UpdateLifeTotal(float amountToAdd)
	{

		lifeBar.value -= amountToAdd;
		
		if (lifeBar.value <= 0) {
			return true;
		} else 
			return false;
	}
}
