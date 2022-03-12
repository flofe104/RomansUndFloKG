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

        protected static readonly Type IENUMERATOR_TYPE = typeof(IEnumerator);

        protected static readonly Type TEST_MONOBEHAVIOUR_TYPE = typeof(TestMonoBehaviourAttribute);

        protected const string START_METHOD_NAME = "Start";

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


        [MenuItem("Testing/StartAllSingleTestsInPlaymode")]
        public static void StartSingleMethodTestInPlaymode()
        {
            AddEvent(nameof(TestAllSingleTestMethods));
            EditorApplication.isPlaying = true;
        }

        [MenuItem("Testing/StartAllEnumeratorTestsInPlaymode")]
        public static void StartEnumeratorMethodTestInPlaymode()
        {
            AddEvent(nameof(TestAllEnumeratorTestMethods));
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
                GetFunctionOfThisTypeWithName(functionName).Invoke(null, null);

            ResetTestEvents();
        }


        protected static void TestAllSingleTestMethods()
        {
            foreach (Type t in GetAllTypesWithAttribute<TestMonoBehaviourAttribute>())
            {
                CallOnceTestsOfType(t); 
            }
            EditorApplication.isPlaying = false;
        }

        protected static void TestAllEnumeratorTestMethods()
        {
            foreach (Type t in GetAllTypesWithAttribute<TestMonoBehaviourAttribute>())
            {
                CallEnumeratorTestOfType(t);
            }
        }


        protected static void CallAllTestsOfType(Type t)
        {
            MethodInfo[] methods = PrepareTypeForTests(t, out object source);

            foreach (MethodInfo method in FilterForMethodsWithAttribute<TestAttribute>(methods))
            {
                TestMethodOnce(t, method, source);
            }
            foreach (MethodInfo method in FilterForMethodsWithAttribute<TestEnumeratorAttribute>(methods))
            {
                TestEnumeratorMethod(t, method, source);
            }
        }

        protected static void CallOnceTestsOfType(Type t)
        {
            MethodInfo[] methods = PrepareTypeForTests(t, out object source);

            foreach (MethodInfo method in FilterForMethodsWithAttribute<TestAttribute>(methods))
            {
                TestMethodOnce(t, method, source);
            }
        }

        protected static void CallEnumeratorTestOfType(Type t)
        {
            MethodInfo[] methods = GetMethodsFromType(t);
            object source = GetInstance(t);

            foreach (MethodInfo method in FilterForMethodsWithAttribute<TestEnumeratorAttribute>(methods))
            {
                TestEnumeratorMethod(t, method, source);
            }
        }

        protected static MethodInfo[] PrepareTypeForTests(Type t, out object source)
        {
            TestMonoBehaviourAttribute monoBehaviourTest = (TestMonoBehaviourAttribute)
                               (t.GetCustomAttributes(TEST_MONOBEHAVIOUR_TYPE, false)[0]);

            MethodInfo[] methods = GetMethodsFromType(t);
            source = GetInstance(t);

            if (monoBehaviourTest.CallStartBeforeUnitTesting)
            {
                MethodInfo m = GetFunctionOfAnyTypeWithName(t, START_METHOD_NAME);
                if (m != null)
                {
                    m.Invoke(source, null);
                }
            }
            return methods;
        }

        protected static void TestMethodOnce(Type t, MethodInfo m, object source)
        {
            try
            {
                m.Invoke(source, null);
                Debug.Log($"Test in class {t.Name} for method {m.Name} sucessfull");
            }
            catch(Exception ex)
            {
                if(ex.InnerException != null)
                {
                    Debug.LogError($"Test in class {t.Name} for method {m.Name} unsucessfull: {ex.InnerException}");
                }
                else
                {
                    Debug.LogError($"Test in class {t.Name} for method {m.Name} unsucessfull: {ex}");
                }
            }
        }

        protected static void TestEnumeratorMethod(Type t, MethodInfo method, object source)
        {
            if (method.ReturnType == IENUMERATOR_TYPE)
            {
                ((MonoBehaviour)source).StartCoroutine(EnumerableTest(t, method, source));
            }
            else
            {
                Debug.LogError($"Methods that should be tested as enumerator must have return type of {IENUMERATOR_TYPE.Name}!");
            }
        }

        protected static IEnumerator EnumerableTest(Type t, MethodInfo m, object source)
        {
            //Wait a frame so update can be called
            
            yield return null;
            yield return (IEnumerator)m.Invoke(source, null);
            Debug.Log($"Enumerator Test in class {t.Name} for method {m.Name} sucessfull");
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
            MethodInfo method = GetFunctionOfThisTypeWithName(nameof(CreateGameObjectWithMonobheaviour));

            return method.Invoke(null, new Type[] { t });
        }
       
        public static T CreateNewInstanceOf<T>() where T : MonoBehaviour
        {
            GameObject g = new GameObject();
            T result = g.AddComponent<T>();
            return result;
        }


        protected static MethodInfo GetFunctionOfThisTypeWithName(string name)
        {
            return GetFunctionOfAnyTypeWithName(typeof(TestPipeline), name);
        }

        protected static MethodInfo GetFunctionOfAnyTypeWithName(Type t, string name)
        {
            return t.GetMethod(name,
                               BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        }

        protected static object CreateGameObjectWithMonobheaviour(Type t)
        {
            GameObject g = new GameObject();
            object result = g.AddComponent(t);
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

            ///add all protected and private methods of current type
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