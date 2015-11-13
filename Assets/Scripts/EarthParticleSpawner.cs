using UnityEngine;
using System.Collections;

public class EarthParticleSpawner : MonoBehaviour {

	public GameObject particleExplodePrefab;

	void Start () {
	
	}

	void OnCollisionEnter(Collision c){
		print (c.collider.name);
		if (c.collider.tag == "Destructible") {
			createParticle(c.contacts[0].point, c.transform.lossyScale);
			Destroy(c.gameObject);
		}
	}

	void rotateEarth(){
		float xAmount = Random.Range (0.1f, 0.5f);
		float yAmount = Random.Range (-0.7f, -0.1f);
		float zAmount = Random.Range (-0.7f, -0.1f);
		Vector3 newE = transform.eulerAngles + 
			new Vector3 (xAmount * Time.deltaTime, yAmount * Time.deltaTime, zAmount * Time.deltaTime);
		transform.eulerAngles = newE;
	}

	void Update () {
		rotateEarth ();
	}

	void createParticle(Vector3 point, Vector3 size){
		GameObject particle = GameObject.Instantiate (particleExplodePrefab) as GameObject;
		particle.transform.parent = transform;
		particle.transform.position = point;
		transformChildParticles (particle.transform, 2 * size.x);
	}

	private void transformChildParticles(Transform t, float multiple){
		ParticleSystem ps = t.GetComponent<ParticleSystem> ();
		if (ps != null) {
			ps.startSize *= multiple;
		}
		if (t.childCount > 0) {
			foreach(Transform child in t){
				transformChildParticles(child, multiple);
			}
		}
	}
}
