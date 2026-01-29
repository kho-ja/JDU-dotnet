namespace DiApp.Services;

public interface IOperationService
{
    Guid SingletonId { get; }
    Guid ScopedId { get; }
    Guid TransientId { get; }
}

public class OperationService : IOperationService
{
    public OperationService(SingletonService singleton, ScopedService scoped, TransientService transient)
    {
        SingletonId = singleton.Id;
        ScopedId = scoped.Id;
        TransientId = transient.Id;
    }

    public Guid SingletonId { get; }
    public Guid ScopedId { get; }
    public Guid TransientId { get; }
}

public class SingletonService
{
    public Guid Id { get; } = Guid.NewGuid();
}

public class ScopedService
{
    public Guid Id { get; } = Guid.NewGuid();
}

public class TransientService
{
    public Guid Id { get; } = Guid.NewGuid();
}
