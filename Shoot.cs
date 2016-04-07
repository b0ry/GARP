using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shoot : MonoBehaviour {
	public GameObject projectile;
	public GameObject holder;
	public GameObject slash;


	//Need to get $ input for Fire 2 
	void Update() {
		if( Input.GetMouseButtonDown(0)) {InvokeRepeating("PlayerFire",0f,0.5f);}
		if( Input.GetMouseButtonUp(0)) {CancelInvoke("PlayerFire");}
		if( Input.GetMouseButtonDown(1)) {InvokeRepeating("PlayerMelee",0f,1f);}
		if( Input.GetMouseButtonUp(1)) {CancelInvoke("PlayerMelee");}
	}
	void PlayerFire(){
		bool flag = gameObject.GetComponentInChildren<NewWalk>().flag;
		if (!flag){
		GameObject h = (GameObject)Instantiate(holder,new Vector3(transform.localPosition.x, transform.localPosition.y+0.5f, transform.localPosition.z+1f),transform.localRotation);
		GameObject g = (GameObject)Instantiate(projectile,h.transform.position,h.transform.rotation);
		//gameObject.AddComponent<Rigidbody>();
		//GameObject player =  GameObject.Find("MyPlayer");
			g.transform.SetParent (h.transform);
			Destroy (g,3f);
			Destroy (h,3f);
		}
	}
	void PlayerMelee(){
		bool flag = gameObject.GetComponentInChildren<NewWalk> ().flag;
		if (!flag) {
			GameObject f = (GameObject)Instantiate (slash, new Vector3(transform.localPosition.x,transform.localPosition.y+1f,transform.localPosition.z+1f), transform.localRotation);
			//f.AddComponent<Rigidbody> ();
			f.transform.SetParent (transform);
			f.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x+90f,transform.localEulerAngles.y,transform.localEulerAngles.z);
		}
	}
}
