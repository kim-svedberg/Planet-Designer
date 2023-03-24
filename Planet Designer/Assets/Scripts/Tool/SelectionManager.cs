using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Selectable selectable;
    private bool selectionUpdatedThisFrame;
    private Reticle reticle;

    public static SelectionManager Instance { get; private set; }
    public Selectable Selectable => selectable;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (selectable && Reticle.Instance.OnPlanetSurface == false && Input.GetKeyDown(0))
            Select(null);

        if (selectable && Input.GetKeyDown(KeyCode.Escape))
            Select(null);

        if (selectable && Input.GetKeyDown(KeyCode.Backspace))
        {
            Select(null);
            selectable.Delete();
        }

        if (selectionUpdatedThisFrame)
            selectionUpdatedThisFrame = false;

        if (selectable)
            selectable.WhileSelected();
    }

    public void Select(Selectable selectable)
    {
        // Only select one selectable each frame
        if (selectionUpdatedThisFrame)
            return;

        selectionUpdatedThisFrame = true;

        // Deselect current selectable
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

        // Select new selectable
        if (selectable)
        {
            this.selectable = selectable;
            selectable.Selected = true;
            selectable.OnSelected();
        }
    }
}
