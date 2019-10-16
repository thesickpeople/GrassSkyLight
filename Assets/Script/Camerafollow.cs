using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerafollow : MonoBehaviour {
    public Transform Chan;
    [Range(0,180)]
    public float max_X_Euler;
    [Range(-180, 0)]
    public float min_X_Euler;
    [Range(0, 60)]
    public float X_rotateSpeed;
    [Range(0, 60)]
    public float Y_rotateSpeed;
    [Range(1,10)]
    public float XZ_dist;
    [Range(1, 10)]
    public float Y_dist;
    private float current_rotation_X;
    private float current_rotation_Y;
    private float last_rotation_X;
    private float last_rotation_Y;
    private bool[] Xobstacle;
    private bool[] Yobstacle;
	// Use this for initialization
	void Start () {
        Xobstacle = new bool[2];
        Yobstacle = new bool[2];
    }
	
	// Update is called once per frame
	void LateUpdate () {
        current_rotation_X += Input.GetAxis("Mouse Y") * X_rotateSpeed;
        current_rotation_Y+= Input.GetAxis("Mouse X") * Y_rotateSpeed;
        Xobstacle[0] = Physics.Linecast(transform.position, transform.position + new Vector3(0, -1.5f, 0));
        Xobstacle[1] = Physics.Linecast(transform.position, transform.position + new Vector3(0, 1.5f, 0));
        Yobstacle[0] = Physics.Linecast(transform.position, transform.position + transform.TransformDirection(new Vector3(1, 0, 0)));
        Yobstacle[1] = Physics.Linecast(transform.position, transform.position + transform.TransformDirection(new Vector3(-1, 0, 0)));
        Physics.Linecast(transform.position, transform.position + new Vector3(1, 0, 0));
        current_rotation_X = Mathf.Clamp(current_rotation_X, min_X_Euler, max_X_Euler);
        if (Xobstacle[0] && current_rotation_X > last_rotation_X)
        {
            current_rotation_X = last_rotation_X;
        }
        else if(Xobstacle[1]&& current_rotation_X < last_rotation_X)
        {
            current_rotation_X = last_rotation_X;
        }
        if (Yobstacle[0]&&current_rotation_Y<last_rotation_Y)
        {
            current_rotation_Y = last_rotation_Y;
        }
        else if (Yobstacle[1] && current_rotation_Y > last_rotation_Y)
        {
            current_rotation_Y = last_rotation_Y;
        }
        transform.localEulerAngles = new Vector3(-current_rotation_X, current_rotation_Y, 0f);
        transform.position = Chan.position+Vector3.up*0.2f;
        transform.Translate(Vector3.back * XZ_dist, Space.Self);
        transform.Translate(Vector3.up * Y_dist, Space.World);
        last_rotation_X = current_rotation_X;
        last_rotation_Y = current_rotation_Y;
    }
}
