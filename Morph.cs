using UnityEngine;
using System.Linq;
using System.Collections;

public class Morph : MonoBehaviour {
	private GameObject trngl;
	private GameObject cube;
	private GameObject sphere;
	// Use this for initialization
	void Start () {
		trngl = GameObject.FindGameObjectWithTag("tetra");
		cube = GameObject.FindGameObjectWithTag("cube");
		sphere = GameObject.FindGameObjectWithTag("sphere");
	}

	public void MorphNGrow(string type){
		float triRange = (float)gameObject.GetComponent<PlayerAttackGA>().rangeOUT;
		float triDamage = (float)gameObject.GetComponent<PlayerAttackGA>().damageOUT;
		float triGrow = (triRange + (triDamage/5))/2;
		float triPos = triGrow*2f;

		int sqrHealth = GameObject.Find ("Health Bar").GetComponent<HealthBar> ().no_cells;
		float sqrAdd = (float)sqrHealth / 10f;
		float sqrBlock = gameObject.GetComponent<PlayerBlockGA>().average;
		float sqrGrow = sqrBlock/50f;
		float sqrPos = sqrGrow*2f;

		float cirStrafe = (float)gameObject.GetComponentInChildren<PlayerStrafeGA>().strafeOUT;
		float cirJump = (float)gameObject.GetComponent<PlayerJumpGA>().jumpOUT;
		float cirRun = (float)gameObject.GetComponent<PlayerRunGA>().runOUT;
		float cirGrow = (cirJump + cirRun + cirStrafe)/2;
		float cirPos = cirGrow*2f;

		trngl.transform.localRotation *= Quaternion.Euler (new Vector3(Random.Range (0f,360f),Random.Range (0f,360f),Random.Range (0f,360f))); 
		cube.transform.localRotation *= Quaternion.Euler (new Vector3(Random.Range (0f,360f),Random.Range (0f,360f),Random.Range (0f,360f))); 
		sphere.transform.localRotation *= Quaternion.Euler (new Vector3(Random.Range (0f,360f),Random.Range (0f,360f),Random.Range (0f,360f))); 

		trngl.transform.localPosition = new Vector3(Random.Range (-triPos,triPos),Random.Range (-triPos,triPos)+triPos,Random.Range (-triPos,triPos)); 
		cube.transform.localPosition = new Vector3(Random.Range (-sqrPos,sqrPos),Random.Range (-sqrPos,sqrPos)+sqrPos,Random.Range (-sqrPos,sqrPos)); 
		sphere.transform.localPosition = new Vector3(Random.Range (-cirPos,cirPos),Random.Range (-cirPos,cirPos)+cirPos,Random.Range (-cirPos,cirPos)); 

		if (type == "triangle"){ trngl.transform.localScale = new Vector3(1f+triGrow,1f+triGrow,1f+triGrow); }
		if (type == "square"){ cube.transform.localScale = new Vector3(sqrAdd+sqrGrow,sqrAdd+sqrGrow,sqrAdd+sqrGrow); }
		if (type == "circle"){ sphere.transform.localScale = new Vector3(1.2f+cirGrow,1.2f+cirGrow,1.2f+cirGrow); }

	}
}
