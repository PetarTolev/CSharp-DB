using System;
using AutoMapper;
using TeisterMask.Data.Models;
using TeisterMask.Data.Models.Enums;
using TeisterMask.DataProcessor.ImportDto;

namespace TeisterMask.DataProcessor
{
    public class TeisterMaskProfiler : Profile
    {
        public TeisterMaskProfiler()
        {
            this.CreateMap<ProjectDto, Project>();
            this.CreateMap<TaskDto, Task>();
            //.ForMember(t => t.LabelType, x => x.MapFrom(td => Enum.Parse<LabelType>(td.LabelType)))
            //.ForMember(t => t.ExecutionType, x => x.MapFrom(td => Enum.Parse<ExecutionType>(td.ExecutionType)));

            this.CreateMap<EmployeeDto, EmployeeDto>()
                .ForMember(e => e.EmployeesTasks, x => x.MapFrom(ed => ed.EmployeesTasks));
        }
    }
}