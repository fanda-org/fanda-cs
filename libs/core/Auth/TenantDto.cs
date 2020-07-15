namespace Fanda.Core.Auth
{
    using Fanda.Core.Base;

    public class TenantDto : BaseDto
    {
        public int OrgCount { get; set; }
    }

    public class TenantListDto : BaseListDto
    {
        public int OrgCount { get; set; }
    }
}