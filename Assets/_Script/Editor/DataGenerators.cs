using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace tracking
{
    class TextGenerator
    {
        [MenuItem("Game Tools/Generate Text")]
        public static void GenerateText()
        {
			var gen = "./Assets/Plugins/Common/Tools/TextGenerator/TextGenerator.py";
			var pathIn = "./Assets/Res/Text/Text.xlsx";
			pathIn = Path.GetFullPath(pathIn);
			var pathOut = "./Assets/Resources/Bin_unenc";
			pathOut = Path.GetFullPath(pathOut);
			string cmdline = string.Format("python {0} {1} {2}", gen, pathIn, pathOut);
			common.EditorUtils.ExecuteCmd(cmdline);
			AssetDatabase.Refresh();
        }

		[MenuItem("Game Tools/Generate Game Data")]
		public static void GenerateGameData()
		{
			var gen = "./Assets/Plugins/Tools/GameDataGenerator.py";
			var pathIn = "./Assets/Res/GameData/GameData.xls";
			pathIn = Path.GetFullPath(pathIn);
			var pathOut = "./Assets/Resources/Bin_unenc";
			pathOut = Path.GetFullPath(pathOut);
			string cmdline = string.Format("python {0} {1} {2}", gen, pathIn, pathOut);
			common.EditorUtils.ExecuteCmd(cmdline);
			AssetDatabase.Refresh();
		} 
    }
}
