using UnityEngine;
using System.Collections;

public class FinalBossBehaviour : MonoBehaviour {

	public GameObject player;
	public GameObject meteorParent;
	public GameObject earth;
	private Rigidbody rb;
	private float moveSpeed;
	private Transform currTarget;
	public bool finished = false;
	private bool attackMode = true;

	private float growCount = 7;
	private float attackCount = 7;

	private bool hasGrown = false;
	private int nomCount = 1;
	private bool recentAttack = false;

	private float baseMS;

	private DestructibleBehaviour db;

	void Start () {
		db = GetComponent<DestructibleBehaviour> ();
		rb = GetComponent<Rigidbody> ();
		moveSpeed = transform.localScale.x * 2f;
	}

	void OnCollisionEnter(Collision c){
		if (c.collider.tag == "Destructible") {
			if (c.collider.gameObject == currTarget.gameObject) {
				nomCount++;
				Destroy (c.collider.gameObject);
				currTarget = getTarget ();
				hasGrown = false;
				if (!attackMode){
					recentAttack = false;
				}
			} else {
				nomCount++;
				Destroy (c.collider.gameObject);
				hasGrown = false;
				if (!attackMode){
					recentAttack = false;
				}
			}
		} else if (c.collider.tag == "Player") {
			PlayerVolume pv = c.collider.GetComponent<PlayerVolume>();
			Rigidbody prb = c.collider.GetComponent<Rigidbody>();
			if (prb != null){
				Vector3 newforce = transform.forward.normalized * 10;
				newforce.y = 0;
				prb.AddForce(newforce);
			}
			pv.percentShrink(0.9f);
			currTarget = getTarget();
			attackMode = false;
		}
	}

	Transform getTarget(){
		if (meteorParent.transform.childCount > 0) {
			int i = Mathf.FloorToInt(Random.Range(0, meteorParent.transform.childCount - (0.01f)));
			return meteorParent.transform.GetChild(i);
		} else {
			return null;
		}
	}

	bool canAttackPlayer(){
		return player.transform.lossyScale.x > 0.1f * transform.lossyScale.x;
	}

	void checkNoms(){
		if (!hasGrown && nomCount % growCount == 0){
			float scaling = transform.localScale.x * 1.1f;
			transform.localScale = Vector3.one * Mathf.Round (scaling);
			hasGrown = true;
		}
		if (nomCount % attackCount == 0 && !recentAttack && canAttackPlayer()) {
			attackMode = true;
			recentAttack = true;
		}
	}

	void move (){
		if (rb != null) {
			if (attackMode) {
				moveSpeed = transform.localScale.x * 4f;
				currTarget = player.transform;
			} else {
				moveSpeed = transform.localScale.x * 2f;
			}
			Vector3 desiredForward = (currTarget.position - transform.position).normalized;
			desiredForward.y = 0;
			transform.forward = desiredForward;
			rb.velocity = new Vector3 (transform.forward.x * moveSpeed, 
		                           this.rb.velocity.y, 
		                           transform.forward.z * moveSpeed);
			transform.Rotate (new Vector3 (0, -90, 0));
		}
	}

	void checkFinished(){
		if (transform.lossyScale.x > 1.5f * earth.transform.lossyScale.x) {
			finished = true;
		}
	}

	void Update () {
		checkFinished ();
		if (!db.getStartShrink ()) {
			if (rb != null && rb.velocity.y > 0) {
				Vector3 velo = rb.velocity;
				velo.y = 0;
				rb.velocity = velo;
			}
			if (!finished) {
				print (nomCount);
				checkNoms ();
				if (currTarget == null) {
					currTarget = getTarget ();
				} else {
					move ();
				}
			}
		} else {
			rb.velocity = Vector3.zero;
		}
	}
}
