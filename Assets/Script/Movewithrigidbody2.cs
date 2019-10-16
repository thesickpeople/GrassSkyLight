using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movewithrigidbody2 : MonoBehaviour {
    [Range(0.1f, 10f)]
    public float moveSpeed;
    [Range(.1f, 2.66f)]
    public float jumpHeight;
    [Range(10, 360f)]
    public float rotationSpeed;
    private Animator An;
    private Rigidbody Ri;
    private Transform Chan;
    public Transform CameraforChan;
    private Vector3[] direction;
    private float angle;
    private bool rotating;
    private Matrix4x4 rotate45 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 45, 0), Vector3.one);
    private AnimatorStateInfo currentBaseState;
    static int jumpState = Animator.StringToHash("Base Layer.Jump");
    private bool isjumping;
    void Start()
    {
        An = this.GetComponent<Animator>();
        Ri = this.GetComponent<Rigidbody>();
        Chan = this.transform;
        direction = new Vector3[2];
        rotating = false;
        isjumping = false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) Cursor.visible = false;
    }
    private void LateUpdate()
    {
        Movepos(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        Jump(Input.GetAxis("Jump"));
    }
    private void Jump(float var)
    {
        An.SetBool("Jump", false);
        if (var > 0 && !An.IsInTransition(0)&&!isjumping)
        {
            isjumping = true;
            An.SetBool("Jump", true);
            Ri.velocity = new Vector3(0, Mathf.Sqrt(2 * 9.8f * jumpHeight), 0);
        }
    }
    private void Movepos(float LR,float FB)
    {
        float var = FB != 0 ? FB : LR;
        currentBaseState = An.GetCurrentAnimatorStateInfo(0);
        if (currentBaseState.fullPathHash != jumpState)
        {
            float walk = Input.GetAxis("LSHIFT") > 0 ? 0.01f : 1;
            An.SetFloat("Speed",Mathf.Abs(var)*walk);
            if (var!=0)
            {
                Calculation(FB>0&&LR<0?4: FB > 0 && LR > 0 ? 5:FB < 0 && LR < 0 ?6: FB < 0 && LR > 0 ?7
                    : FB > 0 ? 0 : FB < 0 ? 1 : LR < 0 ? 2 : 3);
                if (!rotating)
                {
                    Ri.MovePosition(Chan.position + direction[1].normalized * Time.fixedDeltaTime * moveSpeed*(walk==0.01f?0.3f:1));
                }
            }
        }
    }
    private void Calculation(int LRFB)
    {
        direction[0] = Chan.forward;
        switch (LRFB) {
            case 4:
                direction[1] = rotate45.MultiplyPoint3x4(-new Vector3(CameraforChan.right.x,0, CameraforChan.right.z));
                break;
            case 5:
                direction[1] = rotate45.MultiplyPoint3x4(new Vector3(CameraforChan.forward.x,0,CameraforChan.forward.z));
                break;
            case 6:
                direction[1] = rotate45.MultiplyPoint3x4(-new Vector3(CameraforChan.forward.x, 0, CameraforChan.forward.z));
                break;
            case 7:
                direction[1] = rotate45.MultiplyPoint3x4(new Vector3(CameraforChan.right.x, 0, CameraforChan.right.z));
                break;
            case 0:
                direction[1] = new Vector3(CameraforChan.forward.x, 0, CameraforChan.forward.z);
                break;
            case 1:
                direction[1] = -new Vector3(CameraforChan.forward.x, 0, CameraforChan.forward.z);
                break;
            case 2:
                direction[1] = -new Vector3(CameraforChan.right.x, 0, CameraforChan.right.z);
                break;
            case 3:
                direction[1] = new Vector3(CameraforChan.right.x, 0, CameraforChan.right.z);
                break;
        }
        angle = (Vector3.Dot(Chan.right, direction[1]) > 0 ? 1 : -1)
                    * Vector2.Angle(new Vector2(direction[0].x, direction[0].z), new Vector2(direction[1].x, direction[1].z));
        if (Mathf.Abs(angle) <90)
        {
            rotating = false;
        }
        else
        {
            rotating = true;
        }
        if (Mathf.Abs(angle) > 10)
        {
            Ri.MoveRotation(Quaternion.Euler(Chan.rotation.eulerAngles+new Vector3(0, angle * rotationSpeed * Time.fixedDeltaTime, 0)));
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        isjumping = false;
    }
    private void OnCollisionStay(Collision collision)
    {
        isjumping = false;
    }
}
