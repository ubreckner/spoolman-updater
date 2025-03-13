namespace Domain;

public interface IOutput
{
}

public static class Output
{
    public static IOutput Empty => new EmptyOutput();

    private sealed class EmptyOutput : IOutput { }
}