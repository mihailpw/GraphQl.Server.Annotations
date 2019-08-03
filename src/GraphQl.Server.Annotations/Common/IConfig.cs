namespace GraphQl.Server.Annotations.Common
{
    internal interface IConfig
    {
        bool ThrowIfPropertyNotFound { get; }
        bool ThrowIfPropertiesTypesDifferent { get; }
    }
}