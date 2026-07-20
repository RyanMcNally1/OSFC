using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("References")]
    public Rigidbody rb;
    public Transform head;
    public Transform normalCameraPoint;
    public Transform aimCameraPoint;
    public PlayerEquipment playerEquipment;

    [Header("Configurations")]
    public float walkSpeed;
    public float runSpeed;
    public float aimWalkSpeed;
    public float jumpSpeed;

    [Header("Runtime")]
    Vector3 newVelocity;
    bool isGrounded = false;
    bool isJumping = false;

    [Header("Aiming")]
    public Camera playerCamera;
    public float normalFOV = 60f;
    public float aimFOV = 40f;
    public float aimSpeed = 10f;

    public float normalSensitivity = 2f;
    public float aimSensitivity = 1f;

    private bool isAiming;
    private float currentSensitivity;

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        currentSensitivity = normalSensitivity;
    }

    void Update() {
        // Check whether the player is aiming
        isAiming =
            playerEquipment != null &&
            playerEquipment.IsRifleEquipped() &&
            Input.GetMouseButton(1);

        currentSensitivity = isAiming
            ? aimSensitivity
            : normalSensitivity;

        // Horizontal rotation
        float mouseX = Input.GetAxis("Mouse X");

        transform.Rotate(
            Vector3.up * mouseX * currentSensitivity
        );

        // Movement
        newVelocity = Vector3.up * rb.linearVelocity.y;

        float speed;

        if (isAiming) {
            speed = aimWalkSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftShift)) {
            speed = runSpeed;
        }
        else {
            speed = walkSpeed;
        }

        newVelocity.x = Input.GetAxis("Horizontal") * speed;
        newVelocity.z = Input.GetAxis("Vertical") * speed;

        if (isGrounded) {
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                newVelocity.y = jumpSpeed;
                isJumping = true;
            }
        }

        rb.linearVelocity = transform.TransformDirection(newVelocity);

        // Smooth FOV change while aiming
        float targetFOV = isAiming
            ? aimFOV
            : normalFOV;

        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            targetFOV,
            aimSpeed * Time.deltaTime
        );
    }

    void FixedUpdate() {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f)) {
            isGrounded = true;
        }
        else {
            isGrounded = false;
        }
    }

    void LateUpdate() {
        // Vertical rotation
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 e = head.eulerAngles;
        e.x -= mouseY * currentSensitivity;
        e.x = RestrictAngle(e.x, -60f, 60f);
        head.eulerAngles = e;

        // Move camera
        Transform target = isAiming
            ? aimCameraPoint
            : normalCameraPoint;

        playerCamera.transform.localPosition = Vector3.Lerp(
            playerCamera.transform.localPosition,
            target.localPosition,
            aimSpeed * Time.deltaTime
        );

        playerCamera.transform.localRotation = Quaternion.Lerp(
            playerCamera.transform.localRotation,
            target.localRotation,
            aimSpeed * Time.deltaTime
        );
    }

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
