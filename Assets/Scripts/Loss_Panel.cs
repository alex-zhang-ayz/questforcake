using UnityEngine;
using System.Collections;

public class Loss_Panel : MonoBehaviour {

	public GameObject comicObj;
	public GameObject endPanel;
	private ShowComic sc;
	public string level5 = "";
	public string mainMenuLevel = "";

	void Start () {
		sc = comicObj.GetComponent<ShowComic> ();
	}

	public void toLevel5(){
		Application.LoadLevel (level5);
	}

	public void toMainMenu(){
		Application.LoadLevel (mainMenuLevel);
	}

	void Update () {
		if (sc.getFinished ()) {
			endPanel.SetActive(true);
		}
	}
}
