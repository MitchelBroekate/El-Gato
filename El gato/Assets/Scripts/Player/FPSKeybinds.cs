using UnityEngine;
using UnityEngine.InputSystem;


public class FPSKeybinds : MonoBehaviour
{
    #region looking variables
    [SerializeField]
    Transform cam;

    [SerializeField]
    float mouseSens;

    float xRotation = 0f;
    #endregion

    #region walking variables
    [SerializeField]
    float walkSpeed;

    Vector2 moveInput;

    Rigidbody rb;
    #endregion

    private void Start()
    {
        #region mouse sens en cursor state
        Cursor.lockState = CursorLockMode.Locked;

        mouseSens = 550f;
        #endregion

        #region rigidbody and walkspeed
        rb = GetComponent<Rigidbody>();
        walkSpeed = 7f;
        #endregion
    }

    private void Update()
    {
        #region looking x and y axis
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.localRotation = Quaternion.Euler(xRotation,0 ,0);
        transform.Rotate(Vector3.up * mouseX);
        #endregion

        #region execute walk void
        Walk();
        #endregion
    }

    #region void walk for adding velocity when pressing wasd and onmove for getting the wasd input
    void Walk()
    {
        Vector3 playerV = new Vector3(moveInput.x * walkSpeed, rb.velocity.y, moveInput.y * walkSpeed);
        rb.velocity = transform.TransformDirection(playerV);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    #endregion
}