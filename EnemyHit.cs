using UnityEngine;
using System.Collections;
using GARP.GA;

public class EnemyHit : MonoBehaviour {
	public GameObject slash;
	public Transform main;
	public int damage4List = 0;
	private Transform myPlayer; 
	private float timer = 1f;

	void Update(){

	}
	// Update is called once per frame
	void OnCollisionEnter (Collision projectile) {
		if(projectile.gameObject.tag=="bullet"){
			GetComponentInChildren<ShowText>().counter = 50;
			GetComponentInChildren<ShowText>().DisplayText(damage4List.ToString ());	

			float weight = projectile.transform.GetComponent<Throw>().weight * 10f;
			Attack attack = new Attack();
			attack.damage = GameObject.Find("MyPlayer").GetComponent<PlayerAttackGA>().damageOUT + (int)weight;
			attack.range = projectile.transform.GetComponent<Throw>().range;
			attack.x = projectile.transform.GetComponent<PlayerAttack>().outputX;
			attack.z = projectile.transform.GetComponent<PlayerAttack>().outputZ;

			GameObject player = GameObject.Find("MyPlayer");
			player.GetComponent<PlayerAttackGA>().AddToAttackList(attack);

		}
	}
}
