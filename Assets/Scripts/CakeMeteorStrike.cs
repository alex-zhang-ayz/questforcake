using UnityEngine;
using System.Collections;

public class CakeMeteorStrike : MonoBehaviour {
	
	public GameObject meteor;
	public GameObject dirLight;
	public GameObject cameraView;
	public GameObject audioObj;
	public Material skyboxMat;
	public Vector3 finalDirLightRot;
	private AudioSource aus;
	private CQE_level3_showComic lsc;
	private float speed = 0.5f, delay = 2.5f, Ldelay = 0.25f;
	private bool started = false, startMeteor = false, finishView = false, entered = false;
	private float startTime, lowerTime = -1, amount;
	private CameraBehaviour cb;

	private bool startSoundLower = false;

	void Start () {
		cb = Camera.main.GetComponent<CameraBehaviour> ();
		lsc = GetComponent<CQE_level3_showComic> ();
		aus = audioObj.GetComponent<AudioSource> ();
		amount = aus.volume / 10;
	}

	void OnTriggerEnter(Collider c){
		if (c.tag == "Player" && !entered) {
			cb.startTempLook (cameraView.transform.position, cameraView.transform.eulerAngles);
			startTime  = Time.time;
			started = true;
			entered = true;
			startSoundLower = true;
		}
	}

	void checkSoundLower(){
		if (startSoundLower) {
			if (Time.time - lowerTime > Ldelay && aus.volume > amount){
				aus.volume -= amount;
				lowerTime = Time.time;
			}else if(aus.volume <= amount){
				aus.volume = amount;
				startSoundLower = false;
			}
		}
	}

	void Update () {
		checkSoundLower ();
		if (started) {
			if (!startMeteor){
				dirLight.transform.eulerAngles = Vector3.Lerp(dirLight.transform.eulerAngles, finalDirLightRot, speed * Time.deltaTime);
			}
			if (!startMeteor && Time.time - startTime > delay){
				meteor.SetActive(true);
				startMeteor = true;
				startTime = Time.time;

			}
			if (!finishView && startMeteor && Time.time - startTime > delay){
				cb.finishTempLook();
				finishView = true;
				RenderSettings.skybox = skyboxMat;
				lsc.complete();
				started = false;

			}
		}
	}
}
