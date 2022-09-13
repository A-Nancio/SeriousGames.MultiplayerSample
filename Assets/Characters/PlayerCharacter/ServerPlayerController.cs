using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

/// <summary>
/// Server side script to do some movements that can only be done server side with Netcode. In charge of spawning (which happens server side in Netcode)
/// and picking up objects
/// </summary>
[DefaultExecutionOrder(0)] // before client component
public class ServerPlayerController : NetworkBehaviour
{
    //item pickup variable
    public NetworkVariable<bool> ObjPickedUp = new NetworkVariable<bool>();
    private NetworkObject m_PickedUpObj;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            if (ObjPickedUp.Value)
            {
                Debug.Log("dropping");
                DropObjServerRpc();
            }
            else
            {

                Debug.Log("picking up");
                // todo use non-alloc, don't do the below at home
                // detect nearby ingredients
                var hit = Physics.OverlapSphere(transform.position, 2, LayerMask.GetMask(new[] {"PickupItems"}), QueryTriggerInteraction.Ignore);
                if (hit.Length > 0)
                {
                    Debug.Log("picked");
                    var ingredient = hit[0].gameObject.GetComponent<Item>();
                    if (ingredient != null)
                    {
                        var netObj = ingredient.NetworkObjectId;
                        // Netcode is a server driven SDK. Shared objects like ingredients need to be interacted with using ServerRPCs. Therefore, there
                        // will be a delay between the button press and the reparenting.
                        // This delay could be hidden with some animations/sounds/VFX that would be triggered here.
                        PickupObjServerRpc(netObj);
                    }
                }
            }
        }
    }

    [ServerRpc]
    public void PickupObjServerRpc(ulong objToPickupID)
    {
        NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objToPickupID, out var objToPickup);
        if (objToPickup == null || objToPickup.transform.parent != null) return; // object already picked up, server authority says no

        objToPickup.GetComponent<Rigidbody>().isKinematic = true;
        objToPickup.transform.parent = transform;
        objToPickup.GetComponent<NetworkTransform>().InLocalSpace = true;
        objToPickup.transform.localPosition = new Vector3(0, 1, 1);
        ObjPickedUp.Value = true;
        m_PickedUpObj = objToPickup;
    }

    [ServerRpc]
    public void DropObjServerRpc()
    {
        if (m_PickedUpObj != null)
        {
            // can be null if enter drop zone while carying
            m_PickedUpObj.transform.parent = null;
            m_PickedUpObj.GetComponent<Rigidbody>().isKinematic = false;
            m_PickedUpObj.GetComponent<NetworkTransform>().InLocalSpace = false;
            m_PickedUpObj = null;
        }

        ObjPickedUp.Value = false;
    }
}
