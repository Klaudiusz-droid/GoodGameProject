using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Transform interactionOrigin;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactableLayers = ~0;

    private Highlightable currentTarget;

    public Highlightable CurrentTarget => currentTarget;

    private void Update()
    {
        Transform origin = interactionOrigin != null ? interactionOrigin : transform;
        Highlightable hitTarget = null;

        if (Physics.Raycast(origin.position, origin.forward, out RaycastHit hit, interactDistance, interactableLayers))
        {
            hitTarget = hit.collider.GetComponentInParent<Highlightable>();
        }

        if (hitTarget == currentTarget) return;

        if (currentTarget != null)
        {
            currentTarget.SetHighlighted(false);
        }

        currentTarget = hitTarget;

        if (currentTarget != null)
        {
            currentTarget.SetHighlighted(true);
        }
    }
}
