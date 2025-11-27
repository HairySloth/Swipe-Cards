using UnityEngine;

public class CardShredController : MonoBehaviour
{
    public SpriteRenderer sr;

    [Tooltip("World-space X coordinate of where shredding begins.")]
    public float cutBoundaryX = -2f;

    Material mat;
    float spriteWidth;

    void Start()
    {
        mat = sr.material;

        // width in world units
        spriteWidth = sr.bounds.size.x;
    }

    void Update()
    {
        UpdateShred();
    }

    void UpdateShred()
    {
        // 1. Get the card's left edge (world space)
        float leftEdge = sr.bounds.min.x;

        // 2. How far it has crossed the cut boundary
        float offAmount = cutBoundaryX - leftEdge;

        // If it hasn't crossed, no shredding
        if (offAmount <= 0)
            return;

        // 3. Normalize (0 = no shred, 1 = fully shredded)
        float normalizedCut = Mathf.Clamp01(offAmount / spriteWidth);

        // 4. Only increase shredding (don't un-shred)
        float current = mat.GetFloat("_ShredAmount");
        mat.SetFloat("_ShredAmount", Mathf.Max(current, normalizedCut));
    }
}