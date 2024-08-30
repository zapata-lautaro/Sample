namespace Sample.Application.Exceptions;

internal class SampleException : Exception
{
    public SampleException(string name, Exception innerException)
        : base(name, innerException)
    {
    }
}
