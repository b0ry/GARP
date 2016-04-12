using UnityEngine;
using System.Collections;

public class PlayerBlock : MonoBehaviour {
	public Camera myCamera;
	public int hits;
	public GameObject hit;
	public float blockEffect ;
	private int counter = 0;
	public bool flag;
	public int damage;

	void Strafe (string lr){
		gameObject.GetComponentInChildren<Renderer>().material.color = gameObject.GetComponentInChildren<NewWalk>().normal;
		flag = false;
		for (int i = 0; i < 10; i++){
			transform.Rotate(Vector3.forward, 10f * Time.deltaTime);
		}
	}

	void FixedUpdate() {
		TextMesh damCount = GetComponentInChildren<TextMesh>();
		damCount.transform.rotation = Quaternion.LookRotation(damCount.transform.position - Camera.main.transform.position);
		// Block
		if (Input.GetKeyDown(KeyCode.S)){
			flag = true;
		}
		if (Input.GetKey(KeyCode.S)){
			for (int i = 0; i < 3; i++){
				damage= Random.Range (1,11);
				float rnd = ((float)damage/40f)+0.25f;
				gameObject.transform.GetChild (i).GetComponent<Renderer>().material.color = new Color (rnd,rnd,1f,1f);
			}
			/*
			if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S)){
				Strafe("Left");
			}
			if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S)){
				Strafe("Right");
			}*/
		}
		// Not Block
		if (!Input.GetKey(KeyCode.S))
		{
			flag = false;
			for (int i = 0; i < 3; i++){
				gameObject.transform.GetChild (i).GetComponent<Renderer>().material.color = gameObject.transform.GetChild (i).GetComponent<NewWalk>().normal;
			}
		}
	}

	void OnCollisionEnter(Collision slash){
		if (slash.gameObject.tag == "slash"){
			GameObject healthBar = GameObject.Find("Health Bar");
			int level = transform.GetComponent<PlayerBlockGA>().level;
			if (!flag){
				healthBar.GetComponent<HealthBar>().hit = damage;
				Rigidbody g = Instantiate(hit,transform.position,transform.rotation)as Rigidbody;
				/////////////////////////
				for (int i = 0; i < 3; i++){
					gameObject.transform.GetChild (i).GetComponent<PlayerCell>().Flash ();
				}
				////////////////////////
			}
			else if (level == 1) {
				blockEffect = Random.value;
				float newDam = (1-blockEffect) * (float)damage;
				healthBar.GetComponent<HealthBar>().hit = (int)newDam;
				TextMesh damCount = GetComponentInChildren<TextMesh>();
				blockEffect *= 100;
				int block4text = (int)blockEffect;
				damCount.text = block4text.ToString () + "%";
				counter = 50;
				transform.GetComponent<PlayerBlockGA>().AddToList(block4text);
				hits++;
			}
			else {
				int rnd = Random.Range (0,transform.GetComponent<PlayerBlockGA>().blockEffectOUT.Count);
				int block4text = transform.GetComponent<PlayerBlockGA>().blockEffectOUT[rnd];
				blockEffect = 1-((float)block4text/100f);
				//block4text = 100 - block4text;
				int mutation = Random.Range (0,10);
				if (mutation == 0) { 
					blockEffect = Random.Range(0.0f, 0.5f);
					blockEffect *= 100;
					block4text = 100-(int)blockEffect;
					Debug.Log (blockEffect);
				}
				float newDam = blockEffect * (float)damage;
				healthBar.GetComponent<HealthBar>().hit = (int)newDam;
				TextMesh damCount = GetComponentInChildren<TextMesh>();
				damCount.text = block4text.ToString () + "%";
				counter = 50;
				int idx = transform.GetComponent<PlayerBlockGA>().i;
				if (idx == 0) { 
					transform.GetComponent<PlayerBlockGA>().blockEffectIN.Clear();
				}
				transform.GetComponent<PlayerBlockGA>().AddToList(block4text);
			}
		}
	}
}

