using Fanda.Dto;
using System;

namespace Fanda.Service.Base
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
