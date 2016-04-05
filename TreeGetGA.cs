using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeGetGA : MonoBehaviour {

	// Use this for initialization
	public void MidTree(int trngl, int sqr, int crcl, int nice, int nasty, Vector3 size, Color shade, List<string> types){
		gameObject.GetComponentInParent<TreeGA>().RankTree(trngl,sqr,crcl,nice,nasty,size,shade,types);
	}

}
