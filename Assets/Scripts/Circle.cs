using UnityEngine;
using System.Collections;

public class Circle : MonoBehaviour 
{   
	//doesn't work
	float theta_scale = 0.01f;        //Set lower to add more points
	int size; //Total number of points in circle
	float r = 1.4f;
	LineRenderer lineRenderer;


	void Awake ()
	{       
		string shaderText = "Shader \"Alpha Additive\" {" + "Properties { _Color (\"Main Color\", Color) = (1,1,1,0) }" + "SubShader {" + "	Tags { \"Queue\" = \"Transparent\" }" + "	Pass {" + "		Blend One One ZWrite Off ColorMask RGB" + "		Material { Diffuse [_Color] Ambient [_Color] }" + "		Lighting On" + "		SetTexture [_Dummy] { combine primary double, primary }" + "	}" + "}" + "}";
		float sizeValue = (2.0f * Mathf.PI)/theta_scale; 
		size = (int)sizeValue;
		size++;
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.enabled = true;
		lineRenderer.material = new Material(shaderText);
		lineRenderer.SetWidth(0.04f, 0.04f); //thickness of line
		lineRenderer.SetVertexCount(size);      
	}
	
	void Start () 
	{      
		int i = 0;
		for(float theta = 0; theta < 2 * Mathf.PI; theta += 0.01f) {
			float x = r*Mathf.Cos(theta);
			float z = r*Mathf.Sin(theta);
			
			Vector3 pos = new Vector3(x, -1.95f, z);
			lineRenderer.SetPosition(i, pos);
			i+=1;
		}
	}
}