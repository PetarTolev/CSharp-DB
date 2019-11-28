namespace SoftJail
{
    using AutoMapper;
    using Data.Models;
    using DataProcessor.ImportDto;
    using Data.Models.Enums;
    using System;
    using System.Globalization;

    public class SoftJailProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public SoftJailProfile()
        {
            this.CreateMap<DepartmentImportDto, Department>();

            this.CreateMap<PrisonerImportDto, Prisoner>()
                .ForMember(p => p.IncarcerationDate, y => y.MapFrom(
                    pi => DateTime.ParseExact(pi.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(p => p.ReleaseDate, y => y.MapFrom(
                    pi => DateTime.ParseExact(pi.ReleaseDate, "dd/MM/yyyy",  CultureInfo.InvariantCulture)));

            this.CreateMap<OfficersImportDto, Officer>()
                .ForMember(o => o.Position, 
                    y => y.MapFrom(oi => Enum.Parse<Position>(oi.Position)))
                .ForMember(o => o.Weapon, 
                    y => y.MapFrom(oi => Enum.Parse<Weapon>(oi.Weapon)));
        }
    }
}
