﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Shared
{
    public class BaseDuplicate
    {
        [Required]
        public DuplicateField Field { get; set; }
        [Required]
        public string Value { get; set; }
        public Guid Id { get; set; } = default;
    }

    public class BaseOrgDuplicate : BaseDuplicate
    {
        public Guid OrgId { get; set; } = default;
    }

    public enum DuplicateField
    {
        Id = 1,
        Code = 2,
        Name = 3
    }
}