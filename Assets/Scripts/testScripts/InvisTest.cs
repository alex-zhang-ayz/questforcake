using UnityEngine;
using System.Collections;

public class InvisTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Renderer r = this.GetComponent<Renderer>();
		r.material.renderQueue = 2002;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
