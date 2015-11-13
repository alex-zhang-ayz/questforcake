using UnityEngine;
using System.Collections;

public interface QuestTrigger{
	void reset();
	int getFinishValue();
	string getExtraLines();
}
