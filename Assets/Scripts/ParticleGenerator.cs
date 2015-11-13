using UnityEngine;
using System.Collections;

public class ParticleGenerator : MonoBehaviour {

	public GameObject particleGrowPrefab;
	public GameObject particleShrinkPrefab;
	public GameObject player;
	private ParticleSystem partSys;
	private int particleLimit = 5;


	void Start () {

	}

	private bool overLimit(){
		if (player.transform.childCount > particleLimit + 1) {
			return true;
		} else {
			return false;
		}
	}
	
	public void spawnGrowParticle(){
		if (overLimit ()) {
			Destroy(player.transform.GetChild(1).gameObject);
		}
		GameObject particle = GameObject.Instantiate (particleGrowPrefab) as GameObject;
		particle.transform.parent = player.transform;
		particle.transform.localPosition = Vector3.zero;
		transformChildParticles (particle.transform, 2 * player.transform.localScale.x);
	}

	public void spawnShrinkParticle(){
		if (overLimit ()) {
			Destroy(player.transform.GetChild(1).gameObject);
		}
		GameObject particle = GameObject.Instantiate (particleShrinkPrefab) as GameObject;
		particle.transform.parent = player.transform;
		particle.transform.localPosition = Vector3.zero;
		transformChildParticles (particle.transform, 2 * player.transform.localScale.x);
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
	
	}
}
