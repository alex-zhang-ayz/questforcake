using UnityEngine;
using System.Collections;

public class MeteorBehaviour : MonoBehaviour {

	public Material[] materials;
	public int dir = 0;
	private Renderer r;
	private Rigidbody rb;
	public float[] bounds;
	public float baseSpeed = 24;

	private bool doOnce = false;

	public Vector3 pos = Vector3.zero;

	void Start () {
		r = GetComponent<Renderer> ();
		rb = GetComponent<Rigidbody> ();
		int randomMat = Mathf.FloorToInt(Random.Range (0, materials.Length));
		if (randomMat == materials.Length) {
			randomMat = materials.Length - 1;
		}
		r.material = materials [randomMat];

		//this.GetComponent<DestructibleBehaviour> ().setBaseMat (r.material);
	}
	

	void move(){
		float speed = (-1) * baseSpeed * Random.Range (1, 2.5f);
		switch (dir) {
		case 0:
			rb.velocity = new Vector3 (speed, 0, 0);
			break;
		case 1:
			rb.velocity = new Vector3 (-speed, 0, 0);
			break;
		case 2:
			rb.velocity = new Vector3 (0, 0, speed);
			break;
		case 3:
			rb.velocity = new Vector3 (0, 0, -speed);
			break;
		}

	}

	void directedMove(){
		float speed = baseSpeed / 2;
		transform.position = Vector3.Lerp (transform.position, pos, speed * Time.deltaTime);
	}

	void checkBounds(){
		if (transform.position.x < bounds [0] || transform.position.x > bounds [1]
		    || transform.position.z < bounds[2] || transform.position.z > bounds[3]) {
			print (transform.position);
			print (string.Format("x: {0} <-> {1}\nz: {2} <-> {3}\n", bounds[0], bounds[1], bounds[2], bounds[3]));
			print (string.Format("{0} (meteor) destroyed", gameObject.tag));
			Destroy(gameObject);
		}
	}

	void Update () {

		if (!doOnce) {
			DestructibleBehaviour db = GetComponent<DestructibleBehaviour>();
			if (db != null){
				db.setBaseMat (r.material);
				doOnce = true;
			}
		}
		if (Vector3.Equals (pos, Vector3.zero)) {
			move ();
		} else {
			directedMove();
		}
		checkBounds ();
	}
}
