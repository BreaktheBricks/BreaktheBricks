using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Boundary : MonoBehaviour {

	public BoxCollider2D top;
	public BoxCollider2D bottom;
	public BoxCollider2D left;
	public BoxCollider2D right;

	public float thickness = 1;

	void Start()
	{
		UpdateBoundary();
	}

	void UpdateBoundary()
	{
		var cam = Camera.main;
		if (cam != null)
		{
			var plane = new Plane(Vector3.forward, 0);
			var r = cam.ViewportPointToRay(new Vector3(0, 0));
			float enter;
			plane.Raycast(r, out enter);
			var p0 = r.GetPoint(enter);

			r = cam.ViewportPointToRay(new Vector3(1, 0));
			plane.Raycast(r, out enter);
			var p1 = r.GetPoint(enter);

			r = cam.ViewportPointToRay(new Vector3(1, 1));
			plane.Raycast(r, out enter);
			var p2 = r.GetPoint(enter);

			r = cam.ViewportPointToRay(new Vector3(0, 1));
			plane.Raycast(r, out enter);
			var p3 = r.GetPoint(enter);

			var borderOffset = new Vector3(0, thickness / 2, 0);
			top.transform.position = (p2 + p3) * 0.5f + borderOffset;
			bottom.transform.position = (p0 + p1) * 0.5f - borderOffset; 

			borderOffset = new Vector3(thickness / 2, 0, 0);
			left.transform.position = (p0 + p3) * 0.5f - borderOffset;
			right.transform.position = (p1 + p2) * 0.5f + borderOffset; 

			top.offset = Vector2.zero;
			top.size = new Vector2((p2-p3).magnitude + 1, thickness);
			bottom.offset = Vector2.zero;
			bottom.size = top.size;
			left.offset = Vector2.zero;
			left.size = new Vector2(thickness, (p0-p1).magnitude + 1);
			right.offset = Vector2.zero;
			right.size = left.size;


		}
	}

#if UNITY_EDITOR
	void Update()
	{
		UpdateBoundary();
	}
#endif	
	
}
