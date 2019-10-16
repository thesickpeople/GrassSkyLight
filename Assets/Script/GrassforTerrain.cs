using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassforTerrain : MonoBehaviour {
    public Mesh quad;
    public Terrain terrain;
    public Material grassmat;
    public Texture2D highmap;
    private Transform parentts;
    private GameObject[] grass;
    void Start() {
        parentts = this.transform;
        grass = new GameObject[10000];
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++) {
                grass[i] = new GameObject();
                grass[i].transform.parent = parentts;
                grass[i].AddComponent<MeshFilter>().mesh = quad;
                MeshRenderer meshrender = grass[i].AddComponent<MeshRenderer>();
                meshrender.material = grassmat;
                grass[i].transform.position =new Vector3(i,highmap.GetPixel(Mathf.FloorToInt(i*5.12f),512-Mathf.FloorToInt(j*5.12f)).grayscale*300+1f,j);
            }
        }
        MaterialPropertyBlock props = new MaterialPropertyBlock();
        MeshRenderer renderer;
        foreach (Transform obj in parentts)
        {
            float r = Random.Range(0.086f, 0.535f);
            // float g = Random.Range(0.0f, 1.0f);
            float b = Random.Range(0.18f, 0.535f);
            props.SetColor("_Color", new Color(r, .859f, b));
            renderer = obj.GetComponent<MeshRenderer>();
            renderer.SetPropertyBlock(props);
        }
    }
	void Update () {
		
	}
}
