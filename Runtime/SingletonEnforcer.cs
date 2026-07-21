using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class SingletonEnforcer : MonoBehaviour
{
    public static List<SingletonEnforcer> instances;
    
    [Tooltip("The instance of object you want to enforce having one of. Prunes the hierarchy one time at Awake().")]
    [SerializeField] Component desiredInstance;

    void Awake()
    {
        if(SingletonEnforcer.instances == null)
        {
            instances = new List<SingletonEnforcer>();
            instances.Add(this);
        }
        else
        {
            //Establish singleton of this object
            if(SingletonEnforcer.instances.Count >= 1)
            {
                foreach(SingletonEnforcer instance in SingletonEnforcer.instances)
                {
                    if(instance.GetDesiredType() == desiredInstance.GetType())
                    {
                        Debug.Log("Another instance of SingletonEnforcer found sharing a desiredInstance type. Removing this one on " + this.gameObject.name + ".");
                        Destroy(this);
                        return;
                    }
                }
                //if you make it through the foreach loop, then no other types were found
                // in that case, add this as a new instance type.
                instances.Add(this);
            }
        }

        // get objects of type
        // if found more than 1 report it in debug.
        // Now that other versions of the singleton enforcer have been removed,
        //    carry on removing other instances of the singleton object you originally
        //    added this component to mitigate.

        Component[] foundObjects = (Component[])FindObjectsOfType(desiredInstance.GetType());

        if(foundObjects.Length > 1)
        { //Multiple objects found. Flag it and delete this one.
            Debug.Log("Multiple objects sharing the type of " + desiredInstance.GetType() + ". Removing all others besides " + this.gameObject.name + ". Please consider removing the desired ones yourself from the objects in editor.");
            foreach(Component ob in foundObjects)
            {
                if(ob != desiredInstance)
                    Destroy(ob.gameObject);
            }
        }
    }

    public System.Type GetDesiredType()
    {
        return desiredInstance.GetType();
    }
}
