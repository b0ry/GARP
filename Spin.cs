using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {
	float rotationAmount = 180.0f;
	void Update()
	{
		transform.Rotate(Vector3.forward);
		/*Vector3 rot = transform.rotation.eulerAngles;
		rot.x = rot.x + rotationAmount * Time.deltaTime;
		if(rot.x > 360){
			rot.x -= 360;
		}
		else if(rot.x < 360){
			rot.x += 360;
		}
		transform.eulerAngles = rot;*/
	}
}
