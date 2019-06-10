using UnityEngine;

public class CameraController : MonoBehaviour
{
    // To stop camera from falling player without rotating

    public GameObject player;
    public float smooth = 3f;
    public float dragSpeed = 0.5f;

    private Transform frontPos;
    private Transform backPos;
    private Vector3 offset;
    private Vector3 offsetX;
    private Vector3 standardVectForward;
    private Vector3 oldMousePos;
    private Vector3 dragOrigin;

    // Start is called before the first frame update
    private void Start()
    {
        offset = transform.position - player.transform.position;
        standardVectForward = transform.forward;

        if (GameObject.Find("FrontPos"))
        {
            frontPos = GameObject.Find("FrontPos").transform;
        }

        if (GameObject.Find("JumpPos"))
        {
            backPos = GameObject.Find("JumpPos").transform;
        }
    }

    private void Update()
    {
        float delta = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            // zoom
            if (delta != 0.0f)
            {
                MouseWheelEvent(delta);
            }
            // left click, drag camera
            if (Input.GetMouseButton(0))
            {
                DragCamera();
            } 
            // right click, free camera
            if (Input.GetMouseButton(1))
            {
                FreeCamera();
            }
        }
        // front view
        else if (Input.GetKey(KeyCode.LeftControl)) 
        {
            print("frontview");
            DoFrontView();
        }
        // back view
        else if (Input.GetKey(KeyCode.LeftAlt)) 
        {
            print("backview");
            DoBackView();
        }
        // normal view
        else
        {
            print("normalview");
            DoNormalView();
        }
    }

    private void MouseWheelEvent(float delta)
    {
        Vector3 focusToPosition = transform.position;

        Vector3 post = focusToPosition * (1.0f + delta);

        if (post.magnitude > 0.01)
            transform.position = post;
    }

    private void DragCamera()
    {
        Camera.main.transform.position -= new Vector3(Input.GetAxis("Mouse X") * dragSpeed, 0, Input.GetAxis("Mouse Y") * dragSpeed);
    }

    //public float turnSpeed = 4.0f;
    //public Transform player;

    //public float height = 1f;
    //public float distance = 2f;

    //private Vector3 offsetX;

    //void Start()
    //{

    //    offsetX = new Vector3(0, height, distance);
    //}

    //void LateUpdate()
    //{
    //    offsetX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offsetX;
    //    transform.position = player.position + offsetX;
    //    transform.LookAt(player.position);
    //}

    private void FreeCameraTest()
    {
        print("offset before DoNormalView: " + offset);
        DoNormalView();

        print("offset after DoNormalView: " + offset);
        Vector3 offsetX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * dragSpeed, Vector3.up) * offset;
        print("offsetX: " + offsetX);
        transform.position = player.transform.position + offsetX;
        transform.LookAt(player.transform.position);
    }

    //offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
    //      transform.position = player.position + offset; 
    //      transform.LookAt(player.position);


    private void FreeCamera()
    {
        Vector3 tempOffset = new Vector3(player.transform.position.x, player.transform.position.y + 8.0f, player.transform.position.z + 7.0f);
        Vector3 freeOffset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * dragSpeed, Vector3.up) * tempOffset;
        transform.position = player.transform.position + freeOffset;
        transform.LookAt(player.transform.position);
    }

    private void DoFrontView()
    {
        transform.position = frontPos.position;
        transform.forward = frontPos.forward;
    }

    private void DoBackView()
    {
        transform.position = backPos.position;
        transform.forward = backPos.forward;
    }

    private void DoNormalView()
    {
        transform.position = player.transform.position + offset;
        transform.forward = standardVectForward;
    }
}
