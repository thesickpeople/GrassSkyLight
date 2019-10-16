using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movewithrigidbody : MonoBehaviour {
    [Range(0.1f,10f)]
    public float moveSpeedfront;
    [Range(0.1f, 10f)]
    public float moveSpeedback;
    [Range(.1f, 2.66f)]
    public float jumpHeight;
    [Range(10, 360f)]
    public float rotationSpeed; 
    private Animator An;
    private Rigidbody Ri;
    //private Collider Co;
    private Transform Chan;
    private AnimatorStateInfo currentBaseState;
    static int jumpState = Animator.StringToHash("Base Layer.Jump");
    void Start () {
        An = this.GetComponent<Animator>();
        Ri = this.GetComponent<Rigidbody>();
        Chan = this.transform;
	}
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))Cursor.visible = false;
    }
    void FixedUpdate () {
        currentBaseState = An.GetCurrentAnimatorStateInfo(0);//得到动画状态机Layers0的状态信息
        if (currentBaseState.fullPathHash != jumpState)
        {
            if (!An.IsInTransition(0))
            {
                An.SetBool("Jump", false);
            }
            Frontbackmove(Input.GetAxis("Vertical"));
            Jump(Input.GetButtonDown("Jump"));
            Rotatechange(Input.GetAxis("Horizontal"));
        }
        else
        {
            float ver = Input.GetAxis("Vertical");
            if (ver > 0)
            {
                Ri.AddForce(Chan.TransformDirection(new Vector3(0, 0, ver * moveSpeedfront * Time.fixedDeltaTime*36)),ForceMode.VelocityChange);
            }
        }
        
    }
    private void Frontbackmove(float ver)
    {
        if (Input.GetAxis("LSHIFT") > 0) ver *= .36f;
        An.SetFloat("Speed", ver);
        if (ver > 0.1f)
        {
            Ri.MovePosition(Chan.position+Chan.TransformDirection(new Vector3(0, 0, ver * moveSpeedfront*Time.fixedDeltaTime)));
        }
        else if(ver < -0.1f)
        {
            Ri.MovePosition(Chan.position+Chan.TransformDirection(new Vector3(0, 0, ver * moveSpeedback * Time.fixedDeltaTime)));
        }
    }
    private void Jump(bool ver)
    {
        if (ver)
        {
           An.SetBool("Jump", true);
           Ri.velocity = new Vector3(0, Mathf.Sqrt(2 * 9.8f * jumpHeight),0);
        }
    }
    public  void Rotatechange(float ver)
    {
        An.SetFloat("Direction", ver);
        if (ver != 0)
        {
            Ri.MoveRotation(Quaternion.Euler(Chan.rotation.eulerAngles+new Vector3(0,ver*rotationSpeed*Time.fixedDeltaTime,0)));
        }
    }
}
