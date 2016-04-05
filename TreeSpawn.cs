using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using B83.ExpressionParser;

public class TreeSpawn : MonoBehaviour {
	AudioSource audio;
	public AudioClip sampleHit;
	private float volume = 1f;
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
	private bool flag = false;
	public int level;
	public Color dudColour;
	public AudioClip sampleBlock;
	
	void Start () {
		audio = GetComponent<AudioSource>();

		float xz = Random.Range (1.5f,2.2f);
		float y = Random.Range (2.0f,5.0f);
		size = new Vector3(xz,y,xz);
		transform.localScale = size;

		items = Random.Range (1,(int)Mathf.Ceil (xz*y/3));
		string tag = gameObject.transform.parent.tag;
		idx = int.Parse (tag);
		for (int i = 0; i <= items; i++){
			int type = Random.Range (1,6);
			if (type == 1) {types.Add ("triangle");}
			if (type == 2) {types.Add ("square");}
			if (type == 3) {types.Add ("circle");}
			if (type == 4) {types.Add ("nice");}
			if (type == 5) {types.Add ("nasty");}
		}
		float r = Random.Range (0.0f,0.5f);
		float g = Random.Range (0.5f,1.0f);
		float b = Random.Range (0.0f,0.25f);
		Renderer treeSkin = this.gameObject.GetComponent<Renderer>();
		shade = new Color(r,g,b);

		Color newShade = treeParent.transform.GetComponent<TreeGA>().shades[idx-1];
		if (newShade != new Color (0f,0f,0f,0f)){
			shade = newShade;
			size = treeParent.transform.GetComponent<TreeGA>().sizes[idx-1];
			size += new Vector3 (0.2f,0.5f,0.2f);
			transform.localScale = size;
			List<string> newTypes = treeParent.transform.GetComponent<TreeGA>().typess[idx-1];
			types.InsertRange(0,newTypes);
			// level++;
		}
		treeSkin.material.color = shade;
		dudColour = new Color(1f,1f,1f,1f);
	}
	
	// Update is called once per frame
	void OnCollisionEnter (Collision lngRange) {
		if(lngRange.gameObject.tag=="bullet" && flag == false){
			int playerLevel = GameObject.Find("MyPlayer").GetComponent<PlayerAttackGA>().level;
			if (level <= playerLevel) {
				audio.PlayOneShot(sampleHit, volume);
				for (int i = 0; i <= items; i++){
					if (types[i] == "triangle") { 
						Transform trngl = Instantiate(triangle, transform.position, transform.rotation) as Transform;
						trngl.parent = this.transform.parent;
					}
					if (types[i] == "square") { 
						Transform sqr = Instantiate (square, transform.position, transform.rotation) as Transform;
						sqr.parent = this.transform.parent;
					}
					if (types[i] == "circle") { 
						Transform crcl = Instantiate (circle, transform.position, transform.rotation) as Transform;
						crcl.parent = this.transform.parent;
					}
					if (types[i] == "nice") { 
						Transform nicely = Instantiate(nice, transform.position, transform.rotation) as Transform;
						nicely.parent = this.transform.parent;
					}
					if (types[i] == "nasty") { 
						Transform naughty = Instantiate (nasty, transform.position, transform.rotation) as Transform;
						naughty.parent = this.transform.parent;
					}
				}
				flag = true;
				gameObject.GetComponentInParent<TreeChromosones>().TreeHit();
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
	IEnumerator destroyWithSound()
	{
		yield return new WaitForSeconds(0.25f);
		Destroy(this.gameObject);
	}
	IEnumerator Flash() 
	{
		gameObject.GetComponent<Renderer>().material.color = dudColour;
		yield return new WaitForSeconds(.1f);
		gameObject.GetComponent<Renderer>().material.color = shade; 
		yield return new WaitForSeconds(.1f);
	}

}
