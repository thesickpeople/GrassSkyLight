using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    public Transform objects;
    public Transform perant;
	// Use this for initialization
	void Start () {

        MaterialPropertyBlock props = new MaterialPropertyBlock();
        MeshRenderer renderer;

            float r = Random.Range(0.0f, 1.0f);
            float g = Random.Range(0.0f, 1.0f);
            float b = Random.Range(0.0f, 1.0f);
            props.SetColor("_Color", new Color(r, g, b));
            renderer = objects.GetComponent<MeshRenderer>();
            renderer.SetPropertyBlock(props);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
