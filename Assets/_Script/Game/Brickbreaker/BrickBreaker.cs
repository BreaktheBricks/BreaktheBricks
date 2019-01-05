using System;
using System.Collections.Generic;
using UnityEngine;

public class BrickBreaker : MonoBehaviour {

	[NonSerialized]
	public float ballMovingSpeed = 20;
	[NonSerialized]
	public float barMovingSpeed = 10;
	[NonSerialized]
	public int stride = 0;


	// brick layout params
	[NonSerialized]
	public bool scaledBrickArea = false;


	public static BrickBreaker current;
	
	
	void Awake()
	{
		current = this;
	}

	void OnDestroy()
	{
		current = null;
		if (onTouchEvent_ != null)
		{
			onTouchEvent_.Dispose();
			onTouchEvent_ = null;
		}
	}

	lua.LuaFunction onEvent_;
	public lua.LuaFunction onEvent 
	{
		get
		{
			return onEvent_;
		}
		set
		{
			if (onEvent_ != null)
			{
				onEvent_.Dispose();
			}
			if (value != null)
			{
				onEvent_ = value.Retain();
			}
			else
			{
				onEvent_ = null;
			}
		}
	}

	lua.LuaFunction onTouchEvent_;
	public lua.LuaFunction onTouchEvent
	{
		get
		{
			return onTouchEvent_;
		}
		set
		{
			if (onTouchEvent_ != null)
			{
				onTouchEvent_.Dispose();
			}
			if (value != null)
			{
				onTouchEvent_ = value.Retain();
			}
			else
			{
				onTouchEvent_ = null;
			}
		}
	}

	void _TouchBegan(int touchId, Vector2 pos, int mod=0)
	{
		if (onTouchEvent != null)
		{
			onTouchEvent.Invoke("began", touchId, ScreenPosToWorld(pos), mod);
		}
	}

	void _TouchEnded(int touchId, Vector2 pos, int mod=0)
	{
		if (onTouchEvent != null)
		{
			onTouchEvent.Invoke("ended", touchId, ScreenPosToWorld(pos), mod);
		}
	}

	void _TouchMoved(int touchId, Vector2 pos, int mod=0)
	{
		if (onTouchEvent != null)
		{
			onTouchEvent.Invoke("moved", touchId, ScreenPosToWorld(pos), mod);
		}
	}

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
	bool mouseButtonDown = false;
#endif

	void Update()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		for (int i = 0; i < Input.touchCount; ++i)
		{
			var touch = Input.GetTouch(i);
			switch (touch.phase)
			{
				case TouchPhase.Began:
					_TouchBegan(touch.fingerId, touch.position);
					break;
				case TouchPhase.Ended:
				case TouchPhase.Canceled:
					_TouchEnded(touch.fingerId, touch.position);
					break;
				case TouchPhase.Moved:
					_TouchMoved(touch.fingerId, touch.position);
					break;
			}
		}
#elif UNITY_EDITOR
		var touchId = 0;
		var mod = 0;
		if (Input.GetKey(KeyCode.LeftControl))
		{
			mod = 1;
		}
		if (Input.GetMouseButtonDown(0))
		{
			if (!mouseButtonDown)
			{
				_TouchBegan(touchId, Input.mousePosition, mod);
				mouseButtonDown = true;
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			_TouchEnded(touchId, Input.mousePosition, mod);
			mouseButtonDown = false;
		}
		if (mouseButtonDown)
		{
			_TouchMoved(touchId, Input.mousePosition, mod);
		}


		if (onEvent != null)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				onEvent.Invoke("launch_ball");
			}
			else if (Input.GetKeyDown(KeyCode.Q))
			{
				onEvent.Invoke("cast_skill");
			}
		}
#endif
	}


	static readonly Plane gameBoard = new Plane(Vector3.forward, Vector3.zero);

	public static Vector3 ScreenPosToWorld(Vector2 pos)
	{
		var cam = Camera.main;
		var ray = cam.ScreenPointToRay(new Vector3(pos.x, pos.y));
		float enter;
		gameBoard.Raycast(ray, out enter);
		return ray.GetPoint(enter);
	}

	public static void GetScreenBoundaryInWorld(out Vector3 min, out Vector3 max)
	{
		var cam = Camera.main;
		var ray = cam.ViewportPointToRay(new Vector3(0, 0, 0));
		float enter = 0;
		gameBoard.Raycast(ray, out enter);
		min = ray.GetPoint(enter);

		ray = cam.ViewportPointToRay(new Vector3(1, 1, 0));
		gameBoard.Raycast(ray, out enter);
		max = ray.GetPoint(enter);
	}

	public Vector3 GetUIWorldPos(Vector3 worldPos)
	{
		return worldPos;
	}

}
