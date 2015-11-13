using UnityEngine;
using System.Collections;

public class CQE_showComic : MonoBehaviour, CompleteQuestEvent {

	public GameObject player;
	public GameObject snorlax;
	public GameObject comicUI;
	public GameObject volumeUI;
	private PlayerControls pc;
	private Level2_ShowComic l2sc;
	private bool startComic;

	void Start () {
		l2sc = comicUI.GetComponent<Level2_ShowComic> ();
		pc = player.GetComponent<PlayerControls> ();
	}

	public void complete(){
		startComic = true;
		comicUI.SetActive (true);
		volumeUI.SetActive (false);
		pc.toggleCanMove ();
	}

	void Update () {
		if (startComic && l2sc.getFinished ()) {
			Destroy(snorlax);
			comicUI.SetActive(false);
			volumeUI.SetActive(true);
			pc.toggleCanMove();
			startComic = false;
		}
	}
}
