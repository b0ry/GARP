using UnityEngine;
using System.Collections;
using GARP.GA;

public class EnemyHit : MonoBehaviour {

	public int damage4List = 0;
	public float range4List = 0;
	public string xdir4List = "";
	public string zdir4List = "";
	public Transform main;
	private int counter = 0;

	void Update(){
		TextMesh damCount = GetComponentInChildren<TextMesh>();
		damCount.transform.rotation = Quaternion.LookRotation(damCount.transform.position - main.position);
		if (counter > 0) {
			counter--;
		}
		if (counter == 0) {
			damCount.text = "";
		}
	}
	// Update is called once per frame
	void OnCollisionEnter (Collision projectile) {
		if(projectile.gameObject.tag=="bullet"){
			TextMesh damCount = GetComponentInChildren<TextMesh>();
			damCount.text = damage4List.ToString ();		
			counter = 50;
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
