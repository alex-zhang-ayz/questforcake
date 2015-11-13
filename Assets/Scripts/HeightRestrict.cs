using UnityEngine;
using System.Collections;

public class HeightRestrict : MonoBehaviour {

	public GameObject player;
	public float maxHeight;
	public float minHeight;
	public float yReduction;
	private Rigidbody rb;

	void Start () {
		rb = player.GetComponent<Rigidbody> ();
	}
	

	void Update () {

		if (rb != null && player.transform.position.y >= maxHeight && rb.velocity.y > 0) {
			rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * yReduction, rb.velocity.z);
		}


	}
}
