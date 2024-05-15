using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;


public class FPSKeybinds : MonoBehaviour
{
    #region looking variables
    [SerializeField]
    Transform cam;

    [SerializeField]
    float mouseSens;

    bool canMove = true;

    float xRotation = 0f;
    #endregion

    #region walking variables
    [SerializeField]
    float walkSpeed;

    [SerializeField]
    Rigidbody rb;
    #endregion

    private void Start()
    {
        #region mouse sens en cursor state
        Cursor.lockState = CursorLockMode.Locked;

        mouseSens = 550f;
        #endregion

        #region default walkspeed
        walkSpeed = 7f;
        #endregion
    }

    private void UpdateScript()
    {
        if (canMove)
        {
            #region looking x and y axis
            float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            cam.localRotation = Quaternion.Euler(xRotation, 0, 0);
            transform.Rotate(Vector3.up * mouseX);
            #endregion

            #region execute walk void
            #endregion
        }
    }

    #region void walk for adding velocity when pressing wasd and onmove for getting the wasd input

    public void EnableInput(bool enabled)
    {
        canMove = enabled;
        if (!canMove)
        {
            rb.velocity = Vector3.zero;
        }
    }
    void OnMove(InputValue value)
    {
        Walk(value.Get<Vector2>());
    }
    void Walk(Vector2 input)
    {
        Vector3 playerV = new Vector3(input.x * walkSpeed, rb.velocity.y, input.y * walkSpeed);
        rb.velocity = transform.TransformDirection(playerV);
    }
    #endregion
}