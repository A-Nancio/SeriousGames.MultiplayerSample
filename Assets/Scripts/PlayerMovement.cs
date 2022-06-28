using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(ClientNetworkTransform))]
public class PlayerMovement : NetworkBehaviour
{
    // responsible for moving the character
    public CharacterController controller;
    public float speed;

    //responsible for moving the camera
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float xRotation = 0f;

    //place the character
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            //transform.position = new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
            Camera.main.transform.SetParent(playerBody, false);
            Camera.main.transform.position = transform.position + new Vector3(0f, 1.7f, 0f);
            Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
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
        controller.SimpleMove(direction * speed);
    }
    
    private void MouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

}