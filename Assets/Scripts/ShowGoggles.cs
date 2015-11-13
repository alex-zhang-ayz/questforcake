using UnityEngine;
using System.Collections;

public class ShowGoggles : MonoBehaviour {

	public GameObject comicUI;
	private PlayerControls pc;
	private Level2_ShowComic l2sc;
	private bool doOnce1 = false, doOnce2 = false;

	void OnTriggerEnter(Collider c){
		if (c.gameObject.tag == "Player") {
			if (!doOnce1){
				c.transform.GetChild(0).gameObject.SetActive(true);
				comicUI.SetActive(true);
				pc = c.GetComponent<PlayerControls>();
				pc.toggleCanMove();
				doOnce1 = true;

				c.GetComponent<Rigidbody>().velocity = Vector3.zero;

				Vector3 newPos = transform.position;
				newPos.y -= 100;
				transform.position = newPos;
			}
		}
	}

	void Start () {
		l2sc = comicUI.GetComponent<Level2_ShowComic> ();
	}
	
	void checkFinished(){
		if (l2sc.getFinished () && !doOnce2) {
			pc.toggleCanMove();
			comicUI.SetActive(false);
			doOnce2 = true;
		}
	}

	void Update () {
		checkFinished ();
	}
}
