using UnityEngine;

public class UnityChanController : MonoBehaviour
{
    public float animSpeed = 1.0f;
    public float forwardSpeed = 7.0f;
    public float backwardSpeed = 2.0f;
    public float rotateSpeed = 2.0f;
    public float jumpPower = 3.0f;
    public bool useCurves = true;               // Mecanimでカーブ調整を使うか設定する
                                                // このスイッチが入っていないとカーブは使われない
    public float useCurvesHeight = 0.5f;        // カーブ補正の有効高さ（地面をすり抜けやすい時には大きくする）

    private CapsuleCollider col;
    private Rigidbody rb;
    private Animator anim;
    private AnimatorStateInfo currentBaseState;
    private Vector3 velocity;
    // CapsuleColliderで設定されているコライダのHeight、Centerの初期値を収める変数
    private float orgColHeight;
    private Vector3 orgVectColCenter;

    // Reference to different animator states
    private static int idleState = Animator.StringToHash("Base Layer.Idle");
    private static int locoState = Animator.StringToHash("Base Layer.Locomotion");
    private static int jumpState = Animator.StringToHash("Base Layer.Jump");
    private static int restState = Animator.StringToHash("Base Layer.Rest");
    private static int idleJumpState = Animator.StringToHash("Base Layer.IdleJump");

    // Start is called before the first frame update
    private void Start()
    {
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        // CapsuleCollider components
        orgColHeight = col.height;
        orgVectColCenter = col.center;
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        anim.SetFloat("Direction", horizontal);
        anim.SetFloat("Speed", vertical);
        anim.speed = animSpeed;
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0);

        // Must set gravity to false when jumping, otherwise always true
        rb.useGravity = true;

        // direction change with up and down arrow keys
        velocity = new Vector3(0.0f, 0.0f, vertical);
        // Change movement direction of character 
        velocity = transform.TransformDirection(velocity);
        if (vertical >= 0.1)
        {
            velocity *= forwardSpeed;
        } else if (vertical <= -0.1)
        {
            velocity *= backwardSpeed;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (currentBaseState.nameHash == idleState)
            {
                rb.AddForce(Vector3.up * jumpPower);
                //anim.SetBool("IdleJump", true);
                anim.Play("IdleJump", -1, 0f);
            }

            //アニメーションのステートがLocomotionの最中のみジャンプできる
            if (currentBaseState.nameHash == locoState)
            {
                //ステート遷移中でなかったらジャンプできる
                if (!anim.IsInTransition(0))
                {
                    rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
                    anim.SetBool("Jump", true);     // Animatorにジャンプに切り替えるフラグを送る
                }
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            anim.SetBool("Rest", true);
        }

        // To move character with up and down arrow keys
        transform.localPosition += velocity * Time.fixedDeltaTime;

        // To rotate direction of character with left and right arrow keys
        transform.Rotate(0, horizontal * rotateSpeed, 0);

        // Reset collider just in case
        if (currentBaseState.nameHash == locoState || currentBaseState.nameHash == idleState)
        {
            if (useCurves)
            {
                resetCollider();
            }        
        }
        else if (currentBaseState.nameHash == idleJumpState)
        {
            if (!anim.IsInTransition(0))
            {
                anim.SetBool("IdleJump", false);
            }
        }
        else if (currentBaseState.nameHash == jumpState)
        {
            if (!anim.IsInTransition(0))
            {
                if (useCurves)
                {
                    // 以下JUMP00アニメーションについているカーブJumpHeightとGravityControl
                    // JumpHeight:JUMP00でのジャンプの高さ（0〜1）
                    // GravityControl:1⇒ジャンプ中（重力無効）、0⇒重力有効
                    float jumpHeight = anim.GetFloat("JumpHeight");
                    float gravityControl = anim.GetFloat("GravityControl");
                    if (gravityControl > 0)
                        rb.useGravity = false;  //ジャンプ中の重力の影響を切る

                    // レイキャストをキャラクターのセンターから落とす
                    Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
                    RaycastHit hitInfo = new RaycastHit();
                    // 高さが useCurvesHeight 以上ある時のみ、コライダーの高さと中心をJUMP00アニメーションについているカーブで調整する
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        if (hitInfo.distance > useCurvesHeight)
                        {
                            col.height = orgColHeight - jumpHeight;          // 調整されたコライダーの高さ
                            float adjCenterY = orgVectColCenter.y + jumpHeight;
                            col.center = new Vector3(0, adjCenterY, 0); // 調整されたコライダーのセンター
                        }
                        else
                        {
                            // 閾値よりも低い時には初期値に戻す（念のため）					
                            resetCollider();
                        }
                    }
                }
                // Jump bool値をリセットする（ループしないようにする）				
                anim.SetBool("Jump", false);
            }
        }
        else if (currentBaseState.nameHash == restState)
        {
            if (!anim.IsInTransition(0))
            {
                anim.SetBool("Rest", false);
            }
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(Screen.width - 260, 10, 250, 190), "Interaction");
        GUI.Label(new Rect(Screen.width - 245, 30, 250, 30), "Up/Down Arrow : Go Forwald/Go Back");
        GUI.Label(new Rect(Screen.width - 245, 50, 250, 30), "Left/Right Arrow : Turn Left/Turn Right");
        GUI.Label(new Rect(Screen.width - 245, 70, 250, 30), "Space : Jump");
        GUI.Label(new Rect(Screen.width - 245, 90, 250, 30), "Left Ctrl : Front Camera");
        GUI.Label(new Rect(Screen.width - 245, 110, 250, 30), "Alt : Free view Camera");
        GUI.Label(new Rect(Screen.width - 245, 130, 250, 30), "Shift + Mouse wheel : Zoom");
        GUI.Label(new Rect(Screen.width - 245, 150, 250, 30), "Shift + Left click : Drag");
        GUI.Label(new Rect(Screen.width - 245, 170, 250, 30), "Shift + Right click : Free");
    }

    void resetCollider()
    {
        // コンポーネントのHeight、Centerの初期値を戻す
        col.height = orgColHeight;
        col.center = orgVectColCenter;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
