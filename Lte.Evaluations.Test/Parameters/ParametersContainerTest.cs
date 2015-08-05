﻿using System.Collections.Generic;
using System.Linq;
using Lte.Evaluations.Service;
using Lte.Parameters.Abstract;
using Lte.Parameters.Region;
using Lte.Parameters.Region.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lte.Evaluations.Test.Parameters
{
    [TestClass]
    public class ParametersContainerTest : ParametersConfig
    {
        private readonly ParametersContainer container = new ParametersContainer();
        private readonly Mock<ITownRepository> mockTownRepository = new Mock<ITownRepository>();
        private readonly Mock<IRegionRepository> mockRegionRepositroy = new Mock<IRegionRepository>();

        [TestInitialize]
        public void TestInitialize()
        {
            Mock<IENodebRepository> eNodebRepository = new Mock<IENodebRepository>();
            mockTownRepository.SetupGet(x => x.Towns).Returns(towns.AsQueryable());
            eNodebRepository.SetupGet(x => x.GetAll()).Returns(eNodebs.AsQueryable());
            container.ImportTownENodebStats(mockTownRepository.Object, eNodebRepository.Object,
                mockRegionRepositroy.Object);
        }

        [TestMethod]
        public void TestParametersContainer_TownENodebStats()
        {
            Assert.AreEqual(container.TownENodebStats.Count(), 7);
            Assert.AreEqual(container.TownENodebStats.ElementAt(0).TotalENodebs, 2);
            Assert.AreEqual(container.TownENodebStats.ElementAt(1).TotalENodebs, 1);
            Assert.AreEqual(container.TownENodebStats.ElementAt(2).TotalENodebs, 1);
            Assert.AreEqual(container.TownENodebStats.ElementAt(3).TotalENodebs, 0);
            Assert.AreEqual(container.TownENodebStats.ElementAt(4).TotalENodebs, 2);
            Assert.AreEqual(container.TownENodebStats.ElementAt(5).TotalENodebs, 1);
            Assert.AreEqual(container.TownENodebStats.ElementAt(6).TotalENodebs, 2);
        }

        [TestMethod]
        public void TestParametersContainer_GetENodebsByDistrict_City1()
        {
            Dictionary<string, int> stat = container.GetENodebsByDistrict("City1");
            Assert.AreEqual(stat.Count(), 2);
            Assert.AreEqual(stat["District1"], 4);
            Assert.AreEqual(stat["District2"], 2);
        }

        [TestMethod]
        public void TestParametersContainer_GetENodebsByDistrict_City2()
        {
            Dictionary<string, int> stat = container.GetENodebsByDistrict("City2");
            Assert.AreEqual(stat.Count(), 2);
            Assert.AreEqual(stat["District1"], 1);
            Assert.AreEqual(stat["District3"], 2);
        }

        [TestMethod]
        public void TestParametersContainer_GetENodebsByTown_City1_District1()
        {
            Dictionary<string, int> stat = container.GetENodebsByTown("City1", "District1");
            Assert.AreEqual(stat.Count(), 3);
            Assert.AreEqual(stat["Town1"], 2);
            Assert.AreEqual(stat["Town2"], 1);
            Assert.AreEqual(stat["Town3"], 1);
        }

        [TestMethod]
        public void TestParametersContainer_GetENodebsByTown_City2_District3()
        {
            Dictionary<string, int> stat = container.GetENodebsByTown("City2", "District3");
            Assert.AreEqual(stat.Count(), 1);
            Assert.AreEqual(stat["Town2"], 2);
        }
    }
}