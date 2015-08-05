﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lte.Evaluations.Entities;

namespace Lte.Evaluations.Test.Entities
{
    [TestClass]
    public class StatValueIntervalTest
    {
        private StatValueInterval _statValueInterval;

        [TestInitialize]
        public void TestInitialize()
        {
            _statValueInterval = new StatValueInterval
            {
                Color = new Color
                {
                    ColorA = 122,
                    ColorB = 17,
                    ColorG = 201,
                    ColorR = 144
                },
                IntervalLowLevel = 10,
                IntervalUpLevel = 13
            };
        }

        [TestMethod]
        public void TestStatValueInterval_ColorStringForKml()
        {
            Assert.AreEqual(_statValueInterval.Color.ColorStringForKml, "7A11C990");
        }

        [TestMethod]
        public void TestStatValueInterval_XElement()
        {
            Assert.AreEqual(_statValueInterval.XElement.ToString(),@"<Interval>
  <LowLevel>10</LowLevel>
  <UpLevel>13</UpLevel>
  <A>122</A>
  <B>17</B>
  <R>144</R>
  <G>201</G>
</Interval>");
        }
    }
}