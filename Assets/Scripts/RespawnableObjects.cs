using UnityEngine;
using System.Collections;

public class RespawnableObjects : MonoBehaviour {
	//Component in the parent object
	//Children are the respawnable objects

	private GameObject[] savedObjects;
	public bool objStartActive = false;
	public GameObject copyContainer;

	void Start () {
		savedObjects = new GameObject[transform.childCount];
		int i = 0;
		foreach (Transform child in transform) {
			savedObjects[i] = child.gameObject;
			child.gameObject.SetActive(false);
			i++;
		}
		copyObjects ();
	}

	public void copyObjects(){
		foreach (GameObject saved in savedObjects) {
			GameObject g = GameObject.Instantiate(saved, saved.transform.position, 
			                                      saved.transform.rotation) as GameObject;
			g.transform.SetParent(copyContainer.transform);
			g.transform.localScale = saved.transform.localScale;
			g.SetActive(objStartActive);
		}
	}

	public void removeContainerObjects(){
		ExtraFunctions.destroyChildren (copyContainer.transform);
	}

	public int getBaseChildCount(){
		return transform.childCount;
	}

	void Update () {
	
	}
}
