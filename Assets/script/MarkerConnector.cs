using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerConnector : MonoBehaviour
{
    public Transform[] connectedMarker;

    void OnDrawGizmos()
    {

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Transform child1 = transform.GetChild(i);
            Transform child2 = transform.GetChild(i + 1);
            Gizmos.color = Color.yellow;
            UnityEditor.Handles.Label(child1.position, child1.name);
            Gizmos.DrawLine(child1.position, child2.position);
        }
        Gizmos.DrawLine(transform.GetChild(transform.childCount-1).position, transform.GetChild(0).position);
         UnityEditor.Handles.Label(transform.GetChild(transform.childCount-1).position, transform.GetChild(transform.childCount-1).name);
    }
}
