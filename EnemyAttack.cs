using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {
	private Transform myPlayer; 
	public GameObject slash;
	//public bool flag = false;
	private float timer = 1f;
	// Use this for initialization
	void Start () {
		myPlayer = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		timer += Time.deltaTime;
		float mag;
		float x = myPlayer.position.x - this.transform.position.x;
		float z = myPlayer.position.z - this.transform.position.z;
		mag = new Vector2(x,z).magnitude; 

		if (mag < 5 && timer >= 1f) {
			EnemyAttacksPlayer ();
			StartCoroutine(EnemyAttacksPlayer ());
			timer = 0f;
		}

	}
	IEnumerator EnemyAttacksPlayer () {
		yield return new WaitForSeconds(0.5f);
		GameObject h = (GameObject)Instantiate(slash,transform.position,transform.rotation * Quaternion.Euler(90f,0f,0f));
		h.transform.parent = this.transform;
		Destroy (h, 0.7f);
		//flag = true;
	}
}
