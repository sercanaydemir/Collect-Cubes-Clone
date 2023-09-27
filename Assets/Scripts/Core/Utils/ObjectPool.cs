using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : Component
{
    /// <summary>
    /// Pooled object prefab
    /// </summary>
    [SerializeField]
    private T prefab;

    [SerializeField]
    protected Transform parent;

    /// <summary>
    /// Singleton structure
    /// </summary>
    public static ObjectPool<T> Instance { get; private set; }
    private Queue<T> objects = new Queue<T>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
    }

    /// <summary>
    /// Get generic object from pool.
    /// Create one if queue is empty 
    /// </summary>
    public T Get()
    {
        if (objects.Count == 0)
            AddObject();
        return GetObjectFromPool();
    }
    public T Get(Transform transform)
    {
        if (objects.Count == 0)
            AddObject();
        
        return GetObjectFromPool(transform);
    }

    public T GetNew()
    {
        
        return GetObjectFromPool();
    }

    /// <summary>
    /// Closes the object and returns it to the pool
    /// </summary>
    /// <param name="objectToReturn"> Object to return to the pool</param>
    public void ReturnToPool(T objectToReturn)
    {
      //  objectToReturn.transform.ResetTransform();
        
        SetParent(objectToReturn);
        objectToReturn.gameObject.SetActive(false);
        objects.Enqueue(objectToReturn);
    }

    /// <summary>
    /// Gets object from queue
    /// </summary>
    public T GetObjectFromPool()
    {
        T obj = objects.Dequeue();
        obj.transform.SetParent(null);
        obj.transform.position = prefab.transform.position;
        obj.transform.eulerAngles = prefab.transform.eulerAngles;
        obj.transform.localScale = prefab.transform.localScale;
        //SetParent(obj);

        obj.gameObject.SetActive(true);
        return obj;
    }
    public T GetObjectFromPool(Transform t)
    {
        T obj = objects.Dequeue();
        obj.transform.SetParent(t);
        obj.transform.parent = null;
        obj.transform.position = prefab.transform.position;
        obj.transform.eulerAngles = prefab.transform.eulerAngles;
        obj.transform.localScale = prefab.transform.localScale;
        
        //obj.gameObject.SetActive(true);
        return obj;
    }

    /// <summary>
    /// Creates an object and adds it to the queue
    /// </summary>
    public void AddObject()
    {
        var newObject = GameObject.Instantiate(prefab);
        SetParent(newObject);
        newObject.gameObject.SetActive(true);
        objects.Enqueue(newObject);
    }

    public T GetNewObject()
    {
        T newObject = GameObject.Instantiate(prefab);
        SetParent(newObject);
        newObject.gameObject.SetActive(true);
        return newObject;
        
    }

    public void SetParent(T obj)
    {
        if (parent == null)
            obj.transform.SetParent(transform);
        else
            obj.transform.SetParent(parent);
    }


}
