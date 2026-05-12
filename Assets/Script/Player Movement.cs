using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        if (!IsOwner) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (moveX == 0 && moveZ == 0) return; // skip if no input

        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;

        // Apply locally for responsiveness
        transform.Translate(move);

        // Tell the server to update position for all other clients
        MoveServerRpc(transform.position);
    }

    [ServerRpc]
    void MoveServerRpc(Vector3 newPosition)
    {
        // Server updates position, NetworkTransform or ClientRpc broadcasts it
        transform.position = newPosition;
    }
}