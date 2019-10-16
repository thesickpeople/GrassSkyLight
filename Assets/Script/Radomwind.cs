using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radomwind : MonoBehaviour {
    public WindZone[] wz;
    // Use this for initialization
    void Start () {
        Cursor.lockState = CursorLockMode.Confined;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        wz[0].transform.Rotate(new Vector3(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f), Random.Range(0.1f, 1f)));
    }
}
