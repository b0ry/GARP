using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeChromosones : MonoBehaviour {
	public int triangles;
	public int squares;
	public int circles;
	public int nice;
	public int nasty;
	public Vector3 size;
	public Color shade;
	public List<string> types;

	void Start() {
		size = gameObject.GetComponentInChildren<TreeSpawn>().size;
		shade = gameObject.GetComponentInChildren<TreeSpawn>().shade;
		types = gameObject.GetComponentInChildren<TreeSpawn>().types;
	}

	public void TreeHit(){
		size = gameObject.GetComponentInChildren<TreeSpawn>().size;
		shade = gameObject.GetComponentInChildren<TreeSpawn>().shade;
		types = gameObject.GetComponentInChildren<TreeSpawn>().types;
		countdownTree ();
		StartCoroutine (countdownTree ());
	}

	IEnumerator countdownTree() {
		yield return new WaitForSeconds(10.0f);
		Debug.Log ( this.gameObject.transform.root.name );
		//gameObject.GetComponentInParent<TreeGetGA>().MidTree(triangles,squares,circles,nice,nasty,size,shade,types);
		gameObject.GetComponentInParent<TreeGA>().RankTree(triangles,squares,circles,nice,nasty,size,shade,types);
	}
}
