using Assignment.Models;
using Mapster;

namespace Assignment.Service
{

    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<Candidate, Candidate>
                .NewConfig()
                .Ignore(dest => dest.Id);
            TypeAdapterConfig<Address, Address>
                .NewConfig()
                .Ignore(dest => dest.Id);
            TypeAdapterConfig<CandidatesAnalytics, CandidatesAnalytics>
                .NewConfig()
                .Ignore(dest => dest.Id);
            TypeAdapterConfig<Certificate, Certificate>
                .NewConfig()
                .Ignore(dest => dest.Id);
            TypeAdapterConfig<PhotoId, PhotoId>
                .NewConfig()
                .Ignore(dest => dest.Id);
        }
    }
}
