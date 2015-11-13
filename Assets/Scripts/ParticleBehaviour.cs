using UnityEngine;
using System.Collections;

public class ParticleBehaviour : MonoBehaviour {

	private ParticleSystem ps;

	void Start () {
		ps = this.GetComponent<ParticleSystem> ();
	}

	void checkFinished(){
		if (ps.isStopped) {
			Destroy(transform.parent.gameObject);
		}
	}

	void Update () {
		checkFinished ();
	}
}
