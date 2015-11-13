using UnityEngine;
using System.Collections;

public class Quest{

	private GameObject parentNPC;
	private Reward reward;
	private string name;
	private string description;

	public Quest(GameObject g, Reward r, string name, string desu){
		parentNPC = g;
		reward = r;
		this.name = name;
		this.description = desu;
	}

	public string getName(){
		return this.name;
	}

	public string getDescription(){
		return this.description;
	}

	public void claimReward(){
		reward.applyReward ();
	}

	public override string ToString(){
		return string.Format ("Name: {0}\nDescription: {1}\n", name, description);
	}
}
