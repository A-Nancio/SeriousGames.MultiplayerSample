using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(ClientNetworkTransform))]
public class PlayerController : NetworkBehaviour
{   
    [SerializeField]
    private Animator animator;

    //Movement variables
    private CharacterController m_CharacterController;
    public float speed;

    //Camera variables
    [SerializeField]
    private Camera m_Camera;
    private float xRotation = 0f;
    public float mouseSensitivity = 100f;

    public override void OnNetworkSpawn()
    {   
        base.OnNetworkSpawn();
        if (!IsOwner)
        {
            m_Camera.gameObject.SetActive(false);
            return;
        }
        m_CharacterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    } 

    // Update is called once per frame
    void Update()
    {
        if (IsClient && IsOwner)
        {
            KeyboardInput();
            MouseInput();
        }
    }

    private void KeyboardInput() 
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = transform.right * horizontal + transform.forward * vertical;

        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        m_CharacterController.SimpleMove(direction.normalized * speed);
    }

    private void MouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        m_Camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
