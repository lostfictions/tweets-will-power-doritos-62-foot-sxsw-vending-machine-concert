using UnityEngine;
using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

using UnityEditor;
using System.IO;
using System.Linq;

[CustomEditor(typeof(TweetQueue))]
public class TweetQueueEditor : Editor
{
	readonly string assetPath = "Assets/Meshes";
	readonly string assetName = "Font.fbx";
	readonly string generatedAssetDir = "AutoPrefabs";

	bool foldout1 = false;

	Material mat;

	public override void OnInspectorGUI()
	{
		TweetQueue tq = (TweetQueue)target;

		EditorGUIUtility.LookLikeControls();

		if(GUILayout.Button("Generate char\nprefabs and\npopulate lists", GUILayout.ExpandWidth(false)))
			AutoPopulateLists();

		mat = (Material)EditorGUILayout.ObjectField("Material", mat, typeof(Material), false);


		foldout1 = EditorGUILayout.Foldout(foldout1, "Character map");
		if(foldout1)
		{
			for(int i=0; i<tq.chars.Length; i++)
			{
				tq.charMeshes[i] = (GameObject)EditorGUILayout.ObjectField(tq.chars[i], tq.charMeshes[i], typeof(GameObject), false);
			}
		}

		EditorGUIUtility.LookLikeInspector();

		DrawDefaultInspector();

		if(GUI.changed)
			EditorUtility.SetDirty(tq);
	}

	void AutoPopulateLists()
	{
		//First generate the prefabs.

		if(!File.Exists(assetPath + "/" + assetName))
		{
			Debug.LogWarning("File not found: " + assetPath + "/" + assetName);
			return;
		}

		if(Directory.Exists(assetPath + "/" + generatedAssetDir))
		{
			var files = Directory.GetFiles(assetPath + "/" + generatedAssetDir, "*.prefab");
			foreach(var f in files)
			{
				AssetDatabase.MoveAssetToTrash(f);
				//TODO: store the list of files instead and then check for
				//files that should no longer exist once all prefabs are
				//replaced
			}
		}
		else
		{
			AssetDatabase.CreateFolder(assetPath, generatedAssetDir);
		}

		var assets = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath + "/" + assetName);

		foreach(var go in assets.Where(a => a is GameObject && a.name.Length == 1))
		{
			var prefab = PrefabUtility.CreatePrefab(assetPath + "/" + generatedAssetDir + "/" + go.name + ".prefab", (GameObject)go);

			var mc = prefab.AddComponent<MeshCollider>();
			mc.convex = true;
			mc.isTrigger = true;

			var rb = prefab.AddComponent<Rigidbody>();
			rb.isKinematic = true;

			if(mat)
				prefab.renderer.sharedMaterial = mat;
		}

		AssetDatabase.SaveAssets();

		////////////////////////////
		//Now populate the dictionary lists.

		var prefabPaths = Directory.GetFiles(assetPath + "/" + generatedAssetDir, "*.prefab");
		((TweetQueue)target).chars = new string[prefabPaths.Length];
		((TweetQueue)target).charMeshes = new GameObject[prefabPaths.Length];
		for(int i=0; i<prefabPaths.Length; i++)
		{
			GameObject go = (GameObject)AssetDatabase.LoadMainAssetAtPath(prefabPaths[i]);
			// if(go.name.Length > 1)
				// Debug.LogWarning("Prefab \"" + go.name + "\"'s name is longer than one!\nThis'll probably cause problems.");
				// This shouldn't happen, since we check for name length when parsing assets
			((TweetQueue)target).chars[i] = go.name[0].ToString();
			((TweetQueue)target).charMeshes[i] = go;
		}
		
		EditorUtility.SetDirty(target);
	}

}
