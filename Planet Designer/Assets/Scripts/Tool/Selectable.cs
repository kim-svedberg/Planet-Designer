using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Selectable : MonoBehaviour
{
    private bool selected;

    public bool Selected { get { return selected; } set { selected = value; } }

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
            Select();
    }

    [ContextMenu("Select")]
    public void Select()
    {
        SelectionManager.Instance.Select(this);
    }

    public abstract void OnSelected();

    public abstract void OnDeselected();

    public abstract void WhileSelected();

    public abstract void Delete();

    public abstract bool CheckClickedOn();

}
