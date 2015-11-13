using UnityEngine;
using System.Collections;

public class Reward{

	private static GameObject player;
	private int type, value;

	public Reward (int type, int value=0){
		if (player == null) {
			player = GameObject.FindGameObjectWithTag("Player");
		}
		this.type = type;
		this.value = value;
	}

	public int getReward(){
		switch (type) {
		case 0:
			return value;
		}
		return -1;
	}

	public void applyReward(){
		switch (type) {
		case 0:
			PlayerVolume pv = player.GetComponent<PlayerVolume>();
			pv.addLevels(value);
			break;
		}
	}

}
