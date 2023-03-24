using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public class PoolableType
{
    [Tooltip(@"The Name of this Type of Poolable Object (Does not effect operation)")]
    public string TypeName;

    [Tooltip(@"The Prefab of the gameobject to be pooled")]
    public GameObject Prefab;
    [HideInInspector]
    [Tooltip(@"The sorting tag is used to organize object pools.  Each object in a given pool will have it's name set to this tag value.")]
    public string SortingTag = "";

    [Tooltip(@"The maximum number of objects allowed to be instantiated.")]
    public int Max;

    [Tooltip(@"If true, the object pool can ignore the maximum if there are no objects in storage, and another is requested.")]
    public bool AutoExpand;
}
public class ObjectPooler : Singleton<ObjectPooler>
{
    [SerializeField]
    private List<PoolableType> PoolableTypes = new List<PoolableType>();
    [SerializeField]
    private bool UseParentTransforms = false;
    private Dictionary<string, PoolableType> TagTypeLookup;
    private Dictionary<GameObject, PoolableType> PrefabTypeLookup;
    private Dictionary<string, Queue<GameObject>> SleepingObjects;
    private Dictionary<string, HashSet<GameObject>> ActiveObjects;
    private Dictionary<string, Transform> TypeParents;

    //Init
    public override void Awake()
    {

        // init dictionaries
        if (UseParentTransforms)
            TypeParents = new Dictionary<string, Transform>();
        TagTypeLookup = new Dictionary<string, PoolableType>();
        PrefabTypeLookup = new Dictionary<GameObject, PoolableType>();
        SleepingObjects = new Dictionary<string, Queue<GameObject>>();
        ActiveObjects = new Dictionary<string, HashSet<GameObject>>();
        // init types and sets in dicts
        foreach (PoolableType type in PoolableTypes)
        {
            if (UseParentTransforms)
            {
                // create empty GameObject to store children under
                GameObject SortinParent = new GameObject(type.TypeName + " Parent");
                // assign this GameObject to be a child of whatever GameObject this script is attached to
                SortinParent.transform.parent = transform;
                // add a reference to this parent in the TypeParents lookup
                TypeParents[type.SortingTag] = SortinParent.transform;
            }
            //
            type.SortingTag = type.TypeName;
            // add a reference for this sorting tag in each of the required dicitonaries
            TagTypeLookup.Add(type.SortingTag, type);
            PrefabTypeLookup.Add(type.Prefab, type);
            SleepingObjects.Add(type.SortingTag, new Queue<GameObject>());
            ActiveObjects.Add(type.SortingTag, new HashSet<GameObject>());

            // init sleeping objects
            for (int i = 0; i < type.Max; i++)
            {
                // create gameobject and set it's parent as appropriate
                GameObject pooledObject = Instantiate(type.Prefab, UseParentTransforms ? TypeParents[type.SortingTag] : transform);
                // name the gameobject for easy sorting
                pooledObject.name = type.SortingTag;
                // inactivate the gameobject for storage
                pooledObject.SetActive(false);
                // add this object to the sleeping pool
                SleepingObjects[type.SortingTag].Enqueue(pooledObject);
            }
        }
    }

    //Destroy
    public static void Destroy(GameObject obj) => Instance._Destroy(obj);
    public static void DestroyWithTag(string objName) => Instance._Destroy(objName);

    private void _Destroy(GameObject obj)
    {
        string sortingTag = obj.name;
        // check if this object is one we actually pool
        if (!ActiveObjects.ContainsKey(sortingTag)) return;
        // check if this object is actually in our active set right now
        if (!ActiveObjects[sortingTag].Contains(obj)) return;
        // since it is, we will deactivate the object
        obj.SetActive(false);
        // then remove it from the active set
        ActiveObjects[sortingTag].Remove(obj);
        // and add it to the sleeping queue
        SleepingObjects[sortingTag].Enqueue(obj);
    }
    private void _Destroy(string objName)
    {
        string sortingTag = objName;
        // check if this object is one we actually pool
        List<GameObject> goList = ActiveObjects[sortingTag].ToList();
        if (!ActiveObjects.ContainsKey(sortingTag)) return;
        // check if this object is actually in our active set right now
        if (!ActiveObjects[sortingTag].Contains(goList[0])) return;
        // since it is, we will deactivate the object
        goList[0].SetActive(false);
        // then remove it from the active set
        ActiveObjects[sortingTag].Remove(goList[0]);
        // and add it to the sleeping queue
        SleepingObjects[sortingTag].Enqueue(goList[0]);
    }

    //Generate
    public static GameObject Generate<T>(T Identifier, Vector3 Position, Quaternion Rotation) =>
        Instance._Generate(Identifier, Position, Rotation);

    /// <summary>
    /// Grab a member from the list of SleepingObjects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Identifier">Either the Prefab to instantiate or the Tag for the PoolableType</param>
    /// <param name="Position">The Position to activate the object at</param>
    /// <returns>A gameobject from the correct pool</returns>
    public static GameObject Generate<T>(T Identifier, Vector3 Position) =>
        Instance._Generate(Identifier, Position, Quaternion.identity);

    /// <summary>
    /// Grab a member from the list of SleepingObjects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Identifier">Either the Prefab to instantiate or the Tag for the PoolableType</param>
    /// <returns>A gameobject from the correct pool</returns>
    public static GameObject Generate<T>(T Identifier) =>
        Instance._Generate(Identifier, Vector3.zero, Quaternion.identity);

    private GameObject _Generate<T>(T Identifier, Vector3 Position, Quaternion Rotation)
    {
        // get the tag of the prefab we wish to instantiate
        string Tag = "";
        // did the user pass us a string tag?
        if (Identifier is string)
        {
            if (!SleepingObjects.ContainsKey(Identifier as string)) return null;
            else Tag = Identifier as string;
        }
        // did the user pass us a prefab?
        else if (Identifier is GameObject)
        {
            // check if this prefab is one that we manage
            if (!PrefabTypeLookup.ContainsKey(Identifier as GameObject)) return null;
            // if we manage this type, get the tag
            else Tag = PrefabTypeLookup[Identifier as GameObject].SortingTag;
        }
        else return null;

        // If we make it here, we know the tag has been set and exists.
        GameObject Member;
        // if no objects are left in the queue, but we can auto expand, then make a new object
        if (SleepingObjects[Tag].Count == 0 && TagTypeLookup[Tag].AutoExpand)
        {
            Member = Instantiate(TagTypeLookup[Tag].Prefab, UseParentTransforms ? TypeParents[Tag] : transform);
            Member.name = Tag;
        }
        // else if there are members in the queue get the member from the sleeping queue
        else if (SleepingObjects[Tag].Count > 0)
            Member = SleepingObjects[Tag].Dequeue();
        // else we cannot instantiate
        else return null;

        // Now that we have a member, we can update it's variables and return it
        Member.transform.position = Position;
        Member.transform.rotation = Rotation;
        ActiveObjects[Tag].Add(Member);

        // can't forget to do this!
        Member.SetActive(true);

        return Member;
    }
}
