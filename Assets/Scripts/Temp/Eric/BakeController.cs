using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BakeController : MonoBehaviour
{
    public PlaceScript placeableObject;
    public NavMeshSurface surface;

    private void Awake()
    {
        Debug.Log("Awake BakeController");
        placeableObject.OnObjectPlaced += PlaceableObject_OnObjectPlaced;
    }

    private void PlaceableObject_OnObjectPlaced()
    {
        Debug.Log("BAKE CONTROLLER NOTIFIED");
        surface.BuildNavMesh();
    }

    private void OnDestroy()
    {
        placeableObject.OnObjectPlaced -= PlaceableObject_OnObjectPlaced;
    }
}
