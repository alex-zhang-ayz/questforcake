using UnityEngine;
using System.Collections;

public class MeteorSpawn : MonoBehaviour {

	public bool spawnMeteors = false;
	public int dir = 0; //0: pos-x, 1: neg-x, 2: pos-z, 3: neg-z
	public GameObject player;
	public GameObject cubePrefab;
	public GameObject conePrefab;
	public GameObject spherePrefab;
	public GameObject cylinderPrefab;
	public GameObject earth;
	private GameObject standard;
	private StandardObject sob;
	public float[] spawnRange;

	private float delay = 1f;
	private float startTime;
	private float relativeScaling = 1;
	private float[] bounds = new float[4]{-1500, 2500, -2000, 2000}; 

	private int limit = 20;


	void Start () {
		standard = GameObject.Find ("StandardObject");
		sob = standard.GetComponent<StandardObject> ();
		//spawnRange = new float[6]{1500, 1600, 0, 75, -1100, 1100};
		startTime = Time.time;
	}

	void scaleObject(GameObject newSpawn, float playerScale){
		/*
		float Rscaling = Random.Range (0.8f * playerScale, 1.2f * playerScale);
		float scaling;
		if (Rscaling > 10) {
			scaling = Mathf.Round (Rscaling / 10) * 10;
		}else{
			scaling = Mathf.Round(Rscaling);
		}
		newSpawn.transform.localScale = scaling * Vector3.one;
		*/
		float earthRel = 0.3f, playerRel = 1-earthRel;
		float avg = (earthRel * earth.transform.localScale.x + playerScale * playerRel) / 2;
		float randomWithinRange = Random.Range (Mathf.Min (playerScale, avg), Mathf.Max (playerScale, avg));
		float scaled = Random.Range (0.25f, 1) * randomWithinRange;
		newSpawn.transform.localScale = Mathf.Floor(scaled) * Vector3.one;
	}
	
	void spawn(GameObject g){
		if (sob != null && sob.getStandardScaling () != 0) {
			relativeScaling = sob.getStandardScaling ();
		}
		float playerScale = player.transform.localScale.x;

		GameObject newSpawn = GameObject.Instantiate (g) as GameObject;
		newSpawn.transform.SetParent (transform);
		scaleObject (newSpawn, playerScale);

		float randomX = Random.Range (spawnRange [0], spawnRange [1]);
		float randomY = Random.Range (spawnRange [2], spawnRange [3]);
		float randomZ = Random.Range (spawnRange [4], spawnRange [5]);
		newSpawn.transform.position = new Vector3 (randomX, randomY, randomZ);
		newSpawn.transform.eulerAngles = new Vector3 (Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360));
		newSpawn.GetComponent<MeteorBehaviour> ().dir = dir;
		newSpawn.GetComponent<MeteorBehaviour> ().bounds = bounds;
	}

	void Update () {
		if (spawnMeteors && transform.childCount < limit) {
			if (Time.time - startTime > delay){
				int i = Mathf.RoundToInt (Random.Range (0f, 3.99f));
				switch (i) {
				case 0:
					spawn (cubePrefab);
					break;
				case 1:
					spawn (spherePrefab);
					break;
				case 2:
					spawn (cylinderPrefab);
					break;
				case 3:
					spawn (conePrefab);
					break;
				}
				startTime = Time.time;
			}
		} else {
			startTime = Time.time;
		}
	}
}
