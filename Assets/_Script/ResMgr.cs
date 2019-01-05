using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ResMgr {

	public static Sprite LoadSprite(string uri)
	{
		return Resources.Load<Sprite>(uri);
	}

	public static Sprite[] LoadSprites(string uri)
	{
		return Resources.LoadAll<Sprite>(uri);
	}

	public static Object LoadObject(string uri)
	{
		return Resources.Load(uri);
	}

	public static byte[] LoadBytes(string uri)
	{
		var text = Resources.Load<TextAsset>(uri);
		return text.bytes;
	}

	public static string LoadString(string uri)
	{
		var text = Resources.Load<TextAsset>(uri);
		return text.text;
    }

	public static Texture LoadTexture2D(string uri)
	{
		return Resources.Load<Texture2D>(uri);
	}

	public static void UnloadUnused()
	{
		Resources.UnloadUnusedAssets();
	}

    public static string LoadJsonFile(string path, bool unenc = false)   // read json file from Resources folder
    {
        byte[] bytes;
        if (!unenc)
        {
            bytes = DlcMgr.instance.LoadEncAsBytes(path);
        }
        else
        {
            bytes = ResMgr.LoadBytes(path);
        }

        if (bytes == null)
        {
            Debug.LogError(string.Format("read json file {0} failed", path));
            return null;
        }

        string jsonString = SimpleJSON.JSON.ParseFromBytes(bytes).ToString();

        return jsonString;
    }

	public static void LoadSceneAdditive(string uri)
	{
		SceneManager.LoadScene(uri, LoadSceneMode.Additive);
	}
}
