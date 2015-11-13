using UnityEngine;
using System.Collections;

public class FollowerVolumeController : MonoBehaviour {

	public GameObject player;
	public GameObject destructible;
	public string type = "sphere";
	private GameObject standard;
	private StandardObject sob;
	private float relativeScaling;
	private PlayerVolume pv;
	//for now only the ballbunny/spherical followers
	private float currentVolume = 0;

	void Start () {
		pv = player.GetComponent<PlayerVolume> ();
		standard = GameObject.Find ("StandardObject");
		sob = standard.GetComponent<StandardObject> ();
		//currentVolume = VolumeCalculator.volumeFromDiameter (destructible.transform.lossyScale.x);
	}

	private void setMass(){
		Rigidbody rb = this.GetComponent<Rigidbody> ();
		if (rb != null) {
			rb.mass = transform.localScale.x;
		}
	}

	private void increaseLocalScale(){
		relativeScaling = sob.getStandardScaling ();
		transform.localScale = Vector3.one * (transform.localScale.x 
		                                      + relativeScaling / destructible.transform.localScale.x);
	}

	public void grow(){
		if (pv.shrinkByAmount (getNeededVolume ())) {
			currentVolume += getNeededVolume();
			increaseLocalScale();
			setMass ();
		} else {
			print ("not enough volume, you need "+ getNeededVolume()+" volume.");
		}
	}

	public float getCurrentVolume(){
		return currentVolume;
	}

	private float getNeededVolume(){

		float nextVolume = VolumeCalculator.volumeFromDiameter (destructible.transform.lossyScale.x / relativeScaling + 1);
		return nextVolume - currentVolume;
	}


	void updateCurrentVolume(){
		relativeScaling = sob.getStandardScaling ();
		currentVolume = VolumeCalculator.getScaledVolume (destructible, type, relativeScaling);
		setMass ();
	}

	void Update () {
		updateCurrentVolume ();
	}
}
