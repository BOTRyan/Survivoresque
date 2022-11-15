using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public static Vector3 velocity = Vector3.zero;
    public static Vector2 moveInput;
    public static Vector2 lookInput;

    // Components
    public static PlayerControls playerControls;
    public static PlayerInput playerInput;
    public static Rigidbody2D rb2D;
    public static Camera mainCam;

    // Utilites
    public static bool isGamepad;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerControls = new PlayerControls();
        playerControls.Enable();
        playerInput = GetComponent<PlayerInput>();
        mainCam = Camera.main;
    }

    void Update()
    {
        // Movement
        moveInput = playerControls.PlayerMap.Move.ReadValue<Vector2>();

        // Rotation
        lookInput = playerControls.PlayerMap.Look.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // Movement
        Vector2 characterMovement = new Vector2(moveInput.x, moveInput.y);
        characterMovement = characterMovement.normalized;
        Move(characterMovement * Time.fixedDeltaTime * baseSpeed);

        if (isGamepad)
        {
            // Rotation Gamepad
            if (lookInput.x != 0 || lookInput.y != 0)
            {
                float aimAngle2 = (Mathf.Atan2(lookInput.y, lookInput.x) * Mathf.Rad2Deg);
                transform.rotation = Quaternion.AngleAxis(aimAngle2, Vector3.forward);
            }
            else
            {
                float faceAngle = (Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, faceAngle));
            }
        }
        else
        {
            // Rotation Mouse
            Vector3 mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3 rotation = mousePos - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, rotZ);
        }
    }

    public void Move(Vector2 move)
    {
        Vector3 targetVelocity = new Vector2(move.x * 10f, move.y * 10f);

        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, movementSmoothing);
    }

    public void OnDeviceChange(PlayerInput pi)
    {
        isGamepad = pi.currentControlScheme.Equals("Gamepad") ? true : false;
    }
}
