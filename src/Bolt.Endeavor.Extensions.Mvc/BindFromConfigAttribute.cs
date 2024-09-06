namespace Bolt.Endeavor.Extensions.Mvc;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class BindFromConfigAttribute : Attribute
{
    public string? SectionName { get; set; }
    public bool Optional { get; set; }
}