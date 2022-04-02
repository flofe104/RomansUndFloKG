using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Testing
{
    public class Assert
    {

        public static void AreEqual(int actual, int expected)
        {
            if (!actual.Equals(expected))
                throw new Exception($"Expected value was {expected} but actual value was {actual}.");
        }

        public static void AreEqual(float actual, float expected)
        {
            if (!actual.Equals(expected))
                throw new Exception($"Expected value was {expected} but actual value was {actual}.");
        }

        public static void AreEqual(object actual, object expected)
        {
            if(!actual.Equals(expected))
                throw new Exception($"Expected value was {expected} but actual value was {actual}.");
        }

        public static void IsTrue(bool actual)
        {
            if (actual != true)
                throw new Exception($"Expected value was {true} but actual value was {actual}.");
        }


        public static void AreNotEqual(object actual, object expected)
        {
            if (actual.Equals(expected))
                throw new Exception($"Values {expected} and {actual} should not match.");
        }

        public static void AreNotEqual(int actual, int expected)
        {
            if (actual.Equals(expected))
                throw new Exception($"Values {expected} and {actual} should not match.");
        }

        public static void AreNotEqual(float actual, float expected)
        {
            if (actual.Equals(expected))
                throw new Exception($"Values {expected} and {actual} should not match.");
        }


        public static void GreaterOrEqual(float first, float snd)
        {
            if (first <= snd)
                throw new Exception($"{first} was expected to be larger equal than {snd}.");
        }

        public static void LessOrEqual(float first, float snd)
        {
            if (first >= snd)
                throw new Exception($"{first} was expected to be smaller equal than {snd}.");
        }
        public static void Greater(float first, float snd)
        {
            if (first < snd)
                throw new Exception($"{first} was expected to be larger equal than {snd}.");
        }

        public static void Lesser(float first, float snd)
        {
            if (first > snd)
                throw new Exception($"{first} was expected to be smaller equal than {snd}.");
        }
        public static void ApproxEqual(float first, float snd, float e = 0.1f)
        {
            if(first > snd + e || first < snd - e)
                throw new Exception($"{first} was expected to be approximately equal to {snd}.");
        }


    }
}
