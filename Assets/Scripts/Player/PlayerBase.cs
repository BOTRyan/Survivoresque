using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    // Stats
    public static float baseSpeed = 50;
    public static float baseAttack = 1;
    public static float baseAttackInterval = 1;
    public static float baseCritChance = .1f;
    public static float baseHealth = 10;
    public static float baseUltCharge = 10;

    // Movement/Rotation
    public static float movementSmoothing = .05f;
    private Vector3 velocity = Vector3.zero;
    public static Vector2 moveInput;
    public static float faceAngle = 0;
    public static Vector2 lookInput;

    // Components
    public static PlayerInputs playerInputs;
    private Rigidbody2D rb2D;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerInputs = new PlayerInputs();
        playerInputs.PlayerMap.Enable();
    }

    void Update()
    {
        // Movement
        moveInput = playerInputs.PlayerMap.Move.ReadValue<Vector2>();

        // Rotation
        lookInput = playerInputs.PlayerMap.Look.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // Movement
        Vector2 characterMovement = new Vector2(moveInput.x, moveInput.y * -1);
        characterMovement = characterMovement.normalized;
        Move(characterMovement * Time.fixedDeltaTime * baseSpeed);

        // Rotation
        float aimAngle = (Mathf.Atan2(lookInput.x, lookInput.y) * Mathf.Rad2Deg) - 90;
        if (lookInput.x != 0 || lookInput.y != 0)
        {
            transform.rotation = Quaternion.AngleAxis(aimAngle, Vector3.forward);
        }
        else
        {
            faceAngle = (Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg) - 90;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, faceAngle));
        }
    }

    public void Move(Vector2 move)
    {
        Vector3 targetVelocity = new Vector2(move.x * 10f, move.y * 10f);

        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, movementSmoothing);
    }
}
