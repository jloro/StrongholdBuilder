using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlaceScript : MonoBehaviour, IPlaceableObject
{
    public event Action OnObjectPlaced;

    public abstract void Place();
    protected abstract void SetStart(Vector3 point);
    protected abstract void Adjust(Vector3 hit);
    protected abstract void SetEnd(Vector3 point);

    public float rotation;

    private void Awake()
    {
        Debug.Log("Awake PlaceScript");
    }

    protected void WrongPlacement(Renderer build)
    {
		List<Material> tmp = new List<Material>();
		build.GetMaterials(tmp);
		foreach (Material mat in tmp)
		{
			mat.color = Color.red;
		}
        //rend.material.shader = Shader.Find("Specular");
        //rend.material.SetColor("_SpecColor", Color.red);
    }

    protected void GoodPlacement(Renderer build)
    {
		List<Material> tmp = new List<Material>();
		build.GetMaterials(tmp);
		foreach (Material mat in tmp)
		{
			mat.color = Color.green;
		}
        //rend.material.shader = Shader.Find("Specular");
        //rend.material.SetColor("_SpecColor", Color.green);
    }

    protected Vector3 SnapToGrid(Vector3 point)
    {
        Vector3 snapPoint = new Vector3();

        snapPoint.x = Mathf.FloorToInt(point.x / 1);
        snapPoint.y = 0.5f;
        snapPoint.z = Mathf.FloorToInt(point.z / 1);

        return snapPoint;
    }

    protected bool HitTerrain(RaycastHit[] hits, out RaycastHit hit)

    {
        
		foreach (RaycastHit el in hits)
		{
			if (el.transform.gameObject.CompareTag(Tags.Terrain))
			{
				hit = el;
				return true;
			}
		}
		hit = new RaycastHit();
		return false;
    }

    protected void Rotate(GameObject go)
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            Vector3 tmp = go.transform.localEulerAngles;
            tmp.y += rotation;
            go.transform.localEulerAngles = tmp;
        }
        else if (Input.mouseScrollDelta.y > 0)
        {
            Vector3 tmp = go.transform.localEulerAngles;
            tmp.y -= rotation;
            go.transform.localEulerAngles = tmp;
        }
    }
    protected void ObjectPlaced()
    {
//        Debug.Log("Placed event triggered");
        //SA.SelectBuilding.singleton.StopBuilding();
        OnObjectPlaced?.Invoke();
    }
}
