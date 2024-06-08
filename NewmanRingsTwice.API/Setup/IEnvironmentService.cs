using NewmanRingsTwice.Domain.Shared;

namespace NewmanRingsTwice.API.Setup
{
    public interface IEnvironmentService
    {
        int GetEnvironmentType();
        EnvironmentType GetEnvironmentTypeEnum();
    }
}