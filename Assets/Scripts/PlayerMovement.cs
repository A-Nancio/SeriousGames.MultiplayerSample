using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(ClientNetworkTransform))]
public class PlayerMovement : NetworkBehaviour
{
    // responsible for moving the character
    public float speed;

    //responsible for moving the camera
    public float mouseSensitivity = 100f;

    [SerializeField]
    private Camera m_Camera;

    private CharacterController m_CharacterController;
    private float xRotation = 0f;

    public void Start() {
        //Cursor.lockState = CursorLockMode.Locked;
    }
    //place the character
    public override void OnNetworkSpawn()
    {   
        base.OnNetworkSpawn();
        enabled = IsClient;
        if (!IsOwner)
        {
            m_Camera.gameObject.SetActive(false);
            enabled = false;
            return;
        }

        m_CharacterController = GetComponent<CharacterController>();
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
        m_CharacterController.SimpleMove(direction * speed);
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