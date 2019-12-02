using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class Player : NetworkBehaviour
{

    public GameObject bombPrefab;

    public Camera attachedCamera;
    public Transform attachedVirtualCamera;

    public float speed = 10f, jump = 10f;
    public LayerMask ignoreLayers;
    public float rayDistance = 10f;
    public bool isGrounded = false;
    private Rigidbody rigid;

    #region Unity Events
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayDistance);
    }
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();

        // Detaches cameras from objects
        attachedCamera.transform.SetParent(null);
        attachedVirtualCamera.SetParent(null);

        if (isLocalPlayer) // Detects if the player object is the one being controlled by the client, and enables camera if true
        {
            attachedCamera.enabled = true;
            attachedCamera.rect = new Rect(0f, 0f, 0.5f, 1.0f);
        }
        else
        {
            attachedCamera.enabled = false;
        }

    }
    private void FixedUpdate()
    {
        Ray groundRay = new Ray(transform.position, Vector3.down);
        isGrounded = Physics.Raycast(groundRay, rayDistance, ~ignoreLayers);
    }
    private void OnTriggerEnter(Collider col) // When entering a trigger, check if trigger is an item, if so collect the item
    {
        Item item = col.GetComponent<Item>();
        if (item)
        {
            item.Collect();
        }
    }
    private void Update()
    {
        if(isLocalPlayer) // Checks if this player belongs to the client, if so, allow input
        {
            // Obtains movement inputs and moves player accordingly
            float inputH = Input.GetAxis("Horizontal");
            float inputV = Input.GetAxis("Vertical");
            Move(inputH, inputV);

            // Runs jump function if jump button is pressed
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            // Runs bomb spawning function if button is pressed
            if (Input.GetButtonDown("Fire3"))
            {
                Cmd_SpawnBomb(transform.position);
            }
        }
    }
    #endregion

    #region Commands
    [Command]
    public void Cmd_SpawnBomb(Vector3 position) // Instantiates a bomb in both the client and server
    {
        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        NetworkServer.Spawn(bomb);
    }
    #endregion

    #region Custom
    private void Jump()
    {
        if (isGrounded) // Checks if player is grounded, if so moves player upwards
        {
            rigid.AddForce(Vector3.up * jump, ForceMode.Impulse);
        }
    }
    private void Move(float inputH, float inputV)
    {
        Vector3 direction = new Vector3(inputH, 0, inputV);

        // [Optional] Move with camera
        Vector3 euler = Camera.main.transform.eulerAngles;
        direction = Quaternion.Euler(0, euler.y, 0) * direction; // Convert direction to relative direction to camera only on Y

        rigid.MovePosition(transform.position + (direction * speed * Time.fixedDeltaTime)); // Updates player's position with direction values, multiplied by the player's speed, multiplied by Time.fixedDeltaTime so the movement is consistent.
    }
    #endregion
}

