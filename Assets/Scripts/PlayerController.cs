using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public float thrust;
    public float jumpThrust;
    public Text countText;
    public Text winText;
    public Text countDownText;
    public float countDown;
    public GameObject panel;

    private Rigidbody rb;
    private int count;
    private bool stopCountdown;

    // Start is called before the first frame update
    public void Start() {
        
        rb = GetComponent<Rigidbody>();
        count = 0;
        stopCountdown = false;
        SetCountText();
        SetCountDownText();
        winText.text = "";
        panel.SetActive(false);
    }

    // Update() is called once per frame
    // Called before rendering a frame
    public void Update() {
        SetCountDownText();
        if (countDown <= 0) {
            endGame();
        }
        if (!stopCountdown) {
            countDown -= Time.deltaTime;
        }
    }

    // Called before performing any physics calculation
    public void FixedUpdate() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(horizontal, 0.0f, vertical);

        // move
        rb.AddForce(move * thrust);

        // jump
        if (Input.GetKeyDown(KeyCode.Space)) {
            rb.AddForce(Vector3.up * jumpThrust);
        }
    }

    // Destroy everything that enters the trigger
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Pick Up")) {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();
        }
    }

    private void SetCountText() {
        countText.text = "Count: " + count.ToString();
        if (count < 20 && countDown == 0) {
            stopCountdown = true;
            winText.text = "Game over! Please try again.";
        } else if (count == 20 && countDown >= 0) {
            stopCountdown = true;
            winText.color = Color.green;
            winText.text = "You win!";
        }
    }

    private void SetCountDownText() {
        countDownText.text = "Timer: " + (countDown).ToString("0");
    }

    private void endGame() {
        panel.SetActive(true);
        winText.color = Color.red;
        winText.text = "Game over! Please try again.";
        // disable Update() call; change this with enabled = true;
        // disable Update() call; change this with enabled = true;
        enabled = false;
    }

    private void move() {

    }

    private void jump() {

    }
}



