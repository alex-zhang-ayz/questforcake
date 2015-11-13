using UnityEngine;
using System.Collections;

public class OutOfBoundsReset : MonoBehaviour {

	public float yValue;
	public GameObject player;

	void Start () {
	
	}
	

	void Update () {
		if (player.transform.position.y < yValue) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
