using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class _Init : MonoBehaviour {

	const string kTagInternalInit = "_InternalInit";

	class _InternalInit : MonoBehaviour
	{
		public lua.LuaStateBehaviour luaState;
		void Awake()
		{
			common.Common.instance.Init();

            lua.Config.Log = (msg) => UnityEngine.Debug.Log("[lua]" + msg);
			lua.Config.LogWarning = (msg) => UnityEngine.Debug.LogWarning("[lua]" + msg);
			lua.Config.LogError = (msg) => UnityEngine.Debug.LogError("[lua]" + msg);

			luaState = lua.LuaStateBehaviour.Create();
			lua.Api.luaL_requiref(luaState.luaVm, "pb", lua.CModules.luaopen_pb, 0);
			lua.Api.luaL_requiref(luaState.luaVm, "rapidjson", lua.CModules.luaopen_rapidjson, 0);

			lua.Lua.scriptLoader = LuaScriptLoader.ScriptLoader;
			lua.Lua.typeLoader = LuaScriptLoader.TypeLoader;

#if UNITY_EDITOR
			lua.Lua.editorGetPathDelegate = LuaScriptLoader.Editor_GetPath;
			luaState.luaVm.Editor_UpdatePath();
#endif

#if !RELEASE_VERSION
            common.DebugCallback.StartLogging(
                () =>
                {
                    var meta = new Dictionary<string, string>();
                    meta.Add("whos_build", BuildEnv.whosBuild);
                    meta.Add("server_time", MyTime.UtcNowSince1970.ToString("MM-dd HH:mm"));
                    meta.Add("game_version", BuildEnv.gameVersion);
                    meta.Add("build_number", BuildEnv.buildNumber);
                    return meta;
                });
#endif
        }

		void OnDestroy()
		{
			federation.WebRequest2.AbortAllProcessingRequests();
		}

        void OnApplicationPause(bool pauseStatus)
		{
		}

        void Update()
        {
#if !RELEASE_VERSION
            AirInspector.AirMgr.instance.Update();
#endif
        }
    }

	static _InternalInit _internalInit;
	public static lua.Lua luaVm
	{
		get
		{
			return _internalInit.luaState.luaVm;
		}
	}

	bool _preventCodeStripping;
	void _PreventCodeStripping()
	{
		if (_preventCodeStripping)
		{
			LayerMask.NameToLayer("Actor_a");
			LayerMask.GetMask("Actor_a");
			SendMessage("none");
			SendMessage("none", SendMessageOptions.DontRequireReceiver);
			SendMessage("none", null, SendMessageOptions.DontRequireReceiver);
			gameObject.SendMessage("none");
			gameObject.SendMessage("none", SendMessageOptions.DontRequireReceiver);
			gameObject.SendMessage("none", null, SendMessageOptions.DontRequireReceiver);
			gameObject.SendMessageUpwards("none");
			gameObject.SendMessageUpwards("none", SendMessageOptions.DontRequireReceiver);
			gameObject.SendMessageUpwards("none", null, SendMessageOptions.DontRequireReceiver);
			GetComponentInChildren(typeof(UnityEngine.UI.Text));
			gameObject.GetComponentInChildren(typeof(UnityEngine.UI.Text));
			transform.SetAsLastSibling();
			transform.SetAsFirstSibling();
		}
	}

	void Awake()
	{
		var go = GameObject.FindGameObjectWithTag(kTagInternalInit);
		if (go == null)
		{
			go = new GameObject(kTagInternalInit);
			go.tag = kTagInternalInit;
			GameObject.DontDestroyOnLoad(go);
			_internalInit = go.AddComponent<_InternalInit>();
		}
		_PreventCodeStripping();
    }
    


	internal class _Test
	{
		[common.FloatingDebug.Item("Memory", "OnLowMemory")]
		static void Test_OnLowMemory()
		{
			lua.LuaLowMemoryHandler.OnLowMemory();
		}
	}
}
