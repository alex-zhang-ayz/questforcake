using UnityEngine;
using System.Collections;

public class DestructiblesHandler : MonoBehaviour {
	
	public GameObject player;
	public bool canDestroy = true;
	private PlayerVolume pv;
	private float tobeAdded = 0;

	void Start () {
		pv = player.GetComponent<PlayerVolume> ();
	}

	public void updatePlayerAmount(float f){
		tobeAdded += f;
	}


	void Update () {
		if (tobeAdded > 0){
			float tp = tobeAdded * 0.1f;
			if (tp < 1){
				pv.grow(tobeAdded);
				tobeAdded = 0;
			}else{
				pv.grow(tp);
				tobeAdded -= tp;
			}
		}
	}
}
