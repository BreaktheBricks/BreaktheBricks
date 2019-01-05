using federation;
using lua;

class Fed_Lua
{
	public static bool IsFedInited()
	{
		return Fed.instance.isInitialized;
	}

	public static void InitFed()
	{
		Fed.DestoryInstance();
		Fed.instance.Init(BuildEnv.clientId);
	}

	public static bool CheckInited()
	{
		return Fed.instance.isInitialized;
	}

	public static string srvUrl
	{
		get
		{
			return Fed.instance.Eve_GetConfig("MightyBattleAgent");
		}
	}

    public static string gateWayUrl
    {
        get
        {
            return Fed.instance.Eve_GetConfig("Gateway");
        }
    }
    
    public static string clientId
	{
		get
		{
			return BuildEnv.clientId;
		}
	}
}
