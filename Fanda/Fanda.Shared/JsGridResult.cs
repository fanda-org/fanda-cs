using System;
using System.Collections.Generic;
using System.Text;

namespace Fanda.Shared
{
    public class JsGridResult<T> where T: class
    {
        public T Data { get; set; }
        public int ItemsCount { get; set; } 
    }
}
