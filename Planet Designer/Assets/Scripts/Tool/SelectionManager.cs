using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject reticlePrefab;
    [SerializeField] private Selectable selectable;
    [SerializeField] private LayerMask raycastLayerMask;
    private bool selectionUpdatedThisFrame;
    private Reticle reticle;

    public static SelectionManager Instance { get; private set; }
    public Vector3 MouseSurfacePosition { get; private set; }
    public GeographicCoordinates MouseSurfaceGeographicCoordinates { get; private set; }
    public Selectable Selectable => selectable;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        reticle = Instantiate(reticlePrefab).GetComponent<Reticle>();
        reticle.name = "Reticle";
        reticle.gameObject.SetActive(false);
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit, 3000f, raycastLayerMask))
        {
            MouseSurfacePosition = raycastHit.point;
            MouseSurfaceGeographicCoordinates = GeographicCoordinates.FromPosition(MouseSurfacePosition);

            if (!reticle.gameObject.activeSelf)
            {
                reticle.gameObject.SetActive(true);
                Cursor.visible = false;
            }

            reticle.GeographicTransform.Coordinates = MouseSurfaceGeographicCoordinates;
            reticle.GeographicTransform.UpdateTransform();
        }

        else
        {
            if (reticle.gameObject.activeSelf)
            {
                reticle.gameObject.SetActive(false);
                Cursor.visible = true;
            }  
        }

        if (selectionUpdatedThisFrame)
            selectionUpdatedThisFrame = false;

        if (selectable)
            selectable.WhileSelected();
    }

    public void Select(Selectable selectable)
    {
        if (selectionUpdatedThisFrame)
            return;

        selectionUpdatedThisFrame = true;

        if (this.selectable)
        {
            this.selectable.Selected = false;
            this.selectable.OnDeselected();

            if (this.selectable == selectable)
            {
                this.selectable = null;
                return;
            }
        }

        this.selectable = selectable;
        selectable.Selected = true;
        selectable.OnSelected();
    }
}
