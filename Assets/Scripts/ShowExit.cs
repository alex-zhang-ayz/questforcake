using UnityEngine;
using System.Collections;

public class ShowExit : MonoBehaviour {

	public GameObject exit;

	void Start () {
	
	}

	void checkShow(){
		if (transform.childCount == 0) {
			exit.SetActive(true);
		}
	}

	void Update () {
		checkShow ();
	}
}
