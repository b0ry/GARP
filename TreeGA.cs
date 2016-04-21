using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GARP.GA;

public class TreeGA : MonoBehaviour {
	public int level = 1;
	public int treeCounter;
	public int[] indices = new int[7];
	public int[] fitness = new int[7];
	public List<Chromosones> treeVals = new List<Chromosones>();
	public GameObject tree;
	private int i =0;
	private int children = 7;
	private GameObject treeCone;
	
	void CreateNewTree() {
		for (int j = 0; j < children; j++) {
			int number = j + 1;
			GameObject[] treeCones = new GameObject[10];
			treeCones = GameObject.FindGameObjectsWithTag (number.ToString ());
			for (int k = 0; k < treeCones.Length; k++) {
				for (int kk = 0; kk < children; kk++) {
					if (treeCones [k].transform.IsChildOf (transform.GetChild (kk))) {
						treeCone = treeCones [k];
						break;
					}
				}
			} 
			
			GameObject newTree = Instantiate (tree, treeCone.transform.position, treeCone.transform.rotation) as GameObject;
			newTree.transform.parent = treeCone.transform;
			newTree.GetComponent<TreeSpawn> ().treeParent = gameObject;
			newTree.GetComponent<TreeSpawn> ().level = level;
			
		}
	}
	
	Chromosones Sum (List<Chromosones> chrL) {
		Chromosones[] chrA = chrL.ToArray();
		Chromosones chrOUT = new Chromosones ();
		for (int i = 0; i < chrA.Length; i++) {
			Chromosones chrAdd = chrA[i];
			chrOUT.triangles += chrAdd.triangles; 
			chrOUT.squares += chrAdd.squares; 
			chrOUT.circles += chrAdd.circles; 
			chrOUT.nasty += chrAdd.nasty; 
			chrOUT.nice += chrAdd.nice; 
		}
		return chrOUT;
	}
	
	Color GetColour () {
		Chromosones sum = Sum(treeVals);
		Color maxHue = new Color (0.15f,-0.1f,0f,0f);
		if (sum.squares > sum.triangles) maxHue = new Color(-0.1f,0f,0.15f,0f);
		if (sum.circles > sum.squares) maxHue = new Color(0.1f,0.075f,-0.15f,0f);
		if (sum.nasty > sum.circles) maxHue = new Color(0.08f,0.085f,0.075f,0f);
		if (sum.nice >sum.nasty) maxHue = new Color(-0.085f,-0.075f,-0.8f,0f);
		return maxHue;
	}

	public void AddTree(Chromosones treeIN){
		treeVals.Add(treeIN);
		i++;
		if (i == children) {
			i = 0;
			level++;
			Color addHue = GetColour();
			for (int j=0; j < children; j++){
				float rndR = Random.Range (0.01f,0.05f);
				float rndG = Random.Range (0.01f,0.05f);
				float rndB = Random.Range (0.01f,0.05f);
				treeVals[j].shade += addHue + new Color(rndR,rndG,rndB,0f);
			}
			CreateNewTree();
		}
	} 
}
