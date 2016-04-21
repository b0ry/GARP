using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {
	float rotationAmount = 180.0f;
	void Update()
	{
		transform.Rotate(Vector3.forward);
	}
}
