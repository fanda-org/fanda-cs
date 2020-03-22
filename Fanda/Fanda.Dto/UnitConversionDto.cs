using System;

namespace Fanda.Data
{
    public class UnitConversionDto
    {
        //public Guid Id { get; set; }
        public Guid FromUnitId { get; set; }
        public Guid ToUnitId { get; set; }
        public byte CalcStep { get; set; }
        public char Operator { get; set; }
        public decimal Factor { get; set; }
        public bool Active { get; set; }
    }
}