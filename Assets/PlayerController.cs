using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;        // Speed of the player movement
    public float jumpForce = 5f;        // Force applied when the player jumps
    public LayerMask groundLayer;       // Layer mask to specify what is ground
    public GameObject arm;

    private Rigidbody rb;
    public bool isGrounded;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Jump();
        RotateTowardsMouse();
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if(transform.position.z >= 2 || transform.position.z <= 2){
            Vector3 movement = new Vector3(moveVertical, 0.0f, -moveHorizontal) * moveSpeed * Time.deltaTime;
            transform.Translate(movement, Space.Self);
        }
    }

    void Jump()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.2f, groundLayer);

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void RotateTowardsMouse()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast and if it hits something, adjust the player's Z rotation
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.point;
            Vector3 direction = (targetPosition - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Only change the Z rotation of the player
            arm.transform.rotation = Quaternion.Euler(0, 0, angle);

        }
        else
        {

        }
    }
}
