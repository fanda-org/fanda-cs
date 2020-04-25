using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanda.Shared
{
    public class DtoErrors : Dictionary<string, string>
    {
        public void AddError(string key, string errorMessage) => Add(key, errorMessage);

        public bool IsValid() => Count == 0;
    }
}
