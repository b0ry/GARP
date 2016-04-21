using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using B83.ExpressionParser;

public class TreeSpawn : MonoBehaviour {
	AudioSource audio;
	public AudioClip sampleHit;
	public GameObject treeParent;
	public Transform triangle;
	public Transform square;
	public Transform circle;
	public Transform nasty;
	public Transform nice;
	public Vector3 size;
	public Color shade;
	public int items;
	public List<string> types;
	public int idx = 0;
	public bool flag = false;
	public int level = 1;
	public Color dudColour = new Color(1f,1f,1f,1f);
	public AudioClip sampleBlock;
	private float volume = 1f;
	private enum typeEnum {triangle, square, circle, nice, nasty};

	void Start () {
		audio = GetComponent<AudioSource>();
		SetFirstSize ();
		SetFirstItems ();
		float r = Random.Range (0.0f,0.5f);
		float g = Random.Range (0.5f,1.0f);
		float b = Random.Range (0.0f,0.25f);
		shade = new Color(r,g,b);
		if (gameObject.GetComponentInParent<TreeGA>().level > 1){ // ie not 1st gen. 	The below line is useful for debugging whether or not setting the initial tree is working.
			Color newShade = gameObject.GetComponentInParent<TreeGA>().treeVals[idx].shade;
			shade = newShade;
			size = gameObject.GetComponentInParent<TreeGA>().treeVals[idx].size;
			size += new Vector3 (0.2f,0.5f,0.2f);
			transform.localScale = size;
			List<string> newTypes = gameObject.GetComponentInParent<TreeGA>().treeVals[idx].types;
			types.InsertRange(0,newTypes);
		}
		Renderer treeSkin = this.gameObject.GetComponent<Renderer>();
		treeSkin.material.color = shade;
	}

	void OnCollisionEnter (Collision lngRange) {
		if(lngRange.gameObject.tag=="bullet" && flag == false){
			int playerLevel = GameObject.Find("MyPlayer").GetComponent<PlayerAttackGA>().levels.attackLevel;
			Vector3 yAdd = new Vector3(0f,2f,0f);

			if (level <= playerLevel) {
				audio.PlayOneShot(sampleHit, volume);
				for (int i = 0; i <= items; i++){
					if (types[i] == "triangle" )
						CreateItem (triangle, yAdd);
					if (types[i] == "square" )
						CreateItem (square, yAdd);
					if (types[i] == "circle" )
						CreateItem (circle, yAdd);
					if (types[i] == "nice" )
						CreateItem (nice, yAdd);
					if (types[i] == "nasty" )
						CreateItem (nasty, yAdd);
				}
				flag = true;
				SendMessageUpwards("TreeHit", SendMessageOptions.DontRequireReceiver);
				destroyWithSound ();
				StartCoroutine(destroyWithSound());
			}
			else{
				audio.PlayOneShot(sampleBlock,volume);
				Flash ();
				StartCoroutine(Flash ());
			}
		}
	}
	void SetFirstItems() {
		string tag = gameObject.transform.parent.tag;
		idx = int.Parse (tag);
		for (int i = 0; i <= items; i++){
			for (int j = 0; j <= 5; j++) {
				int type = Random.Range (0,5);
				typeEnum rndType = (typeEnum)type;
				types.Add (rndType.ToString());
			}
		}
	}

	void SetFirstSize() {
		float xz = Random.Range (1f,1.5f);
		float y = Random.Range (1.4f,2.0f);
		size = new Vector3(xz,y,xz);
		transform.localScale = size;
		items = Random.Range (1,(int)Mathf.Ceil (xz*y/3));
	}

	void CreateItem(Transform toMake, Vector3 yAdd) {
		Transform newMake = Instantiate (toMake, transform.position + yAdd, transform.rotation) as Transform;
		newMake.parent = this.transform.parent;
		}

	IEnumerator destroyWithSound() {
		yield return new WaitForSeconds(0.25f);
		Destroy(this.gameObject);
	}

	IEnumerator Flash() {
		gameObject.GetComponent<Renderer>().material.color = dudColour;
		yield return new WaitForSeconds(.1f);
		gameObject.GetComponent<Renderer>().material.color = shade; 
		yield return new WaitForSeconds(.1f);
	}

}
