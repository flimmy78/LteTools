﻿using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Region.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lte.Evaluations.Test.Parameters
{
    [TestClass]
    public class RegionViewModelAddTownConditionsTest
    {
        [TestMethod]
        public void TestRegionViewModelAddTownConditions_EmptyNewNames()
        {
            RegionViewModel viewModel = new RegionViewModel("")
            {
                CityName = "Foshan",
                NewCityName = "",
                DistrictName = "Chancheng",
                NewDistrictName = "",
                TownName = "Nanzhuang",
                NewTownName = ""
            };
            Town town = viewModel.AddTownConditions;
            Assert.AreEqual(town.CityName, "Foshan");
            Assert.AreEqual(town.DistrictName, "Chancheng");
            Assert.AreEqual(town.TownName, "");
        }

        [TestMethod]
        public void TestRegionViewModelAddTownConditions_NewTown()
        {
            RegionViewModel viewModel = new RegionViewModel("")
            {
                CityName = "Foshan",
                NewCityName = "",
                DistrictName = "Chancheng",
                NewDistrictName = "",
                TownName = "Nanzhuang",
                NewTownName = "Chengqu"
            };
            Town town = viewModel.AddTownConditions;
            Assert.AreEqual(town.CityName, "Foshan");
            Assert.AreEqual(town.DistrictName, "Chancheng");
            Assert.AreEqual(town.TownName, "Chengqu");
        }

        [TestMethod]
        public void TestRegionViewModelAddTownConditions_NewDistrict()
        {
            RegionViewModel viewModel = new RegionViewModel("")
            {
                CityName = "Foshan",
                NewCityName = "",
                DistrictName = "Chancheng",
                NewDistrictName = "Nanhai",
                TownName = "Nanzhuang",
                NewTownName = "Chengqu"
            };
            Town town = viewModel.AddTownConditions;
            Assert.AreEqual(town.CityName, "Foshan");
            Assert.AreEqual(town.DistrictName, "Nanhai");
            Assert.AreEqual(town.TownName, "Chengqu");
        }

        [TestMethod]
        public void TestRegionViewModelAddTownConditions_NewCity()
        {
            RegionViewModel viewModel = new RegionViewModel("")
            {
                CityName = "Foshan",
                NewCityName = "Shenzhen",
                DistrictName = "Chancheng",
                NewDistrictName = "Nanhai",
                TownName = "Nanzhuang",
                NewTownName = "Chengqu"
            };
            Town town = viewModel.AddTownConditions;
            Assert.AreEqual(town.CityName, "Shenzhen");
            Assert.AreEqual(town.DistrictName, "Nanhai");
            Assert.AreEqual(town.TownName, "Chengqu");
        }

    }
}
