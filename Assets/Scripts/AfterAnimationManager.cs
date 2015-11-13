using UnityEngine;
using System.Collections;

public class AfterAnimationManager : MonoBehaviour {

	public GameObject cameraAniObject;
	public GameObject controlsPanel;
	public GameObject dcObject;
	public TextAsset introLinesFile;
	public GameObject chatHandlerObject;
	public GameObject volumeUiObject;
	public bool showUI = true;
	private DestroyControls dc;
	private VolumeUI vui;
	private ChatHandler ch;
	private string introString;
	private string[] introLines;
	private CameraAnimation ca;
	private bool once;

	public bool turnOnDcAfterAni = true;
	public bool toggleControlMenu = false;

	// Use this for initialization
	void Start () {
		once = false;
		dc = dcObject.GetComponent<DestroyControls> ();
		ca = cameraAniObject.GetComponent<CameraAnimation> ();
		ch = chatHandlerObject.GetComponent<ChatHandler> ();
		vui = volumeUiObject.GetComponent<VolumeUI> ();

		introString = introLinesFile.text;
		char[] delimitingChars = {'\n'};
		introLines = introString.Split (delimitingChars);
	}

	void Update () {
		if (!once && ca.getFinished ()) {
			if (toggleControlMenu){
				controlsPanel.SetActive(true);
			}
			if (showUI){
				vui.toggleAll();
			}
			if (!dc.destroyMode && turnOnDcAfterAni){
				dc.checkOverUI = true;
				dc.destroyMode = true;
			}
			foreach(string s in introLines){
				ChatRequest c = new ChatRequest(s);
				ch.sendRequest(c);
			}
			once = true;
		}
	}
}
