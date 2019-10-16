using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grassinstance : MonoBehaviour
{
    public Mesh quad;
    public Material grassmat;
    public Transform plane;
    private Transform parentplane;
    private GameObject[] grasses;
    // Use this for initialization
    void Start()
    {
        parentplane = this.transform;
        grasses = new GameObject[1000];
        for(int i=0;i<1000;i++)
        {
            grasses[i] = new GameObject();
            grasses[i].transform.parent = parentplane;
            grasses[i].AddComponent<MeshFilter>().mesh=quad;
            MeshRenderer meshrender= grasses[i].AddComponent<MeshRenderer>();
            meshrender.material = grassmat;
            grasses[i].transform.position = plane.position + new Vector3(Random.Range(-4.9f, 4.9f), 0.5f, Random.Range(-4.9f, 4.9f));
        }
        MaterialPropertyBlock props = new MaterialPropertyBlock();
        MeshRenderer renderer;
        foreach (Transform obj in parentplane)
        {
            float r = Random.Range(0.086f, 0.535f);
           // float g = Random.Range(0.0f, 1.0f);
            float b = Random.Range(0.18f, 0.535f);
            props.SetColor("_Color", new Color(r, .859f, b));
            renderer = obj.GetComponent<MeshRenderer>();
            renderer.SetPropertyBlock(props);
        }
    }
    void FixedUpdate()
    {

    }
}
