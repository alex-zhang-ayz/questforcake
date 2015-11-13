using UnityEngine;
using System.Collections;

public class CQE_level3_showComic : MonoBehaviour, CompleteQuestEvent {

	public TextAsset endLines;
	public GameObject exitTrigger;
	public GameObject comicUI;
	public GameObject volumeUI;
	public GameObject player;
	public GameObject audioObj;
	private AudioSource aus;
	private string[] endLinesA;
	private ChatHandler ch;
	private Level2_ShowComic l2sc;
	private PlayerControls pc;
	private bool startComic = false;
	public GameObject chatHandlerObj;

	private bool startSoundIncrease = false;
	private float startTime = -1, delay = 1f, amount, baseVolume;

	void Start () {
		pc = player.GetComponent<PlayerControls> ();
		l2sc = comicUI.GetComponent<Level2_ShowComic> ();
		string endLinesStr = endLines.text;
		char[] delimitingChars = {'\n'};
		endLinesA = endLinesStr.Split (delimitingChars);
		ch = chatHandlerObj.GetComponent<ChatHandler> ();
		aus = audioObj.GetComponent<AudioSource> ();
		amount = aus.volume / 4;
		baseVolume = aus.volume;
	}

	public void complete(){

		startComic = true;
		comicUI.SetActive (true);
		volumeUI.SetActive (false);
		pc.toggleCanMove ();
	}


	void checkSoundIncrease(){
		if (startSoundIncrease) {
			if (Time.time - startTime > delay && aus.volume < baseVolume){
				aus.volume += amount;
				startTime = Time.time;
			}else if(aus.volume >= baseVolume){
				aus.volume = baseVolume;
				startSoundIncrease = false;
			}
		}
	}

	void Update () {
		checkSoundIncrease ();

		if (startComic && l2sc.getFinished ()) {
			startSoundIncrease = true;
			comicUI.SetActive(false);
			volumeUI.SetActive(true);
			pc.toggleCanMove();
			startComic = false;
			exitTrigger.SetActive(true);
			foreach (string s in endLinesA){
				ChatRequest c = new ChatRequest(s);
				ch.sendRequest(c);
			}
		}

	}
}
