using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestableBehaviourSetup
{
    

    public static T GetInstance<T>() where T : MonoBehaviour
    {
        GameObject g = new GameObject();
        T t = g.AddComponent<T>();
        return t;
    }


}
