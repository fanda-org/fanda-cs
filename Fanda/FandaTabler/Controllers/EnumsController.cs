using Fanda.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FandaTabler.Controllers
{
    [Authorize]
    public class EnumsController : Controller
    {
        [Produces("application/json")]
        public IList<EnumListItem<AddressType>> GetAddressTypes() => EnumHelper<AddressType>.GetEnumList();

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
    }
}
