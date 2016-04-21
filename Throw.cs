using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using B83.ExpressionParser;

public class Throw : MonoBehaviour {
	public GameObject fireworks;
	private float startTime;
	public float time;
	public float range;
	public float rangeWeight;
	public float weight;
	public float directionX;
	public float directionZ;
	private ExpressionParser parser = new ExpressionParser();
	private float audio;
	private int random;
	private Rigidbody rb;
	private GameObject player;
	private float sine;
	private float cosine;
	private Expression expX;
	private Expression expZ;

	void Start() {
		startTime=Time.time;
		GetRange ();
		audio = GetComponent<AudioSource>().pitch;
		GetPath ();	
		SetQuadrant ();
	}

	void Update() {
		audio -= 0.1f;
		GetComponent<AudioSource>().pitch = audio;
		MoveAlongPath ();
	}

	void OnCollisionEnter (Collision lngRange)
	{
		if (lngRange.gameObject.tag == "enemy") {
			Rigidbody g = Instantiate (fireworks, transform.position, transform.rotation)as Rigidbody;
		}
	}

	void GetRange() {
		player = GameObject.Find("MyPlayer");
		weight = Random.Range(0.1f,0.9f);
		rangeWeight = (1f - weight);
		range = player.GetComponent<PlayerAttackGA>().rangeOUT + (4f + rangeWeight);
		Destroy (gameObject, range);
	}

	void GetPath(){
		expX = parser.EvaluateExpression(GetComponent<PlayerAttack>().outputX);
		expZ = parser.EvaluateExpression(GetComponent<PlayerAttack>().outputZ);
	}

	void SetQuadrant() {
		random = Random.Range (0,2);
		if (random == 0) 
			random = -1;
	}

	void MoveAlongPath() {
		time = ((Time.time-startTime+1.3f)*6)*1f;
		expX.Parameters["time"].Value = time;
		expZ.Parameters["time"].Value = time;
		directionX = (float) expX.Value/10;
		directionZ = (float) expZ.Value/10;
		transform.localPosition += new Vector3(directionX*random, 0f, directionZ+(time/30f));
		transform.localRotation = new Quaternion (0, 0, 0, 0);
	}
}


