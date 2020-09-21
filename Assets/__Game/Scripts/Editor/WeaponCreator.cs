using UnityEditor;
using UnityEngine;

public class WeaponCreator : EditorWindow{
	public enum WeaponType{
		HitScan,
		Homing,
		Projectile
	}
	public WeaponType type;
	
	public enum HitScanType{
		SingleTarget,
		MultipleTargetDropOff,
		AllTargets
	}
	public HitScanType hitScanType;
	
	private string _weaponName = "new weapon";
	private int _damage;
	private HitScanWeapon _hitScanScriptableObject;
	private HomingWeapon _homingScriptableObject;
	private ProjectileWeapon _projectileScriptableObject;
	private float _speed;
	private float _rayLength;
	private LayerMask _layerMask;
	private float _maxTurnDegrees;
	private int _shotEffectID;
	private int _hitEffectID;
	private Mesh _mesh;
	private Material _material;
	private float _homingRadius;
	private float _searchNextTargetDelay;
	private int _homingHealth;
	private int _despawnParticle;

	private bool _create;
	
	[MenuItem("SHUMP/Weapon Creator")]
	static void Init(){
		WeaponCreator weaponCreator = (WeaponCreator) GetWindow(typeof(WeaponCreator));
		weaponCreator.Show();
	}

	private void OnGUI(){
		// Prefab variables
		GUILayout.Label("Prefab Options", EditorStyles.boldLabel);
		_weaponName = EditorGUILayout.TextField("Name", _weaponName);
		_mesh = EditorGUILayout.ObjectField("Mesh", _mesh, typeof(Mesh), true) as Mesh;
		_material = EditorGUILayout.ObjectField("Material", _material, typeof(Material),true) as Material;
		
		// ScriptableObject variables
		GUILayout.Label("ScriptableObject Options", EditorStyles.boldLabel);
		type = (WeaponType) EditorGUILayout.EnumPopup("Weapon Type", type);
		_damage = EditorGUILayout.IntField("Damage", _damage);

		// Depending on type variables
		switch ((int)type){
			case 0:
				// Hit scan
				_rayLength = EditorGUILayout.FloatField("Ray Length", _rayLength);
				_layerMask = EditorGUILayout.LayerField("Hittable Layer", _layerMask);
				hitScanType = (HitScanType) EditorGUILayout.EnumPopup("Type", hitScanType);
				break;
			case 1:
				// Homing
				_speed = EditorGUILayout.FloatField("Speed", _speed);
				_maxTurnDegrees = EditorGUILayout.FloatField("Max Turn Degrees", _maxTurnDegrees);
				_homingRadius = EditorGUILayout.FloatField("Homing Radius", _homingRadius);
				_searchNextTargetDelay = EditorGUILayout.FloatField("Time Between Search For Target", _searchNextTargetDelay);
				_layerMask = EditorGUILayout.LayerField("Hittable Layer", _layerMask);
				_homingHealth = EditorGUILayout.IntField("Rocket Health", _homingHealth);
				break;
			case 2:
				// Projectile
				_speed = EditorGUILayout.FloatField("Speed", _speed);
				_despawnParticle = EditorGUILayout.IntField("Despawn Particle", _despawnParticle);
				_layerMask = EditorGUILayout.LayerField("Hittable Layer", _layerMask);
				break;
		}
		
		// ObjectPool id variables
		_shotEffectID = EditorGUILayout.IntField("Shot Effect ID", _shotEffectID);
		_hitEffectID = EditorGUILayout.IntField("Shot Effect ID", _hitEffectID);
		
		EditorGUILayout.Space();

		// Create weapon
		if (GUILayout.Button("Create Weapon")){
			if (System.IO.File.Exists($"Assets/__Game/Prefabs/Weapons/{_weaponName}.prefab")){
				// Prefab already exist
				if (!EditorUtility.DisplayDialog("Prefab File Exists!",
					"Do you want to overwrite it?", "Yes", "No")) return;
			}

			if (System.IO.File.Exists($"Assets/__Game/ScriptableObjects/{_weaponName}_SO.asset")){
				// Scriptable object already exist
				if (!EditorUtility.DisplayDialog("ScriptableObject File Exists!",
					"Do you want to overwrite it?", "Yes", "No")) return;
			}
			_create = true;
		}
	}

	private void Update(){
		if (!_create) return;
		_create = false;
		CreateWeapon();
	}

	private void CreateWeapon(){
		
		var prefab = new GameObject();
		switch ((int)type){
			case 0:
				// ScriptableObject Variables Hit Scan
				var objHs = CreateInstance<HitScanWeapon>();
				objHs.damage = _damage;
				objHs.rayLength = _rayLength;
				objHs.damageAbleLayer = (1 << _layerMask);
				objHs.hitScanType = (HitScanWeapon.Type)hitScanType;
				objHs.idHitEffect = _hitEffectID;
				objHs.idShotEffect = _shotEffectID;
				objHs.name = $"{_weaponName}_SO";
				
				// Create ScriptableObject
				AssetDatabase.CreateAsset(objHs, $"Assets/__Game/ScriptableObjects/{objHs.name}.asset");
				AssetDatabase.SaveAssets();
				
				// Prefab adds for Hit Scan
				var hitScan = prefab.AddComponent<HitScan>();
				hitScan.weapon = objHs;
				
				break;
			case 1:
				// ScriptableObject Variables Homing
				var objH = CreateInstance<HomingWeapon>();
				objH.damage = _damage;
				objH.speed = _speed;
				objH.rotationSpeed = _maxTurnDegrees;
				objH.hitEffectID = _hitEffectID;
				objH.shotEffectID = _shotEffectID;
				objH.rotationSpeed = _maxTurnDegrees;
				objH.searchNextTargetDelay = _searchNextTargetDelay;
				objH.enemyLayer = (1 << _layerMask);
				objH.name = $"{_weaponName}_SO";

				// Create ScriptableObject
				AssetDatabase.CreateAsset(objH, $"Assets/__Game/ScriptableObjects/{objH.name}.asset");
				AssetDatabase.SaveAssets();
				
				// Prefab adds for Homing
				var homingRigidbody = prefab.AddComponent<Rigidbody>();
				homingRigidbody.useGravity = false;
				var homingCollider = prefab.AddComponent<SphereCollider>();
				homingCollider.isTrigger = true;
				
				var homing = prefab.AddComponent<Homing>();
				homing.weapon = objH;
				var health = prefab.AddComponent<Health>();
				health.maxHealth = _homingHealth;
				prefab.layer = 8;
				break;
			case 2:
				// ScriptableObject Variables Projectile
				var objP = CreateInstance<ProjectileWeapon>();
				objP.damage = _damage;
				objP.speed = _speed;
				objP.idHitEffect = _hitEffectID;
				objP.idShotEffect = _shotEffectID;
				objP.damageableLayer = (1 << _layerMask);
				objP.name = $"{_weaponName}_SO";

				// Create ScriptableObject
				AssetDatabase.CreateAsset(objP, $"Assets/__Game/ScriptableObjects/{objP.name}.asset");
				AssetDatabase.SaveAssets();

				// Prefab adds for Projectile
				var rigidbody = prefab.AddComponent<Rigidbody>();
				rigidbody.freezeRotation = true;
				rigidbody.useGravity = false;
				var sphereCollider = prefab.AddComponent<SphereCollider>();
				sphereCollider.isTrigger = true;
				var projectile = prefab.AddComponent<Projectile>();
				projectile.weapon = objP;
				projectile.deSpawnParticleID = _despawnParticle;
				break;
		}
		
		// Prefab Mesh
		if (_mesh != default){
			var meshFilter = prefab.AddComponent<MeshFilter>();
			meshFilter.mesh = _mesh;
		}

		// Prefab Material
		if (_material != default){
			var meshRenderer = prefab.AddComponent<MeshRenderer>();
			meshRenderer.material = _material;
		}

		// Prefab Create
		prefab.name = _weaponName;
		PrefabUtility.SaveAsPrefabAsset(prefab, $"Assets/__Game/Prefabs/Weapons/{prefab.name}.prefab");
		DestroyImmediate(prefab);
	}
}