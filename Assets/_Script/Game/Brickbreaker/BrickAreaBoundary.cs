using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickAreaBoundary : MonoBehaviour {

	public Vector3 offset = Vector3.zero;
	public Vector3 size = Vector3.one;

	public Bounds bounds
	{
		get
		{
			return new Bounds(
				transform.position + offset, 
				new Vector3(
					size.x * transform.lossyScale.x,
					size.y * transform.lossyScale.y,
					size.z * transform.lossyScale.z));
		}
	}
	void OnDrawGizmos()
	{
		var b = bounds;
		Gizmos.DrawWireCube(b.center, b.size);
	}

}
