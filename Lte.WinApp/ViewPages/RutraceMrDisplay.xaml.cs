﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Lte.Domain.Regular;
using Lte.Evaluations.Rutrace.Entities;
using Lte.Parameters.Abstract;
using Lte.Parameters.Concrete;
using Lte.Parameters.Region.Concrete;
using Lte.Parameters.Service.Lte;
using Lte.WinApp.Import;
using Lte.WinApp.Models;

namespace Lte.WinApp.ViewPages
{
    /// <summary>
    /// RutraceMrDisplay.xaml 的交互逻辑
    /// </summary>
    public partial class RutraceMrDisplay : Page
    {
        private readonly ILteNeighborCellRepository _neighborCellRepository = new EFLteNeighborCellRepository();
        private readonly IENodebRepository _eNodebRepository = new EFENodebRepository();
        private readonly ICellRepository _cellRepository = new EFCellRepository();
        private readonly IMrsCellRepository _mrsRepository = new EFMrsCellRepository();
        private readonly IMroCellRepository _mroRepository = new EFMroCellRepository();

        private readonly IInterferenceStatRepository _interferenceStatRepository
            = new EFInterferenceStatRepository();
        private readonly MroFilesImporter mroFilesImporter;
        private readonly MrsFilesImporter mrsFilesImporter;

        public RutraceMrDisplay()
        {
            InitializeComponent();
            PageTitle.Content = Title;
            mroFilesImporter = new MroFilesImporter(_cellRepository, _neighborCellRepository);
            mrsFilesImporter = new MrsFilesImporter();
        }

        private void QueryNeighbor_OnClick(object sender, RoutedEventArgs e)
        {
            int eNodebId = ENodebId.Text.ConvertToInt(10000);
            byte sectorId = SectorId.Text.ConvertToByte(48);
            NeighborsWithoutPci.ItemsSource = null;
            NeighborsWithoutPci.ItemsSource = _neighborCellRepository.NeighborCells.Where(x =>
                x.CellId == eNodebId  && x.SectorId == sectorId).ToList();

            NeighborsWithPci.ItemsSource = null;
            NeighborsWithPci.ItemsSource = _neighborCellRepository.NearestPciCells.Where(x =>
                x.CellId == eNodebId && x.SectorId == sectorId).ToList();
        }

        private IEnumerable<FileInfo> SelectMrFiles(DirectoryInfo dir, string keyword)
        {
            return (from eNodebDir in dir.GetDirectories() 
                let eNodebId = eNodebDir.Name.ConvertToInt(0) 
                where _eNodebRepository.GetAll().FirstOrDefault(x=>x.ENodebId==eNodebId) != null 
                select eNodebDir).SelectMany(eNodebDir => eNodebDir.GetFiles().Where(x=>
                    x.Name.IndexOf(keyword, StringComparison.Ordinal)>=0 && x.Extension==".xml"));
        }

        private void OpenMrDirectory_OnClick(object sender, RoutedEventArgs e)
        {
            DirectoryDialogWrapper wrapper=new MrDirectoryDialogWrapper();
            if (!wrapper.ShowDialog()) return;
            DirectoryPath.Content = wrapper.Directory;
            DirectoryInfo dir = new DirectoryInfo(wrapper.Directory);
            cmd.AppendText("D:\\\n");
            foreach (DirectoryInfo eNodebDir in dir.GetDirectories())
            {
                if (eNodebDir.GetFiles().FirstOrDefault(x => x.Extension == ".zip")==null) continue;
                cmd.AppendText("cd " + eNodebDir.FullName + "\n");
                cmd.AppendText("D:\\安装文件\\WinRAR\\WinRAR e *.zip\n");
                cmd.AppendText("del *.zip\n");
            }
        }

        private void OpenMro_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DirectoryPath.Content.ToString())) return;
            DirectoryInfo dir = new DirectoryInfo(DirectoryPath.Content.ToString());

            if (ImportMrs.IsChecked == true)
            {
                mrsFilesImporter.Import(SelectMrFiles(dir, "MRS").Select(x => x.FullName),
                    path =>
                    {
                        MrsRecordSet recordSet;
                        using (StreamReader reader = new StreamReader(path))
                        {
                            recordSet = new MrsRecordSet(reader);
                        }
                        return recordSet;
                    });

                MrCoverage.ItemsSource = null;
                MrCoverage.ItemsSource = mrsFilesImporter.RsrpStatList;
                TaDistributions.ItemsSource = null;
                TaDistributions.ItemsSource = mrsFilesImporter.TaStatList;
            }
            if (ImportMro.IsChecked == true)
            {
                mroFilesImporter.Import(SelectMrFiles(dir, "MRO").Select(x => x.FullName),
                    path =>
                    {
                        MroRecordSet recordSet;
                        using (StreamReader reader = new StreamReader(path))
                        {
                            recordSet = new MroRecordSet(reader);
                        }
                        return recordSet;
                    });
                RsrpTa.ItemsSource = null;
                RsrpTa.ItemsSource = mroFilesImporter.RsrpTaStatList;
                Interference.ItemsSource = null;
                Interference.ItemsSource = mroFilesImporter.InterferenceStats;
            }
        }

        private void SaveMrInDb_Click(object sender, RoutedEventArgs e)
        {
            if (mrsFilesImporter.RsrpStatList != null)
            {
                using (QueryMrsCellService service = new QueryMrsCellService(_mrsRepository))
                {
                    service.SaveStats(mrsFilesImporter.RsrpStatList);
                    service.SaveTaStats(mrsFilesImporter.TaStatList);
                }
            }
            if (mroFilesImporter.RsrpTaStatList != null)
            {
                using (QueryMroCellService service = new QueryMroCellService(_mroRepository))
                {
                    service.SaveRsrpTaStats(mroFilesImporter.RsrpTaStatList);
                }
            }
            if (mroFilesImporter.InterferenceStats != null)
            {
                _interferenceStatRepository.Save(mroFilesImporter.InterferenceStats);
            }
        }

    }
}