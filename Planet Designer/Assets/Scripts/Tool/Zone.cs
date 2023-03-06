using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class Zone : MonoBehaviour
{
    [SerializeField] private GeographicCoordinates coordinates;
    [SerializeField] private float radius;
    [SerializeField] private Color gizmosColor;
    [SerializeField] private bool selected;

#if UNITY_EDITOR
    private void OnEnable()
    {
        SceneView.onSceneGUIDelegate += this.OnSceneMouseOver;
    }
#endif

    [ExecuteAlways]
    private void Update()
    {
        if (!selected)
            return;

        selected = false;

#if UNITY_EDITOR
        Vector3 mousePositionInWorld = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
        //GameObject go = HandleUtility.world
        //Hand

        if (Input.GetMouseButtonDown(0))
        {

        }
#endif

    }

#if UNITY_EDITOR
    private void OnSceneMouseOver(SceneView view)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;
        //And add switch Event.current.type for checking Mouse click and switch tiles
        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.DrawRay(ray.origin, hit.transform.position, Color.blue, 5f);
            Debug.Log(hit.transform.name);
            Debug.Log(hit.transform.position);
        }
    }
#endif

    private void OnDrawGizmosSelected()
    {
        selected = true;
        Gizmos.color = gizmosColor;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
