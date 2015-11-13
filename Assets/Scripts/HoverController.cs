using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class HoverController : MonoBehaviour {

	public GameObject player;
	public GameObject primaryEventHandlerObj;
	public Texture2D destroyCursor;
	public Texture2D lockCursor;
	public Texture2D followerCursor;
	//public Texture2D talkCursor;
	public Vector2 hotSpot = Vector2.zero;
	public CursorMode cursorMode = CursorMode.Auto;
	public Material tooBigMat;
	public Material lockedMat;
	public Material outlineMat;
	public GameObject destroyController;
	public GameObject statsTextObject;

	public bool shrinkOnLargerObject = true;
	public int statsTextMode = 1;	//0 for tutorial, 1 otherwise
	
	private GameObject statsTextPanel;
	private GameObject statsTextImage;
	private PrimaryEventHandler peh;
	private DestroyControls dc;
	private string[] objectsOfInterest;
	private bool prevMouseDown;
	private int outlineLayer = 10;

	private GameObject prevOutline;

	public bool firstTB = false, firstLck = false, firstPenalty = false;

	void Start () {
		peh = primaryEventHandlerObj.GetComponent<PrimaryEventHandler> ();
		dc = destroyController.GetComponent<DestroyControls> ();
		statsTextPanel = statsTextObject.transform.parent.gameObject;
		statsTextImage = statsTextPanel.transform.GetChild (0).gameObject;
		objectsOfInterest = new string[2]{"Destructible", "NPC"};
		prevMouseDown = true;
	}

	private void checkFirstTooBig(){
		if (!firstTB) {
			peh.sendText("firstTB");
			firstTB = true;
		}
	}

	private void checkFirstLocked(){
		if (!firstLck) {
			peh.sendText("firstLck");
			firstLck = true;
		}
	}

	private bool checkFollower(GameObject g){
		if (g.transform.root.tag == "Follower") {
			FollowerVolumeController fvc = g.transform.root.GetComponent<FollowerVolumeController> ();
			if (fvc != null){
				fvc.grow ();
			}
			return true;
		} else {
			return false;
		}
	}

	private bool overFollower(GameObject g){
		return g.transform.root.tag == "Follower";
	}

	private void checkFirstBar(GameObject g){
		BarBehaviour bb = g.GetComponent<BarBehaviour> ();
		if (bb != null && !bb.getHasCollided ()) {
			bb.sendText();
		}
	}

	void CastRay(){
		bool overUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject ();
		if (overUI) {
			resetStatsText ();
			destroyOutline();
		}

		bool isClicked = false;
		if (!prevMouseDown && Input.GetMouseButtonDown (0)) {
			isClicked = true;
		} else {
			dc.destroyLaser();
		}
		prevMouseDown = Input.GetMouseButtonDown(0);
		if (!dc.destroyMode) {
			Cursor.SetCursor(null, Vector2.zero, cursorMode);
			resetStatsText ();
		} else {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			float maxDis = player.transform.localScale.x * 10 + 100;

			int layerMask = 1<<outlineLayer;
			layerMask = ~layerMask;

			if (Physics.Raycast (ray, out hit, maxDis, layerMask)) {
				if (Array.IndexOf (objectsOfInterest, hit.collider.tag) >= 0) {

					DestructibleBehaviour db = hit.collider.GetComponent<DestructibleBehaviour> ();
					NPCBehaviour nb = hit.collider.GetComponent<NPCBehaviour> ();
					QuestStartObjects qso = hit.collider.GetComponent<QuestStartObjects>();

					if (db != null && (nb == null || !nb.hasQuest)){
						bool overLocked = !db.getCanDestroy ();
						if (overLocked) {
							if (isClicked) {
								checkFirstLocked();
								if (!checkFollower(hit.collider.gameObject)){
									db.setTempMaterial (lockedMat);
								}
							}
							if (overFollower(hit.collider.gameObject)){
								hotSpot = new Vector2(followerCursor.width / 2, followerCursor.height / 2);
								Cursor.SetCursor (followerCursor, hotSpot, cursorMode);
							}else{
								hotSpot = new Vector2 (lockCursor.width / 2, lockCursor.height / 2);
								Cursor.SetCursor (lockCursor, hotSpot, cursorMode);
							}
						}else{
							if (isClicked) {
								bool successfulShrink = dc.checkDestroy (hit.collider.gameObject);
								if (!successfulShrink) {
									checkFirstTooBig();
									checkFirstBar(hit.collider.gameObject);
									checkPlayerShrink();
									db.setTempMaterial (tooBigMat);
								}else if (qso != null){
									qso.startAssociatedQuest();
								}
							}
							hotSpot = new Vector2 (destroyCursor.width / 2, destroyCursor.height / 2);
							Cursor.SetCursor (destroyCursor, hotSpot, cursorMode);
						}
						drawOutline(hit.collider.transform);
						statsTextPanel.transform.position = Input.mousePosition;
						statsTextImage.SetActive(true);
						statsTextObject.SetActive(true);
						setStatsText (statsTextMode, hit.collider.transform, db);

					}else if (isClicked && nb != null && nb.hasQuest) {
						dc.checkNPC (hit.collider.gameObject);
						Cursor.SetCursor (null, Vector2.zero, cursorMode);
						resetStatsText ();
					}
				} else {
					destroyOutline();
					hotSpot = new Vector2 (destroyCursor.width / 2, destroyCursor.height / 2);
					Cursor.SetCursor (destroyCursor, hotSpot, cursorMode);
					//Cursor.SetCursor(null, Vector2.zero, cursorMode);
				
					resetStatsText ();
				}

			}
		}
	}

	void destroyOutline(){
		if (prevOutline != null) {
			Destroy(prevOutline);
			resetStatsText();
		}
	}

	void destroyOutline(Transform t){
		if (prevOutline != null && t.gameObject.name != prevOutline.name) {
			Destroy(prevOutline);
			resetStatsText();
		}
	}

	void drawOutline(Transform t){
		if (prevOutline == null || t.name != prevOutline.name) {
			destroyOutline (t);
			GameObject g = GameObject.Instantiate (t.gameObject) as GameObject;
			g.transform.position = t.position;
			g.transform.localScale = t.lossyScale * 1.01f;
			g.transform.eulerAngles = t.eulerAngles;
			Destroy (g.GetComponent<Rigidbody> ());
			Destroy (g.GetComponent<DestructibleBehaviour> ());
			if (g.GetComponent<MeteorBehaviour> () != null) {
				Destroy (g.GetComponent<MeteorBehaviour> ());
			}
			g.name = t.name;
			g.tag = "Stage";
			Collider c = g.GetComponent<Collider> ();
			if (c != null) {
				c.isTrigger = true;
			}
			Renderer r = g.GetComponent<Renderer> ();
			if (r != null) {
				r.material = outlineMat;
			}
			g.layer = LayerMask.NameToLayer ("Outlines");
			g.transform.SetParent (t);
			prevOutline = g;
		}
	}

	private void checkPlayerShrink(){
		if (shrinkOnLargerObject) {
			if (!firstPenalty){
				peh.sendText("firstPen");
				firstPenalty = true;
			}
			dc.shrinkPlayer();
		}
	}

	private void resetStatsText(){
		if (statsTextObject.GetComponent<Text>().text != ""){
			statsTextImage.SetActive(false);
			statsTextObject.GetComponent<Text>().text = "";
			statsTextObject.SetActive(false);
		}
	}
	
	private void setStatsText(int mode, Transform t, DestructibleBehaviour db){
		switch (mode) {
		case 0:
			string volStr = "Volume-\n"+db.getCurrentVolume().ToString("F2");
			statsTextObject.GetComponent<Text> ().text = volStr;
			break;
		case 1:
			string[] things = new string[3];
			int numthings = 0;
			float[] dim = db.getDimensions();
			switch (db.type) {
			case "cube":
				things[0] = "L-"+dim[0].ToString("F2");
				things[1] = "W-"+dim[1].ToString("F2");
				things[2] = "H-"+dim[2].ToString("F2");
				numthings = 3;
				break;
			case "sphere":
				things[0] = "R-"+dim[3].ToString("F2");
				numthings = 1;
				break;
			case "cylinder":
				things[0] = "R-"+dim[3].ToString("F2");
				things[1] = "H-"+dim[2].ToString("F2");
				numthings = 2;
				break;
			case "cone":
				things[0] = "R-"+dim[3].ToString("F2");
				things[1] = "H-"+dim[2].ToString("F2");
				numthings = 2;
				break;
			}
			string text = "";
			for (int i=0; i<numthings; i++) {
				text += " " + things[i] + "\n";
			}
			statsTextObject.GetComponent<Text> ().text = text;
			break;
		}

	}
	
	void Update () {
		CastRay();
	}
}
