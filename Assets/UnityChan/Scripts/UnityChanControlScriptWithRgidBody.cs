using UnityEngine;
using System.Collections;
[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (CapsuleCollider))]
[RequireComponent(typeof (Rigidbody))]

public class UnityChanControlScriptWithRgidBody : MonoBehaviour
{
	public float animSpeed = 1.5f;				
	public bool useCurves = true;				//是否使用
	public float useCurvesHeight = 0.5f;		// 高度修正？
	public float forwardSpeed = 7.0f;
	public float backwardSpeed = 2.0f;
	public float rotateSpeed = 2.0f;
	public float jumpPower = 3.0f; 
	private CapsuleCollider col; //胶囊碰撞器
	private Rigidbody rb;
	private Vector3 velocity;
	private float orgColHight;
	private Vector3 orgVectColCenter;
	private Animator anim;							
	private AnimatorStateInfo currentBaseState;			//当前状态参照
	static int idleState = Animator.StringToHash("Base Layer.Idle");
	static int locoState = Animator.StringToHash("Base Layer.Locomotion");
	static int jumpState = Animator.StringToHash("Base Layer.Jump");
	static int restState = Animator.StringToHash("Base Layer.Rest");
	void Start ()
	{//取得动画状态机、胶囊碰撞器、刚体组件、初始时胶囊碰撞器的高度和中心位置
		anim = GetComponent<Animator>();
		col = GetComponent<CapsuleCollider>();
		rb = GetComponent<Rigidbody>();
		orgColHight = col.height;
		orgVectColCenter = col.center;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
    void FixedUpdate ()
	{
		float h = Input.GetAxis("Horizontal");				
		float v = Input.GetAxis("Vertical");				
		anim.SetFloat("Speed", v);							
		anim.SetFloat("Direction", h); 						
		anim.speed = animSpeed;								
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	//得到动画状态机Layers0的状态信息
		rb.useGravity = true;
		velocity = new Vector3(0, 0, v);	
		velocity = transform.TransformDirection(velocity);//得到Chan正前方在世界坐标下的方向
		if (v > 0.1) {
			velocity *= forwardSpeed;		
		} else if (v < -0.1) {
			velocity *= backwardSpeed;	
		}//前进和后退
		if (Input.GetButtonDown("Jump")) {	
			if (currentBaseState.fullPathHash == locoState)
            {//当前状态==walks或者Runs时，才能跳跃
				if(!anim.IsInTransition(0))//没有处于过渡状态
				{
						rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);//ForceMode.VelocityChange是给一个瞬间加速度，但是忽略自身重力
						anim.SetBool("Jump", true);		
				}
			}
		}
		transform.localPosition += velocity * Time.fixedDeltaTime;
		transform.Rotate(0, h * rotateSpeed, 0);	
		if (currentBaseState.fullPathHash == locoState){
			if(useCurves){
				resetCollider();
			}
		}
		else if(currentBaseState.fullPathHash == jumpState)
		{
			if(!anim.IsInTransition(0))
			{
				if(useCurves){
					float jumpHeight = anim.GetFloat("JumpHeight");
					float gravityControl = anim.GetFloat("GravityControl"); 
					if(gravityControl > 0)rb.useGravity = false;	
					Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
					RaycastHit hitInfo = new RaycastHit();
					if (Physics.Raycast(ray, out hitInfo))//向正下方发射射线碰撞检测
					{
						if (hitInfo.distance > useCurvesHeight)
						{
                            
							col.height = orgColHight - jumpHeight;			
							float adjCenterY = orgVectColCenter.y + jumpHeight;
							col.center = new Vector3(0, adjCenterY, 0);	
						}
						else{
							resetCollider();
						}
					}
				}		
				anim.SetBool("Jump", false);
			}
		}
		else if (currentBaseState.fullPathHash == idleState)
		{
			if(useCurves){
				resetCollider();
			}
			if (Input.GetButtonDown("Jump")) {
				anim.SetBool("Rest", true);
			}
		}
		else if (currentBaseState.fullPathHash == restState)
		{
			if(!anim.IsInTransition(0))
			{
				anim.SetBool("Rest", false);
			}
		}
	}
	void resetCollider()
	{//还原
		col.height = orgColHight;
		col.center = orgVectColCenter;
	}
}
