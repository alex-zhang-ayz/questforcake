using UnityEngine;
using System.Collections;

public class AdjustPreservedData : MonoBehaviour {

	public string preservedDataName;
	public Vector3 scale;
	public GameObject playerPrefab;

	void Start () {
		GameObject pgdg = GameObject.Find (preservedDataName);
		float volume = getObjectOfScaleVolume ();
		PreservedGameData pgd = pgdg.GetComponent<PreservedGameData> ();
		pgd.savePlayerScale(scale);
		pgd.savePlayerVolume (volume);
	}

	float getObjectOfScaleVolume(){
		GameObject ob = GameObject.Instantiate (playerPrefab) as GameObject;
		ob.transform.localScale = scale;
		PlayerVolume pv = ob.GetComponent<PlayerVolume> ();
		float vol = VolumeCalculator.newGetVolume (ob, pv.type);
		Destroy (ob);
		return vol;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
