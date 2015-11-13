using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DestroyControls : MonoBehaviour {

	public GameObject player;
	public bool destroyMode = false;
	public GameObject laserPrefab;
	public GameObject primaryEventHandlerObj;
	public GameObject volumeUiObject;
	private VolumeUI vui;
	private GameObject currLaser;
	private PlayerVolume pv;
	private PlayerSoundFX psfx;
	private PrimaryEventHandler peh;

	private bool overLocked;
	private bool onUIExit;
	public bool firstShk;
	public bool checkOverUI = true;

	void Start () {
		peh = primaryEventHandlerObj.GetComponent<PrimaryEventHandler> ();
		vui = volumeUiObject.GetComponent<VolumeUI> ();
		pv = player.GetComponent<PlayerVolume> ();
		psfx = player.GetComponent<PlayerSoundFX> ();
		overLocked = false;
		onUIExit = false;
	}

	void Update () {
		if (checkOverUI) {
			bool overUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject ();
			if (overUI) {
				destroyMode = false;
				onUIExit = true;
			} else if (onUIExit) {
				destroyMode = true;
				onUIExit = false;
			}
		}
	}
	private Vector3 getPlayerPos(){
		return player.transform.position;
	}
	

	void drawLaserToObject(GameObject g){
		Vector3 playerPos = getPlayerPos ();
		Vector3 midpoint = new Vector3 ((playerPos.x + g.transform.position.x) / 2,
		                                (playerPos.y + g.transform.position.y) / 2,
		                                (playerPos.z + g.transform.position.z) / 2);
		Vector3 diff = new Vector3 (playerPos.x - g.transform.position.x,
		                           playerPos.y - g.transform.position.y,
		                           playerPos.z - g.transform.position.z);
		float length = diff.magnitude;

		if (currLaser != null && (currLaser.transform.localScale.y != length || currLaser.transform.position != midpoint)) {
			Destroy (currLaser.gameObject);
			currLaser = GameObject.Instantiate (laserPrefab) as GameObject;
			Quaternion facing = currLaser.transform.rotation;
			currLaser.transform.position = midpoint;

			currLaser.transform.rotation = Quaternion.LookRotation(diff.normalized) * facing;
			currLaser.transform.localScale = new Vector3 (currLaser.transform.localScale.x,
			                                              length/2,
			                                              currLaser.transform.localScale.z);
		} else if (currLaser == null){
			currLaser = GameObject.Instantiate (laserPrefab) as GameObject;
			Quaternion facing = currLaser.transform.rotation;
			currLaser.transform.position = midpoint;

			currLaser.transform.rotation = Quaternion.LookRotation(diff.normalized) * facing;
			currLaser.transform.localScale = new Vector3 (currLaser.transform.localScale.x,
			                                              length/2,
			                                              currLaser.transform.localScale.z);
		}
	}

	public void removeAll(){
		if (currLaser != null) {
			Destroy (currLaser.gameObject);
		}
	}

	public void shrinkPlayer(){
		pv.shrinkPlayer ();
	}

	public bool getOverLocked(){
		return overLocked;
	}

	public void destroyLaser(){
		if (currLaser != null) {
			Destroy (currLaser.gameObject);
		}
	}

	public void checkNPC(GameObject g){
		if (g.tag == "NPC"){ //if clicked object is NPC
			g.GetComponent<NPCBehaviour>().startQuest();
		}
	}

	private void checkFirstShrink(){
		if (!firstShk) {
			peh.sendText("firstShk");
			if (!vui.getShowAll()){
				vui.toggleAll();
			}
			firstShk = true;
		}
	}

	public bool checkDestroy(GameObject g){
		DestructibleBehaviour db = g.GetComponent<DestructibleBehaviour>();
		drawLaserToObject (g);
		if (pv.getCurrScaleVolume () >= Mathf.Floor(db.getCurrentVolume () * 100) / 100) {
			psfx.playSound(0);
			checkFirstShrink();
			db.setStartShrink (true);
			return true;
		} else {
			return false;
		}

	}

}
