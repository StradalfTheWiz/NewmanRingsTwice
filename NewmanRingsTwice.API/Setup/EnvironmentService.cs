using NewmanRingsTwice.Domain.Shared;

namespace NewmanRingsTwice.API.Setup
{
    public class EnvironmentService : IEnvironmentService
    {
        protected readonly IHostEnvironment _webHostEnvironment;

        public EnvironmentService(IHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public int GetEnvironmentType()
        {
            return (int)GetEnvironmentTypeEnum();
        }

        public EnvironmentType GetEnvironmentTypeEnum()
        {
            return GetEnvironmentTypeEnum(_webHostEnvironment);
        }

        public static EnvironmentType GetEnvironmentTypeEnum(IHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.IsDevelopment())
            {
                return EnvironmentType.Development;
            }
            else if (webHostEnvironment.IsEnvironment(nameof(EnvironmentType.Production)))
            {
                return EnvironmentType.Production;
            }

            return EnvironmentType.Unknown;
        }
    }
}
