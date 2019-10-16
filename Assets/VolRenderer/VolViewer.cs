using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VolViewer : MonoBehaviour {
	public Renderer ViewCube;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (ViewCube) {
			Vector3 p = transform.position;
			Vector3 s = transform.lossyScale * 0.5f;
			ViewCube.sharedMaterial.SetVector ("_BoundsCenter", new Vector4 (p.x, p.y, p.z));
			ViewCube.sharedMaterial.SetVector ("_BoundsSize", new Vector4 (s.x, s.y, s.z));
			ViewCube.sharedMaterial.SetMatrix ("_WorldToLocalMatrix", transform.worldToLocalMatrix);
			ViewCube.sharedMaterial.SetMatrix ("_LocalToWorldMatrix", transform.localToWorldMatrix);
		}
	}
}
