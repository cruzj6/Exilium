using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public Transform player;
	public float cameraZDistance;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 cameraPos = transform.position;

		cameraPos.x = player.position.x;
		cameraPos.z = player.position.z - cameraZDistance;

		transform.position = cameraPos;

	}
}
