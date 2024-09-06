namespace Bolt.Endeavor.Extensions.Mvc;

internal interface IDataKeySettings
{
    public string TenantRouteName { get; }
    public string TenantQueryName { get; }
}