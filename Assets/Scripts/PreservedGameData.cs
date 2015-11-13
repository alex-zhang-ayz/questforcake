using UnityEngine;
using System.Collections;

public class PreservedGameData : MonoBehaviour {
	private Vector3 playerScale = Vector3.zero;
	private float playerVolume = -1;

	public void savePlayerScale(Vector3 s){
		playerScale = s;
	}

	public void savePlayerVolume(float f){
		playerVolume = f;
	}

	public Vector3 getSavedScale(){
		return playerScale;
	}

	public float getSavedVolume(){
		return playerVolume;
	}

	public void reset(){
		playerScale = Vector3.zero;
		playerVolume = -1;
	}

	void Awake(){
		DontDestroyOnLoad (transform.gameObject);
	}

	void Start () {
	}
	

	void Update () {
	}
}
