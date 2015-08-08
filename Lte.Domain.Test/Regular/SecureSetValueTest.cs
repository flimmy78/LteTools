﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lte.Domain.Regular;

namespace Lte.Domain.Test.Regular
{
    [TestFixture]
    public class SecureSetValueTest
    {
        interface IA
        {
            int a { get; set; }
        }

        class A : IA
        {
            public int a { get; set; }
        }

        class B : IA
        {
            public int a { get; set; }
            public int b { get; set; }
        }

        A oa = new A() { a = 1};
        IA oa1 = new A() { a = 2 };
        IA ob = new B() { a = 3, b = 4 };

        [Test]
        public void Test_SameType()
        {
            Assert.AreEqual(oa.a, 1);
            oa = oa1.GetObject<A, IA>();
            Assert.AreEqual(oa.a, 2);
        }

        [Test]
        public void Test_DifferentTypes()
        {
            try
            {
                oa = ob.GetObject<A, IA>();
                Assert.AreEqual(oa.a, 3);
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(TypeAccessException));
            }
        }
    }
}
