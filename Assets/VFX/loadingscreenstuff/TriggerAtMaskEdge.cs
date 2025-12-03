using UnityEngine;

public class SpriteAnimationLooper : MonoBehaviour
{
    public GameObject[] spritePrefabs; // Array of sprite prefabs
    public Transform mask;              // The sprite mask object
    public float edgeOffset = 0f;       // Adjust detection point

    private int currentSpriteIndex = 0;

    void Start()
    {
        // Start the coroutine to spawn and play the first sprite.
        StartCoroutine(SpawnAndPlayCurrentSprite());
    }

    System.Collections.IEnumerator SpawnAndPlayCurrentSprite()
    {
        while (true) // Loop through the sprites indefinitely
        {
            // Instantiate the current sprite prefab
            GameObject currentSprite = Instantiate(spritePrefabs[currentSpriteIndex], transform.position, Quaternion.identity);
            Animator animator = currentSprite.GetComponent<Animator>();

            if (animator != null)
            {
                // Wait until the current animation state is done
                while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    yield return null; // Wait for the next frame
                }
            }

            // Destroy the current sprite after the animation is done
            Destroy(currentSprite);

            // Move to the next sprite and loop back if necessary
            currentSpriteIndex = (currentSpriteIndex + 1) % spritePrefabs.Length;
        }
    }
}