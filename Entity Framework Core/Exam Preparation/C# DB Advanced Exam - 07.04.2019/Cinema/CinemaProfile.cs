﻿namespace Cinema
{
    using AutoMapper;
    using Data.Models;
    using DataProcessor.ImportDto;

    public class CinemaProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public CinemaProfile()
        {
            this.CreateMap<HallImportDto, Hall>()
                .ForMember(h => h.Seats, hi => hi.Ignore());

            this.CreateMap<ProjectionImportDto, Projection>();

            this.CreateMap<CustomerImportDto, Customer>()
                .ForMember(ci => ci.Tickets, x => x.MapFrom(c => c.Tickets));

            this.CreateMap<TicketImportDto, Ticket>();
        }
    }
}
