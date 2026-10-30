using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Highlightable : MonoBehaviour
{
    private static readonly int OutlineColorId = Shader.PropertyToID("_OutlineColor");
    private static readonly int OutlineWidthId = Shader.PropertyToID("_OutlineWidth");

    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Color outlineColor = new Color(0.6f, 0.6f, 0.2f);
    [SerializeField] private float outlineWidth = 0.03f;

    private Renderer objectRenderer;
    private MaterialPropertyBlock propertyBlock;
    private int outlineMaterialIndex = -1;
    private bool isHighlighted;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();

        if (outlineMaterial != null)
        {
            Material[] sharedMaterials = objectRenderer.sharedMaterials;
            outlineMaterialIndex = sharedMaterials.Length;

            Material[] extendedMaterials = new Material[sharedMaterials.Length + 1];
            sharedMaterials.CopyTo(extendedMaterials, 0);
            extendedMaterials[outlineMaterialIndex] = outlineMaterial;
            objectRenderer.sharedMaterials = extendedMaterials;
        }

        ApplyOutlineWidth(0f);
    }

    public void SetHighlighted(bool highlighted)
    {
        if (isHighlighted == highlighted) return;

        isHighlighted = highlighted;
        ApplyOutlineWidth(highlighted ? outlineWidth : 0f);
    }

    private void ApplyOutlineWidth(float width)
    {
        if (outlineMaterialIndex < 0) return;

        objectRenderer.GetPropertyBlock(propertyBlock, outlineMaterialIndex);
        propertyBlock.SetColor(OutlineColorId, outlineColor);
        propertyBlock.SetFloat(OutlineWidthId, width);
        objectRenderer.SetPropertyBlock(propertyBlock, outlineMaterialIndex);
    }
}
