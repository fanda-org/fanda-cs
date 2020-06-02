using System.Collections.Generic;

namespace Fanda.Dto.Base
{
    public class DtoErrors : Dictionary<string, List<string>>
    {
        public void AddErrors(string field, string errorMessage)
        {
            if (TryGetValue(field, out List<string> errors))
            {
                errors.Add(errorMessage);
                this[field] = errors;
            }
            else
            {
                errors.Add(errorMessage);
                Add(field, errors);
            }
        }
    }
}
