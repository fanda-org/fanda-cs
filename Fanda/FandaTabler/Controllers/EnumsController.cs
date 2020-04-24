using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanda.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FandaTabler.Controllers
{
    public class EnumsController : Controller
    {
        public JsonResult GetAddressTypes()
        {
            //var addrTypeList = new List<SelectListItem>();
            //foreach (AddressType addrType in Enum.GetValues(typeof(AddressType)))
            //{
            //    addrTypeList.Add(new SelectListItem
            //    {
            //        Text = Enum.GetName(typeof(AddressType), addrType),
            //        Value = addrType.ToString()
            //    });
            //}
            //var list = Microsoft.AspNetCore.Html.GetEnumSelectList<AddressType>();
            //var addrTypeList = FandaEnums.GetAddressTypes();
            var addrTypeList = EnumHelper<AddressType>.GetEnumList();
            return Json(addrTypeList);
        }
    }
}
