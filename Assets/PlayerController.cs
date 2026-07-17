using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("References")]
    public Rigidbody rb;
    public Transform head;
    public Camera camera;

    [Header("Configurations")]
    public float walkSpeed;
    public float runSpeed;
    public float jumpSpeed;

    [Header("Runtime")]
    Vector3 newVelocity;
    bool isGrounded = false;
    bool isJumping = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        // Horizontal rotation
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * 2f);

        newVelocity = Vector3.up * rb.linearVelocity.y;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        newVelocity.x = Input.GetAxis("Horizontal") * speed;
        newVelocity.z = Input.GetAxis("Vertical") * speed;

        if(isGrounded) {
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping) {
                newVelocity.y = jumpSpeed;
                isJumping = true;
            }
        }

        rb.linearVelocity = transform.TransformDirection(newVelocity);
    }

    void FixedUpdate() {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f)) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }
    }

    void LateUpdate() {
        // Vertical rotation
        Vector3 e = head.eulerAngles;
        e.x -= Input.GetAxis("Mouse Y") * 2f;
        e.x = RestrictAngle(e.x, -60f, 60f);
        head.eulerAngles = e;
    }

    //clamp verticle head rotation
    public static float RestrictAngle(float angle, float angleMin, float angleMax) {
        if (angle > 180)
            angle -= 360;
        else if (angle < -180)
            angle += 360;

        if (angle > angleMax)
            angle = angleMax;
        else if (angle < angleMin)
            angle = angleMin;

        return angle;
    }

    void OnCollisionStay(Collision col) {
        isGrounded = true;
        isJumping = false;
    }

    void OnCollisionExit(Collision col) {
        isGrounded = false;
    }
}
