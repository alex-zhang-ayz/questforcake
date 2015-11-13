using UnityEngine;
using System.Collections;

public interface Spawner{
	
	void startSpawn();
	bool getFinish();
	bool getFailed();
	void resetAll();
}
