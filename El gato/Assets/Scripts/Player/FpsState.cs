using UnityEngine;
using UnityEngine.InputSystem;

public class FpsState : PlayerState
{
    #region Variables
    [SerializeField]
    float mouseSens;

    float xRotation = 0f;

    [SerializeField]
    float walkSpeed;
    Vector2 walkDir;

    [SerializeField]
    Rigidbody rb;
    #endregion

    public override void DoUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);

        Walk();
    }

    public override void EnableState()
    {
        base.EnableState();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void DisableState()
    {
        base.DisableState();

        rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// This method sets the velocity for any walking direction.
    /// </summary>
    void Walk()
    {
        Vector3 playerV = new Vector3(walkDir.x * walkSpeed, rb.velocity.y, walkDir.y * walkSpeed);
        rb.velocity = transform.TransformDirection(playerV);
    }

    /// <summary>
    /// This function sets the movement vector2 in a vector2 variable.
    /// </summary>
    /// <param name="value"></param>
    void OnMove(InputValue value)
    {
        walkDir = value.Get<Vector2>();
    }
}