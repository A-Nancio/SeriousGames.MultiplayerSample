using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(ClientNetworkTransform))]
public class PlayerMovement : NetworkBehaviour
{
    public enum PlayerState 
    {
        Idle,
        Walk,
        ReverseWalk
    }
    [SerializeField]
    private NetworkVariable<PlayerState> networkPlayerState = new NetworkVariable<PlayerState>();
    private PlayerState oldPlayerState = PlayerState.Idle;
    private Animator animator;

    // responsible for moving the character
    public float speed;

    //responsible for moving the camera
    public float mouseSensitivity = 100f;

    [SerializeField]
    private Camera m_Camera;

    private CharacterController m_CharacterController;
    private float xRotation = 0f;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
    }
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
        ClientVisuals();
    }

    private void KeyboardInput()
    {

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = transform.right * horizontal + transform.forward * vertical;
        if (vertical == 0)  
            UpdatePlayerStateServerRpc(PlayerState.Idle);
        else if (vertical > 0 && vertical <= 1)
            UpdatePlayerStateServerRpc(PlayerState.Walk);
        else if (vertical < 0)
            UpdatePlayerStateServerRpc(PlayerState.ReverseWalk);

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

    private void ClientVisuals()
    {
        if (oldPlayerState != networkPlayerState.Value)
        {
            oldPlayerState = networkPlayerState.Value;
            
            if (networkPlayerState.Value == PlayerState.Walk)
            {   
                animator.SetFloat("Walk", 1);
            }
            if (networkPlayerState.Value == PlayerState.Idle)
            {
                animator.SetFloat("Walk", 0);
            }
            if (networkPlayerState.Value == PlayerState.ReverseWalk)
            {
                animator.SetFloat("Walk", -1);
            }
        }
    }

    [ServerRpc]
    public void UpdatePlayerStateServerRpc(PlayerState state)
    {
        networkPlayerState.Value = state;
    }


}