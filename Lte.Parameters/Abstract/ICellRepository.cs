﻿using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Abstract
{
    public interface ICellRepository
    {
        IQueryable<Cell> Cells { get; }

        void AddOneCell(Cell cell);

        bool RemoveOneCell(Cell cell);

        void SaveChanges();
    }

    public interface ICdmaCellRepository
    {
        IQueryable<CdmaCell> Cells { get; }

        void AddOneCell(CdmaCell cell);

        bool RemoveOneCell(CdmaCell cell);

        void SaveChanges();
    }

    public interface INearestPciCellRepository
    {
        List<NearestPciCell> NearestPciCells { get; }
            
        NearestPciCell Import(ICell cell, short pci);

        void AddNeighbors(ILteNeighborCellRepository repository, int eNodebId);
    }

    public interface ILteNeighborCellRepository
    {
        IQueryable<LteNeighborCell> NeighborCells { get; }

        IQueryable<NearestPciCell> NearestPciCells { get; } 

        void AddOneCell(LteNeighborCell cell);

        bool RemoveOneCell(LteNeighborCell cell);

        void SaveChanges();
    }

    public interface IMrsCellRepository
    {
        IQueryable<MrsCellDate> MrsCells { get; }

        IQueryable<MrsCellTa> TaCells { get; }

        void AddOneCell(MrsCellDate cell);

        void AddOneCell(MrsCellTa cell);

        bool RemoveOneCell(MrsCellDate cell);

        bool RemoveOneCell(MrsCellTa cell);

        void SaveChanges();
    }

    public interface IMroCellRepository
    {
        IQueryable<MroRsrpTa> RsrpTaCells { get; }

        void AddOneCell(MroRsrpTa cell);

        bool RemovceOneCell(MroRsrpTa cell);

        void SaveChanges();
    }
}