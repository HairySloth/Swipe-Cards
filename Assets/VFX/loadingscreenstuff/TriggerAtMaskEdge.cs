using UnityEngine;

public class TriggerAtMaskEdge : MonoBehaviour
{
    public Transform spriteA;      // Moving sprite
    public Transform mask;         // The sprite mask object
    public GameObject vfxB;        // Effect to play
    public float edgeOffset = 0f;  // Adjust detection point

    private bool triggered = false;

    void Update()
    {
        // Calculate the mask edge, considering the offset
        float maskEdgeX = mask.position.x + edgeOffset;

        // Calculate sprite boundaries
        SpriteRenderer spriteRenderer = spriteA.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            float spriteWidth = spriteRenderer.bounds.size.x;
            float spriteLeftEdgeX = spriteA.position.x - (spriteWidth / 2); // Left edge of the sprite

            // Check if the left edge of the sprite is reaching or crossing the mask edge
            if (!triggered && spriteLeftEdgeX <= maskEdgeX)
            {
                triggered = true;

                // Spawn VFX at the mask edge position
                Instantiate(vfxB, new Vector3(maskEdgeX, spriteA.position.y, spriteA.position.z), Quaternion.identity);
            }
            // Reset triggered state when the sprite exits the mask area
            else if (triggered && spriteLeftEdgeX > maskEdgeX)
            {
                triggered = false; // Reset so VFX can play again on next entry
            }
        }
    }
}