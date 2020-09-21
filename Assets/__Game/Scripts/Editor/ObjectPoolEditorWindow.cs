using System.Linq;
using UnityEditor;
using UnityEngine;

public class ObjectPoolEditorWindow : EditorWindow{
	[MenuItem("SHUMP/ObjectPool")]
	static void Init(){
		ObjectPoolEditorWindow objectPoolEditor = (ObjectPoolEditorWindow) GetWindow(typeof(ObjectPoolEditorWindow));
		objectPoolEditor.Show();
	}

	private GameObject _obj;
	private int _amount = 1;
	private GameObject _poolPrefab;
	private ObjectPool _objectPool;
	private bool _addItem = false;
	private bool _creatingItem = false;
	private Vector2 _scrollPos;

	private void Awake(){
		LoadObjectPool();
	}

	private void OnGUI(){
		GUILayout.Label("Add Object to Pool", EditorStyles.boldLabel);
		// Item to add to ObjectPool variables
		_obj = EditorGUILayout.ObjectField("GameObject", _obj, typeof(GameObject), true) as GameObject;
		_amount = EditorGUILayout.IntField("Amount", _amount);

		// Add to Pool Button
		GUILayout.Space(10f);
		if (GUILayout.Button("Add to Pool")){
			_addItem = true;
		}
		GUILayout.Space(10f);

		// Show content of ObjectPool
		if(_creatingItem || _poolPrefab == null) return;
		GUILayout.Label("Objects in Pool", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical();
		_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
		
		for (int i = 0; i < _objectPool.objects.Length; i++){
			GUILayout.Label($"Object {i}", EditorStyles.boldLabel);
			EditorGUILayout.ObjectField("GameObject", _objectPool.objects[i].obj, typeof(GameObject), true);
			EditorGUILayout.IntField("Amount", _objectPool.objects[i].amount);
			EditorGUILayout.IntField("ID", _objectPool.objects[i].ID);
		}
		
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}

	private void Update(){
		// Begins the process to add object to pool
		if(_addItem) AddToPool();
	}

	private void LoadObjectPool(){
		// Check if file doesnt exist, if it doesn't create such file
		if (!System.IO.File.Exists($"Assets/__Game/Prefabs/ObjectPool/ObjectPool.prefab")){
			var newObjectPool = new GameObject("ObjectPool");
			newObjectPool.AddComponent<ObjectPool>();
		}
		
		// Loads the ObjectPool
		_poolPrefab = PrefabUtility.LoadPrefabContents("Assets/__Game/Prefabs/ObjectPool/ObjectPool.prefab");
		_objectPool = _poolPrefab.GetComponent<ObjectPool>();
	}

	private void AddToPool(){
		_addItem = false;
		if(_obj == null || _amount < 1) return;
		_creatingItem = true;
		
		// Make array a list
		var list = _objectPool.objects.ToList();
		var itemToPool = list[0];

		// Set variables
		itemToPool.amount = _amount;
		itemToPool.obj = _obj;
		var id = list.Count + 1;
		itemToPool.ID = id;

		// Check if ID exist, if it does add 1 to the ID and check again
		IDCheck:	
		foreach (var obj in list){
			if (obj.ID == id){
				itemToPool.ID += 1;
				goto IDCheck;
			}
		}
		
		// Add new object to pool and convert back to array
		list.Add(itemToPool);
		var array = list.ToArray();
		
		// Delete old ObjectPool in scene
		var oldScenePrefab = GameObject.Find("ObjectPool");
		DestroyImmediate(oldScenePrefab);
		
		// Create new prefab object
		var newPrefab = new GameObject("ObjectPool");
		newPrefab.transform.parent = GameObject.FindWithTag("Managers").transform;
		var objPoolComponent = newPrefab.AddComponent<ObjectPool>();
		objPoolComponent.objects = new ObjectPool.ObjectPoolStruct[array.Length];
		objPoolComponent.objects = array;

		// Save prefab
		var prefabToScene = PrefabUtility.SaveAsPrefabAsset(newPrefab, "Assets/__Game/Prefabs/ObjectPool/ObjectPool.prefab");
		DestroyImmediate(newPrefab);
		PrefabUtility.InstantiatePrefab(prefabToScene);
		
		// Reload object pool
		LoadObjectPool();
		_creatingItem = false;
		Repaint();
	}
}