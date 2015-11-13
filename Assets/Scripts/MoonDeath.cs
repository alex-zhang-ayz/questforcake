using UnityEngine;
using System.Collections;

public class MoonDeath : MonoBehaviour {

	public GameObject desParticle;
	private DestructibleBehaviour db;
	private float startTime, delay = 1;
	private int particlesLimit = 10;

	void Start () {
		db = GetComponent<DestructibleBehaviour> ();
		startTime = Time.time;
	}

	void spawnParticle(){
		if (transform.childCount > particlesLimit) {
			Destroy(transform.GetChild(0).gameObject);
		}
		GameObject particle = GameObject.Instantiate (desParticle) as GameObject;
		particle.transform.parent = transform;
		Vector3 rng = Random.onUnitSphere;
		particle.transform.position = (rng * transform.lossyScale.x) + transform.position;
		transformChildParticles (particle.transform, transform.localScale.x / 4);
	
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

	void Update () {
		if (db.getStartShrink ()) {
			if (Time.time - startTime > delay){
				db.setShrinkSpeed (transform.localScale.magnitude);
				spawnParticle ();
			}
		}

	}
}
