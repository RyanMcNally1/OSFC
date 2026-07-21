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
    private Vector3 newVelocity;
    private bool isGrounded = false;
    private bool isJumping = false;

    [Header("Aiming")]
    public Camera playerCamera;
    public float normalFOV = 60f;
    public float aimFOV = 40f;
    public float aimSpeed = 10f;
    public float normalSensitivity = 2f;
    public float aimSensitivity = 1f;

    private bool isAiming;
    private float currentSensitivity;

    [Header("Enemy Collision")]
    [Range(0f, 1f)]
    public float enemyBlockedSpeedMultiplier = 0.35f;

    private bool touchingEnemy = false;
    private Vector3 enemyBlockDirection;

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        currentSensitivity = normalSensitivity;

        if (rb == null) {
            rb = GetComponent<Rigidbody>();
        }
    }

    void Update() {
        HandleAiming();
        HandleHorizontalRotation();
        HandleMovement();
        HandleCameraFOV();
    }

    void FixedUpdate() {
        CheckGrounded();
    }

    void LateUpdate() {
        HandleVerticalRotation();
        HandleCameraPosition();
    }

    void HandleAiming() {
        isAiming =
            playerEquipment != null &&
            playerEquipment.IsRifleEquipped() &&
            Input.GetMouseButton(1);

        currentSensitivity = isAiming
            ? aimSensitivity
            : normalSensitivity;
    }

    void HandleHorizontalRotation() {
        float mouseX = Input.GetAxis("Mouse X");

        transform.Rotate(
            Vector3.up * mouseX * currentSensitivity
        );
    }

void HandleMovement() {
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

    Vector3 localInput = new Vector3(
        Input.GetAxis("Horizontal"),
        0f,
        Input.GetAxis("Vertical")
    );

    localInput = Vector3.ClampMagnitude(
        localInput,
        1f
    );

    Vector3 worldInput =
        transform.TransformDirection(localInput);

    worldInput.y = 0f;

    if (worldInput.sqrMagnitude > 0.01f) {
        worldInput.Normalize();
    }

    if (
        touchingEnemy &&
        worldInput.sqrMagnitude > 0.01f
    ) {
        float pushingTowardEnemy =
            Vector3.Dot(
                worldInput,
                enemyBlockDirection
            );

        if (pushingTowardEnemy > 0.05f) {
            float blockStrength =
                Mathf.Clamp01(
                    pushingTowardEnemy
                );

            float movementMultiplier =
                Mathf.Lerp(
                    1f,
                    enemyBlockedSpeedMultiplier,
                    blockStrength
                );

            speed *= movementMultiplier;
        }
    }

    newVelocity.x =
        localInput.x * speed;

    newVelocity.z =
        localInput.z * speed;

    if (
        isGrounded &&
        Input.GetKeyDown(KeyCode.Space) &&
        !isJumping
    ) {
        newVelocity.y = jumpSpeed;
        isJumping = true;
    }

    rb.linearVelocity =
        transform.TransformDirection(
            newVelocity
        );
}

    void CheckGrounded() {
        float rayDistance = 1.1f;

        if (
            Physics.Raycast(
                transform.position,
                Vector3.down,
                rayDistance
            )
        ) {
            isGrounded = true;
            isJumping = false;
        }
        else {
            isGrounded = false;
        }
    }

    void HandleCameraFOV() {
        float targetFOV = isAiming
            ? aimFOV
            : normalFOV;

        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            targetFOV,
            aimSpeed * Time.deltaTime
        );
    }

    void HandleVerticalRotation() {
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 e = head.eulerAngles;

        e.x -= mouseY * currentSensitivity;
        e.x = RestrictAngle(e.x, -60f, 60f);

        head.eulerAngles = e;
    }

    void HandleCameraPosition() {
        Transform target = isAiming
            ? aimCameraPoint
            : normalCameraPoint;

        if (target == null || playerCamera == null) {
            return;
        }

        playerCamera.transform.localPosition =
            Vector3.Lerp(
                playerCamera.transform.localPosition,
                target.localPosition,
                aimSpeed * Time.deltaTime
            );

        playerCamera.transform.localRotation =
            Quaternion.Lerp(
                playerCamera.transform.localRotation,
                target.localRotation,
                aimSpeed * Time.deltaTime
            );
    }

    void OnCollisionEnter(Collision collision) {
        if (!collision.gameObject.CompareTag("Enemy")) {
            return;
        }

        touchingEnemy = true;
        UpdateEnemyBlockDirection(collision);
    }

    void OnCollisionStay(Collision collision) {
        if (!collision.gameObject.CompareTag("Enemy")) {
            return;
        }

        touchingEnemy = true;
        UpdateEnemyBlockDirection(collision);
    }

    void OnCollisionExit(Collision collision) {
        if (!collision.gameObject.CompareTag("Enemy")) {
            return;
        }

        touchingEnemy = false;
        enemyBlockDirection = Vector3.zero;
    }

    void UpdateEnemyBlockDirection(Collision collision) {
        Vector3 combinedNormal = Vector3.zero;

        foreach (ContactPoint contact in collision.contacts) {
            combinedNormal += contact.normal;
        }

        if (combinedNormal.sqrMagnitude < 0.01f) {
            return;
        }

        combinedNormal.Normalize();
        combinedNormal.y = 0f;

        enemyBlockDirection = -combinedNormal.normalized;
    }

    public static float RestrictAngle(
        float angle,
        float angleMin,
        float angleMax
    ) {
        if (angle > 180f) {
            angle -= 360f;
        }
        else if (angle < -180f) {
            angle += 360f;
        }

        if (angle > angleMax) {
            angle = angleMax;
        }
        else if (angle < angleMin) {
            angle = angleMin;
        }

        return angle;
    }
}
