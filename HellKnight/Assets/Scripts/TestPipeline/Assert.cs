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

    }
}
