using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExtraFunctions : MonoBehaviour {

	//transform, typeof(SOMETYPE)
	public static Transform searchInAncestor(Transform t, System.Type type){
		if (t.root == t) {
			return null;
		} else if (t.GetComponent(type) != null) {
			return t;
		} else {
			return searchInAncestor(t.parent, type);
		}
	}

	public static void destroyChildren(Transform t){
		List<Transform> children = new List<Transform> ();
		foreach (Transform child in t) {
			children.Add(child);
		}
		foreach (Transform tr in children) {
			tr.parent = null;
			Destroy(tr.gameObject);
		}
	}

	void Awake(){
		DontDestroyOnLoad (this);
	}
	

}
