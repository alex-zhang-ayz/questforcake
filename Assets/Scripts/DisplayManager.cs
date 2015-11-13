using UnityEngine;
using System.Collections;

public class DisplayManager : MonoBehaviour {

	public GameObject player;
	private PlayerVolume pv;

	void Start () {
		pv = player.GetComponent<PlayerVolume> ();
	}

	public float getPercentVolume(){
		return pv.getPercentVolume ();
	}

	public float getScaleProgression(){
		return pv.getScaleProgression ();
	}

	public float getScaleDifference(){
		return pv.getScaleDifference();
	}

	public int getDiameter(){
		return pv.getDiameter ();
	}

	void Update () {
	
	}
}
