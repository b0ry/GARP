using UnityEngine;
using System.Collections;
using GARP.GA;

public class EnemyController : MonoBehaviour {
	private Transform myPlayer; 
	public GameObject slash;
	private float timer = 1f;
	void Start () {
		myPlayer = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		timer += Time.deltaTime;
		float x = myPlayer.position.x - this.transform.position.x;
		float z = myPlayer.position.z - this.transform.position.z;
		float mag = new Vector2(x,z).magnitude; 
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

	void OnCollisionEnter (Collision projectile) {
		if(projectile.gameObject.tag=="bullet"){
			float weight = projectile.transform.GetComponent<Throw>().weight * 10f;
			Attack attack = new Attack();
			attack.damage = myPlayer.GetComponent<PlayerAttackGA>().damageOUT + (int)weight;
			attack.range = projectile.transform.GetComponent<Throw>().range;
			attack.x = projectile.transform.GetComponent<PlayerAttack>().outputX;
			attack.z = projectile.transform.GetComponent<PlayerAttack>().outputZ;
			myPlayer.GetComponent<PlayerAttackGA>().AddToAttackList(attack);

			GetComponentInChildren<ShowText>().counter = 50;
			GetComponentInChildren<ShowText>().DisplayText(attack.damage.ToString ());	
			
		}
	}
}
