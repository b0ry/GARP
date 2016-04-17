using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PlayerJumpGA : MonoBehaviour {
	public List<float> jumpIN = new List<float>();
	public int level = 1;
	public float timer;
	public float jumpOUT = 0.0f;

	void Start(){
		timer = 0f;
	}
	void Update(){
		timer += Time.deltaTime;
	}

	public void AddToList(float jump) {
		jumpIN.Add(jump);
		if (timer >= 60f * (float)level) {
			timer = 0f;
			CrossoverMutation();
		}
	
	}
	void CrossoverMutation(){
		int hits = gameObject.GetComponent<ThirdPersonController> ().hits;
	 	jumpOUT = jumpIN.Average () / 10f - 0.01f*(float)hits;
		level++;
		jumpIN.Clear ();
		gameObject.GetComponent<Morph>().MorphNGrow ("circle");
	}
}