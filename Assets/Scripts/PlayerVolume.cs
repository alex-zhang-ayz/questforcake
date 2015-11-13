using UnityEngine;
using System.Collections;

public class PlayerVolume : MonoBehaviour {

	public string type = "sphere";
	public string preservedDataName;
	public GameObject primEventObj;
	public GameObject particleGenObj;
	private PlayerSoundFX psfx;
	private ParticleGenerator pg;
	private PrimaryEventHandler peh;
	private PreservedGameData pgd;
	private float currentVolume;
	private float nextScaleVolume;
	private float currScaleVolume;
	private float diameter = 0;

	private GameObject standard;
	private StandardObject sob;
	private float relativeScaling = 1;

	void Start () {
		standard = GameObject.Find ("StandardObject");
		sob = standard.GetComponent<StandardObject> ();

		relativeScaling = sob.getStandardScaling();
		psfx = gameObject.GetComponent<PlayerSoundFX> ();
		pg = particleGenObj.GetComponent<ParticleGenerator> ();
		peh = primEventObj.GetComponent<PrimaryEventHandler> ();
		GameObject pgdg = GameObject.Find (preservedDataName);
		if (pgdg == null) {
			pgd = null;
		} else {
			pgd = pgdg.GetComponent<PreservedGameData> ();
		}
		if (pgd != null && pgd.getSavedVolume () >= 0) {
			currentVolume = pgd.getSavedVolume ();
		} else {
			currentVolume = VolumeCalculator.newGetVolume(this.gameObject, this.type);
		}
		updateScale(false);
	}

	public void addLevels(int i){
		diameter = Mathf.Pow (6 * currentVolume / Mathf.PI, 1 / 3.0f);
		float currScale = Mathf.Floor (diameter);
		float desiredDiameter = currScale + i;
		float csVol = VolumeCalculator.volumeFromDiameter (desiredDiameter);
		currentVolume = csVol;
	}

	private void updateScale(bool spawnP){
		relativeScaling = sob.getStandardScaling ();
		diameter = Mathf.Pow (6 * currentVolume / Mathf.PI, 1 / 3.0f);
		float currScale, nextScale;
		currScale = Mathf.Floor (diameter);
		nextScale = Mathf.Ceil (diameter);
		if (currScale == nextScale) {
			nextScale += 1;
		}

		float csVol = VolumeCalculator.volumeFromDiameter (currScale);
		if (spawnP && currScaleVolume < csVol){
			pg.spawnGrowParticle();
			psfx.playSound(1);
		}
		currScaleVolume = csVol;
		nextScaleVolume = VolumeCalculator.volumeFromDiameter (nextScale);
		transform.localScale = new Vector3 (currScale * relativeScaling, currScale * relativeScaling, currScale * relativeScaling);
		setMass ();
	}


	public float getCurrentVolume(){
		return currentVolume;
	}

	public void percentShrink(float f){
		if (f <= 1 && f > 0) {
			psfx.playSound(2);
			pg.spawnShrinkParticle();
			currentVolume *= f;
		}
	}

	public void shrinkPlayer(){
		float shrinkPercent = 0.9f;
		if (diameter > 1) {
			float tdiameter = Mathf.Pow (6 * currentVolume * shrinkPercent / Mathf.PI, 1 / 3.0f);
			if (tdiameter > 1){
				currentVolume *= 0.9f;
				pg.spawnShrinkParticle();
				psfx.playSound(2);
			}else{
				currentVolume = currScaleVolume;
			}
		} else {
			peh.sendText("tiny");
		}
		/*
		if (getScaleProgression () > 0) {
			float portion = nextScaleVolume * 0.5f;
			if (currentVolume - portion < currScaleVolume){
				currentVolume = currScaleVolume;
			}else{
				currentVolume -= portion;
			}
			pg.spawnShrinkParticle();
			psfx.playSound(2);
		} else {
			if (diameter > 1){
				if (0.1f * currentVolume > 1){
					currentVolume -= 0.01f * currentVolume;
				}else{
					currentVolume -= 1;
				}
				updateScale(true);
				currentVolume = currScaleVolume;
				pg.spawnShrinkParticle();
				psfx.playSound(2);

			}else{
				peh.sendText("tiny");
			}
		}*/
	}

	public bool shrinkByAmount(float f){
		if (f > currentVolume) {
			return false;
		} else {
			currentVolume -= f;
			return true;
		}
	}

	public void grow(float f){
		currentVolume += f;
	}

	private void setMass(){
		Rigidbody rb = this.GetComponent<Rigidbody> ();
		if (rb != null) {
			rb.mass = transform.localScale.x;
		}
	}

	public void adjustScale(Vector3 newScale){
		currentVolume = VolumeCalculator.volumeFromDiameter(newScale.x);
	}

	public float getScaleDifference(){
		return nextScaleVolume - currScaleVolume;
	}

	public float getScaleProgression(){
		return currentVolume - currScaleVolume;
	}


	public float getCurrScaleVolume(){
		return currScaleVolume;
	}

	public float getNextScaleVolume(){
		return nextScaleVolume;
	}

	public float getPercentVolume(){
		float f = 1;
		if (getScaleDifference() > 0) {
			f = getScaleDifference();
		}
		return getScaleProgression() / f;
	}

	public int getDiameter(){
		return Mathf.FloorToInt(diameter);
	}

	public void saveData(){
		pgd.savePlayerScale (transform.localScale);
		pgd.savePlayerVolume (currentVolume);
	}

	void Update () {
		updateScale (true);
	}
}
