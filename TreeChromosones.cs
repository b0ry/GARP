using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GARP.GA;

public class TreeChromosones : MonoBehaviour {
	public Chromosones tree = new Chromosones();
	public int triangles;
	public int squares;
	public int circles;
	public int nice;
	public int nasty;
	/*public Vector3 size;
	public Color shade;
	public List<string> types;
	private Bush tree = new Bush();*/

	public void TreeHit(){
		tree.size = gameObject.GetComponentInChildren<TreeSpawn>().size;
		tree.shade = gameObject.GetComponentInChildren<TreeSpawn>().shade;
		tree.types = gameObject.GetComponentInChildren<TreeSpawn>().types;
		tree.triangles = triangles;
		tree.squares = squares;
		tree.circles = circles;
		tree.nice = nice;
		tree.nasty = nasty;
		countdownTree ();
		StartCoroutine (countdownTree ());
	}

	IEnumerator countdownTree() {
		yield return new WaitForSeconds(10.0f);
		//gameObject.GetComponentInParent<TreeGetGA>().MidTree(triangles,squares,circles,nice,nasty,size,shade,types);
		SendMessageUpwards("AddTree", tree,  SendMessageOptions.DontRequireReceiver);
	}
}
