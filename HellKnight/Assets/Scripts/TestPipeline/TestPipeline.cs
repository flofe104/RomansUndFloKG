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

        protected static readonly Type TEST_ENUMERATOR_TYPE = typeof(TestEnumeratorAttribute);

        protected const string START_METHOD_NAME = "Start";

        protected static readonly string PREFAB_FIELD_NAME = nameof(RangedEnemySingleProjectileCombat.prefabForTestName);

        protected static PersistenEventNames eventNames;

        protected static MonoBehaviour testRunner;

        protected static MonoBehaviour TestRunner
        {
            get
            {
                if(testRunner == null)
                {
                    GameObject g = new GameObject("Test supervisor");
                    testRunner = g.AddComponent<TestEnumeratorRunner>();
                }
                return testRunner;
            }
        }

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

        protected static bool TryLoadPrefabFromResource(Type t, string prefabName, out MonoBehaviour s)
        {
            s = null;
            UnityEngine.Object o = Resources.Load(prefabName);
            if(o != null)
            {
                GameObject g = (GameObject)GameObject.Instantiate(o);
                s = (MonoBehaviour)g.GetComponent(t);
            }
            return s != null;
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
            TestRunner.StartCoroutine(DelayEnumeratorTest());
        }


        protected static IEnumerator DelayEnumeratorTest()
        {
            yield return new WaitForSeconds(1);
           
            foreach (Type t in GetAllTypesWithAttribute<TestMonoBehaviourAttribute>())
            {
                CallEnumeratorTestOfType(t);
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

            foreach (MethodInfo method in FilterForMethodsWithAttribute<TestEnumeratorAttribute>(methods))
            {
                TestEnumeratorMethod(t, method);
            }
        }

        protected static MethodInfo[] PrepareTypeForTests(Type t, out object source)
        {
            TestMonoBehaviourAttribute monoBehaviourTest = 
                               (t.GetCustomAttributes(TEST_MONOBEHAVIOUR_TYPE, false).FirstOrDefault()) as TestMonoBehaviourAttribute;

            MethodInfo[] methods = GetMethodsFromType(t);
            source = GetInstance(t);

            if (monoBehaviourTest != null && monoBehaviourTest.CallStartBeforeTesting)
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
                if (m.ReturnType == IENUMERATOR_TYPE)
                {
                    Debug.LogError($"Methods that should be tested as IEnumerator must be marked as \"[{nameof(TestEnumeratorAttribute)}]\"!");
                }
                else
                {
                    m.Invoke(source, null);
                    Debug.Log($"Test in class {t.Name} for method {m.Name} sucessfull");
                }
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

        protected static void TestEnumeratorMethod(Type t, MethodInfo method)
        {
            if (method.ReturnType == IENUMERATOR_TYPE)
            {
                TestRunner.StartCoroutine(EnumerableTest(t, method));
            }
            else
            {
                Debug.LogError($"Methods that should be tested as enumerator must have return type of {IENUMERATOR_TYPE.Name}!");
            }
        }

        protected static IEnumerator EnumerableTest(Type t, MethodInfo m)
        {
            TestEnumeratorAttribute enumeratorAttribute =
                          (m.GetCustomAttributes(TEST_ENUMERATOR_TYPE, false).FirstOrDefault()) as TestEnumeratorAttribute;

            if(enumeratorAttribute == null || enumeratorAttribute.delayInSecondsBeforeTestStarts < 0)
            {
                //Wait a frame so Start is called before test start
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(enumeratorAttribute.delayInSecondsBeforeTestStarts);
            }

            object source = GetInstance(t);

            yield return (IEnumerator)m.Invoke(source, null);
            Debug.Log($"Enumerator Test in class {t.Name} for method {m.Name} sucessfull");
        }


        protected static object GetInstance(Type t)
        {
            var o = GameObject.FindObjectOfType(t);
            if (o == null)
            {
                if (HasPrefabName(t, out string prefabName) && TryLoadPrefabFromResource(t, prefabName, out MonoBehaviour m))
                {
                    return m;
                }
                else
                {
                    return CreateGameObject(t);
                }
            }
            else
            {
                return o;
            }
        }

        protected static bool HasPrefabName(Type t, out string s)
        {
            s = null;
            FieldInfo f = t.GetField(PREFAB_FIELD_NAME);
            if(f!=null)
            {
                s = f.GetValue(null) as string;
            }
            return s != null;
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

            foreach (Type t in GetTypesWithAttribute<Attr>(a))
            {
                yield return t;
            }
        }

        protected static IEnumerable<Type> GetTypesWithAttribute<Attr>(Assembly assembly) where Attr : Attribute
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