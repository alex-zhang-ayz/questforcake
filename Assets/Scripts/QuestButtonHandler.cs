using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuestButtonHandler : MonoBehaviour {

	public Button currQButton;
	public Button unclaimedQButton;
	private ColorBlock selected;
	private ColorBlock notSelected;

	void Start () {
		setColorBlocks ();
		currButton ();
	}

	private void setColorBlocks(){
		selected = new ColorBlock ();
		selected.normalColor = Color.white;
		selected.highlightedColor = new Color (245, 245, 245);
		selected.pressedColor = new Color (200, 200, 200);
		selected.disabledColor = Color.black;
		selected.colorMultiplier = 1;
		selected.fadeDuration = 0.1f;

		notSelected = new ColorBlock ();
		notSelected.normalColor = Color.black;
		notSelected.highlightedColor = Color.grey;
		notSelected.pressedColor = Color.grey;
		notSelected.disabledColor = Color.black;
		notSelected.colorMultiplier = 1;
		notSelected.fadeDuration = 0.1f;
	}

	public void currButton(){
		currQButton.colors = selected;
		unclaimedQButton.colors = notSelected;
		//currQButton.interactable = false;
		//unclaimedQButton.interactable = true;
	}

	public void unclaimedButton(){
		currQButton.colors = notSelected;
		unclaimedQButton.colors = selected;
		//currQButton.interactable = true;
		//unclaimedQButton.interactable = false;
	}

	void Update () {
	
	}
}
