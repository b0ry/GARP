﻿using UnityEngine;
using System.Collections;

public class Slash : MonoBehaviour {
	private float timer = 0f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= 0.583f) {
			Destroy (gameObject);
		}
	}
}
