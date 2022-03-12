using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Testing
{
    public class TestPipeline
    {


        protected static readonly Type OBJECT_TYPE = typeof(object);

        protected static readonly Type TEST_RESULT_TYPE = typeof(TestResult);

        protected static readonly Type IENUMERATOR_TYPE = typeof(IEnumerator);

        protected static PersistenEventNames eventNames;
        protected static PersistenEventNames EventNames
        {
            get
            {
                if(eventNames == null)
                {
                    eventNames = (PersistenEventNames)Resources.Load("TestEvents");
                }
                return eventNames;
            }
        }

        [MenuItem("Testing/StartAllTestsInPlaymode")]
        public static void StartAllMethodTestInPlaymode()
        {
            AddEvent(nameof(TestAllMethods));
            EditorApplication.isPlaying = true;
        }

        protected static void ResetTestEvents()
        {
            EventNames.EventNames.Clear();
        }

        protected static void AddEvent(string name)
        {
            EventNames.EventNames.Add(name);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void CallActiveEventsWhenSceneLoaded()
        {
            foreach (string functionName in EventNames.EventNames)
                GetFunctionOfThisType(functionName).Invoke(null, null);

            ResetTestEvents();
        }


        protected static void TestAllMethods()
        {
            foreach (Type t in GetAllTypesWithAttribute<TestMonoBehaviourAttribute>())
            {
                CallTestsOfType(t);
            }
        }

        protected static void CallTestsOfType(Type t)
        {
            MethodInfo[] methods = GetMethodsFromType(t);
            object source = GetInstance(t);
            foreach (MethodInfo method in FilterForMethodsWithAttribute<TestOnceAttribute>(methods))
            {
                TestMethodOnce(t, method, source);
            }
            foreach (MethodInfo method in FilterForMethodsWithAttribute<TestEnumeratorAttribute>(methods))
            {
                TestEnumeratorMethod(t, method, source);
            }
        }


        protected static void TestMethodOnce(Type t, MethodInfo method, object source)
        {
            if(method.ReturnType != TEST_RESULT_TYPE)
            {
                Debug.LogError($"Methods that should be tested must have return type of {nameof(TestResult)}!");
            }
            TestResult result = (TestResult)method.Invoke(source, null);
            EvaluateResultFor(t, method, result);
        }

        protected static void TestEnumeratorMethod(Type t, MethodInfo method, object source)
        {
            if (method.ReturnType != IENUMERATOR_TYPE)
            {
                Debug.LogError($"Methods that should be tested must have return type of {nameof(TestResult)}!");
            }
            ((MonoBehaviour)source).StartCoroutine(EnumerableTest(method, source));
        }

        protected static IEnumerator EnumerableTest(MethodInfo method, object source)
        {
            yield return (IEnumerator)method.Invoke(source, null);
            yield return null;
        }

        protected static void EvaluateResultFor(Type t, MethodInfo m, TestResult r)
        {
            if (r.expectedValue != r.actualValue)
            {
                Debug.LogError($"Test failed in class {t.Name} for method {m.Name}!" +
                    $"The expected result was {r.expectedValue} but the actual result was {r.actualValue}");
            }
        }


        protected static object GetInstance(Type t)
        {
            var o = GameObject.FindObjectOfType(t);
            if(o == null)
            {
                return CreateGameObject(t);
            }
            else
            {
                return o;
            }
        }

        protected static object CreateGameObject(Type t)
        {
            MethodInfo method = GetFunctionOfThisType(nameof(CreateGameObjectWithMonobheaviour));

            method = method.MakeGenericMethod(t);
            return method.Invoke(null, null);
        }
       

        protected static MethodInfo GetFunctionOfThisType(string name)
        {
            return typeof(TestPipeline).GetMethod(name,
                               BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        }

        protected static T CreateGameObjectWithMonobheaviour<T>() where T : MonoBehaviour
        {
            GameObject g = new GameObject();
            T result = g.AddComponent<T>();
            return result;
        }

        protected static IEnumerable<MethodInfo> FilterForMethodsWithAttribute<Attr>(MethodInfo[] methods) where Attr : Attribute
        {
            Type attrType = typeof(Attr);
            return methods.Where(m => m.IsDefined(attrType, true));
        }

        protected static MethodInfo[] GetMethodsFromType(Type target)
        {
            List<MethodInfo> fields = new List<MethodInfo>();

            ///add all protected and private fields of current type
            fields.AddRange(target.GetMethods((BindingFlags.Public
                | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)));

            ///add all private methods of parent types
            Type parentType = target;
            while (parentType != OBJECT_TYPE)
            {
                parentType = parentType.BaseType;
                fields.AddRange(parentType.GetMethods((
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)).Where(f => !f.IsFamily));
            }

            return fields.ToArray();
        }

        protected static IEnumerable<Type> GetAllTypesWithAttribute<Attr>() where Attr : Attribute
        {
            Assembly a = Assembly.GetExecutingAssembly();

            //Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Type t in GetTypesWithHelpAttribute<Attr>(a))
            {
                yield return t;
            }
        }

        protected static IEnumerable<Type> GetTypesWithHelpAttribute<Attr>(Assembly assembly) where Attr : Attribute
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(Attr), true).Length > 0)
                {
                    yield return type;
                }
            }
        }

    }

}