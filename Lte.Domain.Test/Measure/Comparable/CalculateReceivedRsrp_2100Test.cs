﻿using Lte.Domain.Geo.Abstract;
using Lte.Domain.Measure;
using Lte.Domain.Test.Broadcast;
using Lte.Domain.TypeDefs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lte.Domain.Test.Measure.Comparable
{
    [TestClass]
    public class CalculateReceivedRsrp_2100Test
    {
        private readonly ComparableCell ccell = new ComparableCell();
        private readonly Mock<ILinkBudget<double>> budget = new Mock<ILinkBudget<double>>();
        private readonly Mock<IOutdoorCell> ocell = new Mock<IOutdoorCell>();
        private readonly Mock<IBroadcastModel> model = new Mock<IBroadcastModel>();
        const double eps = 1E-6;

        [TestInitialize]
        public void TestInitialize()
        {
            model.MockFrequencyType(FrequencyBandType.Downlink2100);
            model.MockUrbanTypeAndKValues(UrbanType.Dense);           
            budget.SetupGet(x => x.AntennaGain).Returns(18);
            budget.SetupGet(x => x.TransmitPower).Returns(15.2);
            ocell.SetupGet(x => x.Height).Returns(40);          
            ccell.AzimuthAngle = 0;
            budget.SetupGet(x => x.Model).Returns(model.Object);
            ccell.Cell = ocell.Object;           
        }

        [TestMethod]
        public void TestMethod_10mDistance()
        {   
            ccell.Distance = 0.01;
            double rsrp = ccell.CalculateReceivedRsrp(budget.Object, 20);
            Assert.AreEqual(rsrp, -41.048422, eps);
        }

        [TestMethod]
        public void TestMethod_20mDistance()
        {
            ccell.Distance = 0.02;
            double rsrp = ccell.CalculateReceivedRsrp(budget.Object, 10);
            Assert.AreEqual(rsrp, -45.951366, eps);
        }

        [TestMethod]
        public void TestMethod_50mDistance()
        {
            ccell.Distance = 0.05;
            double rsrp = ccell.CalculateReceivedRsrp(budget.Object, 10);
            Assert.AreEqual(rsrp, -65.651985, eps);
        }

        [TestMethod]
        public void TestMethod_100mDistance()
        {
            ccell.Distance = 0.1;
            double rsrp = ccell.CalculateReceivedRsrp(budget.Object, 5);
            Assert.AreEqual(rsrp, -75.554929, eps);
        }

        [TestMethod]
        public void TestMethod_200mDistance()
        {
            ccell.Distance = 0.2;
            double rsrp = ccell.CalculateReceivedRsrp(budget.Object, 2);
            Assert.AreEqual(rsrp, -87.457873, eps);
        }

        [TestMethod]
        public void TestMethod_500mDistance()
        {
            ccell.Distance = 0.5;
            double rsrp = ccell.CalculateReceivedRsrp(budget.Object, 0);
            Assert.AreEqual(rsrp, -105.158492, eps);
        }
    }
}