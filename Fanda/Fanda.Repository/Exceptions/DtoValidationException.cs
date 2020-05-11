using Fanda.Dto.Base;
using System;

namespace Fanda.Repository.Exceptions
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
