using UnityEngine;
using System.Collections;

public interface CameraAnimation{

	void playAnimation();
	bool getFinished();
	void sendFinalPos(Vector3 l, Vector3 r);

}
