using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-111)]
public class ObjectPool : MonoBehaviour{
    public static ObjectPool Instance;
    
    [System.Serializable]
    public struct ObjectPoolStruct{
        public GameObject obj;
        public int amount;
        public int ID;
    }
    public ObjectPoolStruct[] objects;

    private Dictionary<int, Queue<GameObject>> _objectPoolDictionary;

    private void Awake(){
        if (Instance) Destroy(gameObject);
        else Instance = this;

        _objectPoolDictionary = new Dictionary<int, Queue<GameObject>>();

        foreach (var obj in objects){
            var queue = new Queue<GameObject>();
            for (int i = 0; i < obj.amount; i++){
                var instantiatedObject = Instantiate(obj.obj, transform, false);
                queue.Enqueue(instantiatedObject);
                instantiatedObject.SetActive(false);
            }
            _objectPoolDictionary.Add(obj.ID, queue);
        }
    }

    public GameObject ObjectPooler(int id, Vector3 position, Quaternion rotation){
        if (id == 0|| !_objectPoolDictionary.ContainsKey(id)) return null;
        
        var obj = _objectPoolDictionary[id].Dequeue();
        obj.SetActive(false);
        _objectPoolDictionary[id].Enqueue(obj);

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        
        return obj;
    }

    public string IDToName(int id){
        if (!_objectPoolDictionary.ContainsKey(id)) return "";

        var obj = _objectPoolDictionary[id].Dequeue();
        _objectPoolDictionary[id].Enqueue(obj);
        string[] x = obj.name.Split('(');
        
        return x[0];
    }
}