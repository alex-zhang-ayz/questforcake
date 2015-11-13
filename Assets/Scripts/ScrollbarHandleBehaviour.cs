using UnityEngine;
using System.Collections;

public class ScrollbarHandleBehaviour : MonoBehaviour {

	public GameObject scrollBarBehObj;
	private ScrollbarBehaviour sbb;


	void Start () {
		sbb = scrollBarBehObj.GetComponent<ScrollbarBehaviour> ();
	}

	public void startDrag(){
		sbb.startDrag ();
	}

	public void stopDrag(){
		sbb.stopDrag ();

	}

	void Update () {
		
	}
}
