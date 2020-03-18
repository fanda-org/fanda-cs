﻿using System;

namespace Fanda.Dto
{
    public class ProductVarietyDto
    {
        public Guid VarietyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}