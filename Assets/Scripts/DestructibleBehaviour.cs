using UnityEngine;
using System.Collections;

//Things containing DBs that need to shrink too must be at most their parent.

public class DestructibleBehaviour : MonoBehaviour {

	private float shrinkSpeed = 0.9f;
	private Vector3 cutoffScaling = new Vector3 (0.3f, 0.3f, 0.3f);
	public string type = "cube";

	private bool startShrink = false;
	private DestructiblesHandler dh;
	private bool canDestroy;
	public bool isWall = false;

	private Rigidbody rb;
	private Renderer rend;
	private Material baseMat;

	public float interval = 0.75f;
	private float startTime;
	private bool changeMatbool;

	private float currentVolume;


	private GameObject standard;
	private StandardObject sob;

	public bool isPartOfLarger = false;
	private bool sentVolume = false, ssSet = false;
	private float relativeScaling = 1;

	private float[] dimensions; //L, W, H, R

	void Start () {
		standard = GameObject.Find ("StandardObject");
		sob = standard.GetComponent<StandardObject> ();
		relativeScaling = sob.getStandardScaling();
		currentVolume = VolumeCalculator.getScaledVolume (gameObject, type, relativeScaling);

		rend = this.GetComponent<Renderer> ();
		baseMat = rend.material;
		dh = transform.parent.GetComponent<DestructiblesHandler> ();
		startTime = Time.time;
		changeMatbool = false;
		
		rb = GetComponent<Rigidbody> ();
		setMass ();
		shrinkSpeed *= transform.lossyScale.magnitude;

		dimensions = new float[4]{0,0,0,0};
		setDimensions ();
	}

	public void setBaseMat(Material m){
		baseMat = m;
	}

	private void setDimensions(){
		relativeScaling = sob.getStandardScaling ();
		switch (type) {
		case "cube":
			dimensions[0] = transform.lossyScale.x / relativeScaling;
			dimensions[1] = transform.lossyScale.z / relativeScaling;
			dimensions[2] = transform.lossyScale.y / relativeScaling;
			break;
		case "sphere":
			dimensions[3] = (transform.lossyScale.x / 2) / relativeScaling;
			break;
		case "cylinder":
			dimensions[3] = (transform.lossyScale.x / 2) / relativeScaling;
			dimensions[2] = transform.lossyScale.z / relativeScaling;
			break;
		case "cone":
			dimensions[3] = (transform.lossyScale.x * 2 / 2) / relativeScaling;
			dimensions[2] = transform.lossyScale.z * 2 / relativeScaling;
			break;
		}
	}

	public float[] getDimensions(){
		setDimensions ();
		return dimensions;
	}

	private void setMass(){
		if (rb != null) {
			rb.mass = currentVolume;
		}
	}

	public Material getBaseMat(){
		return baseMat;
	}

	public void setStartShrink(bool b){
		startShrink = b;
	}

	public bool getStartShrink(){
		return startShrink;
	}

	public void setShrinkSpeed(float f){
		shrinkSpeed = f;
		ssSet = true;
	}

	private void checkCanDestroy(){
		if (dh != null) {
			this.canDestroy = dh.canDestroy;
			if (!dh.canDestroy){
				startShrink = false;
			}
		}
	}

	public bool getCanDestroy(){
		return this.canDestroy;
	}

	public void setTempMaterial(Material m){
		rend.material = m;
		startTime = Time.time;
		changeMatbool = true;
	}

	private void setMats(){
		if (changeMatbool && Time.time - startTime > interval) {
			rend.material = baseMat;
			changeMatbool = false;
		}
	}

	private void sendVolumeToParent(float f){
		if (dh != null) {
			dh.updatePlayerAmount (f);
		} else {
			Debug.Log("No parent of destructible found");
		}
	}

	void shrink(){
		if (!sentVolume) {
			sendVolumeToParent(currentVolume);
			sentVolume = true;
		}

		Transform tobeShrunk = transform;
		if (isPartOfLarger) {
			tobeShrunk = transform.parent;
		}

		float max = Mathf.Max (new float[3]{tobeShrunk.localScale.x, tobeShrunk.localScale.y,
			tobeShrunk.localScale.z});
		Vector3 rSub = new Vector3 (tobeShrunk.localScale.x / max,
		                          tobeShrunk.localScale.y / max,
		                          tobeShrunk.localScale.z / max);
		Vector3 sub = tobeShrunk.localScale - rSub * Time.deltaTime * shrinkSpeed;
		if (!(sub.x < 0 || sub.y < 0 || sub.z < 0) && tobeShrunk.localScale.magnitude > cutoffScaling.magnitude) {
			tobeShrunk.localScale -= rSub * Time.deltaTime * shrinkSpeed;
		} else {
			Destroy(tobeShrunk.gameObject);
		}

	}

	public float getCurrentVolume(){
		return currentVolume;
	}
	
	void Update () {
		relativeScaling = sob.getStandardScaling ();
		if (!ssSet) {
			shrinkSpeed = transform.lossyScale.magnitude / relativeScaling;
		}
		currentVolume = VolumeCalculator.getScaledVolume (gameObject, type, relativeScaling);
		setMats ();
		if (this.transform.position.y < -500) {
			Destroy(this.gameObject);
		}
		this.checkCanDestroy ();
		if (startShrink) {
			shrink ();
		}

	}
}
