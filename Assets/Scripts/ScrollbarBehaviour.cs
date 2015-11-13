using UnityEngine;
using System.Collections;

public class ScrollbarBehaviour : MonoBehaviour {

	public GameObject handle;
	public GameObject dragArea;
	public GameObject content;
	public GameObject mask;
	private RectTransform rtHandle, rtDragArea, rtContent, rtMask;
	private Vector3 handleStartPos, contentStartPos;
	private float dragLength;
	private float contentMoveLength;
	private float[] dragRange;
	private float[] contentDragRange;
	private bool dragging;
	private float lastMousePosition;

	void Start () {
		rtHandle = handle.GetComponent<RectTransform> ();
		rtDragArea = dragArea.GetComponent<RectTransform> ();
		rtContent = content.GetComponent<RectTransform> ();
		rtMask = mask.GetComponent<RectTransform> ();
		dragRange = new float[2];
		contentDragRange = new float[2];

		initStartPos ();
		initDragVar ();
		initContentVar ();
		setToStartPos ();
	}

	private void initStartPos(){
		float daTop = rtDragArea.localPosition.y + rtDragArea.rect.height / 2;
		Vector3 newPos = rtHandle.localPosition;
		newPos.y = daTop - rtHandle.rect.height / 2;
		handleStartPos = newPos;

		contentStartPos = rtContent.position;
	}

	private void initDragVar (){
		dragRange [0] = handleStartPos.y;
		float endPos = rtDragArea.localPosition.y - rtDragArea.rect.height / 2 + rtHandle.rect.height / 2;
		dragRange [1] = endPos;
		dragLength = dragRange[0] - dragRange[1];
	}

	private void initContentVar(){
		contentDragRange [0] = contentStartPos.y;
		float endPos = 2 * (rtMask.position.y - rtContent.position.y) + rtContent.position.y;
		contentDragRange [1] = endPos;
		contentMoveLength = contentDragRange [1] - contentDragRange [0];
	}

	public void startDrag(){
		dragging = true;
		lastMousePosition = Input.mousePosition.y;
		print (dragging);
	}

	public void stopDrag(){
		dragging = false;
	}

	public void setToStartPos(){
		rtHandle.localPosition = handleStartPos;
		rtContent.position = contentStartPos;
	}

	private void checkMove (){
		if (dragging) {
			if (lastMousePosition != Input.mousePosition.y){
				float diff = Input.mousePosition.y - lastMousePosition;
				Vector3 targetPos = rtHandle.localPosition;
				targetPos.y += diff;
				
				if (targetPos.y > dragRange[0]){
					targetPos.y = dragRange[0];
				}else if (targetPos.y < dragRange[1]){
					targetPos.y = dragRange[1];
				}

				rtHandle.localPosition = targetPos;
				moveContent();
				lastMousePosition = Input.mousePosition.y;
			}
		}
	}

	private void moveContent(){
		float percent = 1 - (rtHandle.localPosition.y - dragRange [1]) / dragLength;
		float amountMore = percent * contentMoveLength;
		Vector3 newPos = rtContent.position;
		newPos.y = contentDragRange [0] + amountMore;

		if (newPos.y > contentDragRange [1]) {
			newPos.y = contentDragRange[1];
		}else if(newPos.y < contentDragRange[0]){
			newPos.y = contentDragRange[0];
		}

		rtContent.position = newPos;
	}

	void Update () {
		checkMove ();
	}
}
