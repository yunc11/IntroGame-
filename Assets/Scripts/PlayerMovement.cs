
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        if (Keyboard.current == null)
            return;

        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            input.y += 1f;

        if (Keyboard.current.sKey.isPressed)
            input.y -= 1f;

        if (Keyboard.current.aKey.isPressed)
            input.x -= 1f;

        if (Keyboard.current.dKey.isPressed)
            input.x += 1f;

        // Move relative to the direction the player is facing.
        Vector3 direction =
            transform.right * input.x +
            transform.forward * input.y;

        // Prevent diagonal movement from being faster.
        direction.y = 0f;
        direction.Normalize();

        transform.position +=
            direction * moveSpeed * Time.deltaTime;
    }
}
