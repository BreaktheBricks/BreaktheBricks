using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Sprites;


public class BitmapFontWizard : ScriptableWizard {

	public Texture2D fontTexture;
	public Material sourceFontMaterial;
	public string fontName = "Bitmap Font";

	[MenuItem("BitmapFont/Show Bitmap Font Creator")]
	public static void ShowBitmapFontCreator()
	{
		DisplayWizard<BitmapFontWizard>("Bitmap Font Wizard", "Exit", "Create");
	}

	void OnWizardCreate()
	{
	}

	void CreateFont()
	{
		var path = AssetDatabase.GetAssetPath(fontTexture);
		if (!string.IsNullOrEmpty(path))
		{
			var spriteObjs = AssetDatabase.LoadAllAssetsAtPath(path);
			var sprites = new List<Sprite>();
			foreach (var s in spriteObjs)
			{
				var sp = s as Sprite;
				if (sp != null)
				{
					if (sp.name.Length > 0)
					{
						sprites.Add(sp);
					}
				}
			}
			var font = new Font(fontName);
			var fontPath = AssetDatabase.GenerateUniqueAssetPath("Assets/" + font.name + ".prefab");

			var characterInfo = new CharacterInfo[sprites.Count];
			// fill	UVs
			float maxHeightOfGlyph = float.MinValue;
			for (int i = 0; i < sprites.Count; ++i)
			{
				var s = sprites[i];
				var ci = new CharacterInfo();
				ci.index = s.name[0];

				var uv = DataUtility.GetOuterUV(s);
				ci.uvTopLeft = new Vector2(uv.x, uv.y);
				ci.uvBottomLeft = new Vector2(uv.x, uv.w);
				ci.uvTopRight = new Vector2(uv.z, uv.w);
				ci.uvBottomRight = new Vector2(uv.z, uv.y);
				var width = (int)s.rect.width;
				var height = (int)s.rect.height;
				ci.advance = width;
				ci.glyphWidth = width;
				ci.style = FontStyle.Normal;
				ci.maxY = 0;
				ci.minY = -height;
				characterInfo[i] = ci;
				if (height > maxHeightOfGlyph)
					maxHeightOfGlyph = height;
			}
			font.characterInfo = characterInfo;
			var material = Instantiate(sourceFontMaterial);
			material.name = "Font Material";
            material.mainTexture = fontTexture;
			font.material = material;
			AssetDatabase.CreateAsset(font, fontPath);
			var so = new SerializedObject(font);
			var lineSpacing = so.FindProperty("m_LineSpacing");
			lineSpacing.floatValue = maxHeightOfGlyph;
			so.ApplyModifiedPropertiesWithoutUndo();
			AssetDatabase.AddObjectToAsset(font.material, font);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	}

	void OnWizardOtherButton()
	{
		CreateFont();
	}



}
