using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtensions
{

    public static T GetOrAddComponent<T>(this GameObject g) where T : Component
    {
        T result = g.GetComponent<T>();
        if(result == null)
        {
            result = g.AddComponent<T>();
        }
        return result;
    }

    /// <summary>
    /// starts a coroutine that executes the given action after "delayInSeconds" seconds
    /// </summary>
    /// <param name="source">the monobehaviour to start the coroutine</param>
    /// <param name="delayInSeconds">the delay before the action is executed</param>
    /// <param name="action">the action to execute</param>
    /// <returns>return ienumerator to stop the action if needed</returns>
    public static IEnumerator DoDelayed(this MonoBehaviour source, float delayInSeconds, System.Action action)
    {
        IEnumerator result = DoActionAfterSeconds(action, delayInSeconds);
        source.StartCoroutine(result);
        return result;
    }


    /// <summary>
    /// invokes an action after waiting for "delay" seconds
    /// </summary>
    /// <param name="action"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    private static IEnumerator DoActionAfterSeconds(System.Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

}
