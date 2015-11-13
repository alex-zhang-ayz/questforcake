using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimedElement{

	private Dictionary<GameObject, TimedElement> container;
	private GameObject referencedGO;
	private float startTime;
	public static float delay = 2f;

	public TimedElement(Dictionary<GameObject, TimedElement> d, GameObject g){
		container = d;
		startTime = Time.time;
		referencedGO = g;
	}

	public GameObject getGameObject(){
		return referencedGO;
	}

	public float getStartTime(){
		return startTime;
	}

	public void removeSelf(){
		container.Remove (referencedGO);
	}

}
