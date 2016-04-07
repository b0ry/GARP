using UnityEngine;
using System.Collections;

public class PlayerBlock : MonoBehaviour {
	public Camera myCamera;
	public int hits;
	public GameObject hit;
	public GameObject hit2;
	public float blockEffect ;
	private int counter = 0;
	private bool flag = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	/*void Update () {
		TextMesh damCount = GetComponentInChildren<TextMesh>();
		//damCount.transform.rotation = Quaternion.LookRotation(damCount.transform.position - myCamera.transform.position);
		if (counter > 0) {
			counter--;
		}
		if (counter == 0) {
			damCount.text = "";
		}
	}*/

	void OnCollisionEnter(Collision slash){
		if (slash.gameObject.tag == "slash"){
			int damage = GetComponentInChildren<NewWalk>().damage;
			bool isBlock = GetComponentInChildren<NewWalk>().flag;
			GameObject healthBar = GameObject.Find("Health Bar");
			int level = transform.GetComponent<PlayerBlockGA>().level;
			if (!isBlock){
				healthBar.GetComponent<HealthBar>().hit = damage;
				Rigidbody g = Instantiate(hit,transform.position,transform.rotation)as Rigidbody;
				Rigidbody h = Instantiate(hit2,transform.position,transform.rotation)as Rigidbody;
				for (int i = 1; i < 4; i++){
					gameObject.transform.GetChild (i).GetComponent<PlayerCell>().Flash ();
				}
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

