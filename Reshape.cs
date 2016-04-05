using UnityEngine;
using System.Collections;

public class Reshape : MonoBehaviour {
	private int maxLat = 12;
	private int maxLng = 24;
	public float radius = 0.5f;	
	private float[] scalars = new float[8];
	private float[] phases = new float[2];


	public void ChangeShape() {
		Mesh mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		mesh.Clear ();
		Vector3[] shape = new Vector3[(maxLng+1)*maxLat+2];
		Vector3[] normals = new Vector3[shape.Length];
		float _pi = Mathf.PI;
		float _2pi = _pi * 2f;
		//shape[0] = Vector3.up*radius;

		for (int j = 0; j < 8; j++)
		{
			scalars[j] = Random.Range(0.0f,1.5f);
		}
		phases[0] = Random.Range (0.01f,3.14f);
		phases[1] = Random.Range (0.01f,3.14f);

		for( int lat = 0; lat < maxLat; lat++ )
		{
			float a1 = _2pi * (float)(lat) / (maxLat+1);
			float sin1 = Mathf.Sin(a1);
			float cos1 = Mathf.Cos(a1);

			for( int lng = 0; lng <= maxLng; lng++ )
			{
				float a2 = _2pi * (float)(lng == maxLng ? 0 : lng) / maxLng;
				float sin2 = Mathf.Sin(phases[0]-a2);
				float cos2 = Mathf.Cos(phases[1]-a2);
				
				shape[ lng + lat * (maxLng + 1) + 1] = new Vector3((scalars[0]*sin1+scalars[1]*cos2)*(scalars[2]*sin2+scalars[3]*cos1),
				                                                   (scalars[4]*cos1)+(scalars[5]*sin2*sin2),
				                                                   (scalars[6]*sin1*sin1)+(scalars[7]*sin2*sin2)) * radius;
			}
		}


		for( int n = 0; n < shape.Length; n++ )
		{
			normals[n] = shape[n].normalized;
		}
		Vector2[] uvs = new Vector2[shape.Length];
		uvs[0] = Vector2.up;
		uvs[uvs.Length-1] = Vector2.zero;
		for( int lat = 0; lat < maxLat; lat++ )
			for( int lng = 0; lng <= maxLng; lng++ )
				uvs[lat*(maxLng+1)+lng+1] = new Vector2( (float)lng / maxLng, 1f -(float)(lat+1) / (maxLat+1) );

		int nbFaces = shape.Length;
		int nbTriangles = nbFaces * 2;
		int nbIndexes = nbTriangles * 3;
		int[] triangles = new int[ nbIndexes ];
		
		//Top Cap
		int i = 0;
		for( int lng = 0; lng < maxLng; lng++ )
		{
			triangles[i++] = lng+2;
			triangles[i++] = lng+1;
			triangles[i++] = 0;
		}
		
		//Middle
		for( int lat = 0; lat < maxLat - 1; lat++ )
		{
			for( int lng = 0; lng < maxLng; lng++ )
			{
				int current = lng + lat * (maxLng + 1) + 1;
				int next = current + maxLng + 1;
				
				triangles[i++] = current;
				triangles[i++] = current + 1;
				triangles[i++] = next + 1;
				
				triangles[i++] = current;
				triangles[i++] = next + 1;
				triangles[i++] = next;
			}
		}
		
		//Bottom Cap
		for( int lng = 0; lng < maxLng; lng++ )
		{
			triangles[i++] = shape.Length - 1;
			triangles[i++] = shape.Length - (lng+2) - 1;
			triangles[i++] = shape.Length - (lng+1) - 1;
		}

		mesh.vertices = shape;
		mesh.normals = normals;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
		mesh.Optimize ();

		//RectTransform rt = gameObject.GetComponent<RectTransform>();
		//float pos = rt.rect.height/2f;
		//this.transform.position = new Vector3 (transform.position.x,Mathf.Max (scalars),transform.position.z);
	}
}

