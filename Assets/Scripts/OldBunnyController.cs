using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class OldBunnyController : MonoBehaviour, Spawner {
	
	public GameObject player;
	public GameObject carrotGarden;
	public GameObject escapeHole;
	public GameObject ballBunnyPrefab;
	public GameObject boxBunnyPrefab;
	public GameObject cylinderBunnyPrefab;
	private CarrotGardenBehaviour cgb;
	private Vector3[] savedPostitions;
	private Vector3[] savedScales;
	private string[] savedTypes;
	private Dictionary<string, GameObject> prefabDict;
	private List<GameObject> carrots;
	private bool started;
	private bool finished;
	private bool failed;
	
	void Start () {
		cgb = carrotGarden.GetComponent<CarrotGardenBehaviour> ();
		started = false;
		finished = false;
		failed = true;
		prefabDict = new Dictionary<string, GameObject> ();
		instantiateDict ();
		savedTypes = new string[transform.childCount];
		savedPostitions = new Vector3[transform.childCount];
		savedScales = new Vector3[transform.childCount];
		saveBunnies ();
		
		carrots = new List<GameObject> ();
		updateLocalCarrotList ();
		
	}
	
	private void updateLocalCarrotList(){
		carrots.Clear ();
		foreach (Transform child in carrotGarden.transform) {
			carrots.Add(child.gameObject);
		}
	}
	
	public bool getFinish(){
		return finished;
	}
	
	private void saveBunnies(){
		int i = 0;
		foreach (Transform child in transform) {
			BunnyBehaviour bb = child.GetComponent<BunnyBehaviour>();
			if (bb != null){
				savedTypes[i] = bb.getBunnyType ();
				savedPostitions[i] = child.position;
				savedScales[i] = child.localScale;
				i++;
			}
		}
	}
	
	public void resetAll(){
		cgb.reset ();
		updateLocalCarrotList ();
		reset ();
	}
	
	public bool getFailed(){
		return finished && failed;
	}
	
	private void instantiateDict(){
		prefabDict.Add ("ballBunny", ballBunnyPrefab);
		prefabDict.Add ("boxBunny", boxBunnyPrefab);
		prefabDict.Add ("cylBunny", cylinderBunnyPrefab);
	}
	
	private void reset(){
		
		
		started = false;
		finished = false;
		failed = true;
		ExtraFunctions.destroyChildren (this.transform);
		
		for (int i=0; i<savedTypes.Length; i++) {
			GameObject bun = GameObject.Instantiate(prefabDict[savedTypes[i]]) as GameObject;
			bun.transform.localScale = savedScales[i];
			bun.transform.position = savedPostitions[i];
			BunnyBehaviour bb = bun.GetComponent<BunnyBehaviour>();
			bb.escapeHole = this.escapeHole;
			bun.transform.parent = this.transform;
			if (i > 0){
				bb.parentBunny = transform.GetChild(i - 1).gameObject;
			}else{
				bb.noParent = true;
			}
			DestructiblesHandler dh = bun.GetComponent<DestructiblesHandler>();
			dh.player = this.player;
			bun.SetActive(false);
		}
	}
	
	public GameObject getNewCarrot(GameObject g){
		carrots.Remove (g);
		Destroy (g);
		return getNewCarrot ();
		
	}
	
	public GameObject getNewCarrot(){
		if (carrots.Count > 0) {
			int value = Mathf.RoundToInt(Random.Range(0, carrots.Count - 1));
			return carrots [value];
		} else {
			return null;
		}
	}
	
	public void allFlee(){
		foreach (Transform child in transform) {
			BunnyBehaviour bb = child.GetComponent<BunnyBehaviour>();
			if (bb != null){
				bb.flee = true;
			}
		}
	}
	
	private void checkFinish(){
		if (transform.childCount == 0) {
			failed = false;
			finished = true;
		}
		bool b = true;
		foreach (Transform child in transform) {
			BunnyBehaviour bb = child.GetComponent<BunnyBehaviour>();
			if (!bb.getFinished()){
				b = false;
			}
		}
		finished = b;
	}
	
	public void startSpawn(){
		if (!started) {
			int i=0;
			foreach(Transform child in transform){
				child.gameObject.SetActive(true);
				BunnyBehaviour bn = child.GetComponent<BunnyBehaviour>();
				bn.carrot = carrots[i];
				
				if (i >= carrots.Count ){
					i = 0;
				}else{
					i++;
				}
			}
			started = true;
		}
	}
	
	void Update () {
		if (started && !finished) {
			checkFinish ();
		}
	}
}
