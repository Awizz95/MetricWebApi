using AutoMapper;
using MetricWebApi.Models.Dto;
using MetricWebApi_Agent.Models.Dto;
using MetricWebApi_Agent.Models.Metrics;

namespace MetricWebApi
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CPUMetric, CPUMetricDto>().
                ForMember(x => 
                x.Time, 
                opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.Time)));
            CreateMap<RAMMetric, RAMMetricDto>().
                ForMember(x => 
                x.Time, 
                opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.Time)));
        }
    }
}
