using Fanda.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanda.Service
{
    public class DtoValidationException<TModel> : Exception
        where TModel : BaseDto
    {
        public TModel Model { get; set; }

        public DtoValidationException(TModel model)
        {
            Model = model;
        }
    }
}
