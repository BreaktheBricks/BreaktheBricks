using System.Collections;
using System.Collections.Generic;
using System.Net;
using federation;
using lua;
using ICSharpCode.SharpZipLib.Tar;
using System.IO;
using System;
using UnityEngine;

public class WebRequest2_Lua
{
	public static void Download_Lua(string url, string storePath, lua.LuaFunction complete)
	{
		if (!string.IsNullOrEmpty(storePath))
		{
			storePath = System.IO.Path.Combine(Application.persistentDataPath, storePath);
			var dirName = Path.GetDirectoryName(storePath);
			if (!Directory.Exists(dirName))
			{
				try
				{
					Directory.CreateDirectory(dirName);
				}
				catch(Exception e)
				{
					complete.Invoke(false, "error creating directory " + dirName+ "\n" + e.Message);
					return;
				}
			}
		}
		var localComplete = complete.Retain();
		WebRequest2.Download(
			url, (bytes) =>
			{
				if (string.IsNullOrEmpty(storePath))
				{
					localComplete.Invoke(true, bytes);
				}
				else
				{
					try
					{
						if (bytes != null)
						{
							File.WriteAllBytes(storePath, bytes);
							localComplete.Invoke(true);
						}
						else
						{
							localComplete.Invoke(false, url + " is not downloaded");
						}
					}
					catch (Exception e)
					{
						localComplete.Invoke(false, "error writing file " + storePath + "\n" + e.Message);
					}
				}
				localComplete.Dispose();
			});
	}


	public static void DownloadAndUnpackTar_Lua(string url, string unpackPath, lua.LuaFunction complete)
	{
		unpackPath = Path.Combine(Application.persistentDataPath, unpackPath);
		if (!Directory.Exists(unpackPath))
		{
			try
			{
				Directory.CreateDirectory(unpackPath);
			}
			catch (Exception e)
			{
				complete.Invoke(false, "error creating directory " + unpackPath + "\n" + e.Message);
				return;
			}
		}

		var localComplete = complete.Retain();
		WebRequest2.Download(
			url, (bytes) => {
				try
				{
					var ms = new MemoryStream(bytes);
					var archive = TarArchive.CreateInputTarArchive(ms);
					archive.ExtractContents(unpackPath);
					localComplete.Invoke(true);
				}
				catch (System.Exception e)
				{
					localComplete.Invoke(false, e.Message);
				}
				localComplete.Dispose();
			});
	}

	static WebRequest2.Context defaultContext = new WebRequest2.Context() {
		responseAsRawString = true,
        requestAsjson = true
	};

	public static void Post_Lua(string url, string function, lua.LuaTable parameter, lua.LuaFunction complete, WebRequest2.Context context = null, string parametersStr = "")
	{
		Dictionary<string, object> param = new Dictionary<string, object>();
		if (context == null)
		{
			context = defaultContext;
		}
		if(parameter != null)
		{
			var L = _Init.luaVm;
			parameter.Push();
			Api.lua_pushnil(L);
			while(Api.lua_next(L, -2) != 0)
			{
				var key = Api.lua_tostring(L, -2);
				var value = L.ValueAt(-1);
				param.Add(key, value);
				Api.lua_pop(L, 1); // pop value
			}
			Api.lua_pop(L, 1); // pop table
		}

		var localComplete = complete.Retain();
		WebRequest2.Post(new System.Uri(url), function, param,
			(s, ResCode, payloadObj, cookies, headers, localContext) =>
			{
				if(s == WebExceptionStatus.Success && ResCode == HttpStatusCode.OK)
				{
					var payload = (SimpleJSON.JSONClass)payloadObj;
					if (context.responseAsRawString)
					{
						localComplete.Invoke(true, (string)payload["payload"]);
					}
					else
					{
						localComplete.Invoke(true, payload.ToString());
					}
				}
				else
				{
					localComplete.Invoke(false);
				}
				localComplete.Dispose();
            }, context, parametersStr);
	}
}
