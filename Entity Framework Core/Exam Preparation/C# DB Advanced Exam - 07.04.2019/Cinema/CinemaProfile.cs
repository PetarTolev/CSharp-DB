using System.Linq;

namespace Cinema
{
    using AutoMapper;
    using Data.Models;
    using DataProcessor.ImportDto;

    public class CinemaProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public CinemaProfile()
        {
            this.CreateMap<MovieDto, Movie>();

            this.CreateMap<HallDto, Hall>()
                .ForMember(h => h.Seats, x => x.MapFrom(hd => Enumerable
                                                                                                .Range(0, hd.Seats)
                                                                                                .Select(s => new Seat())
                                                                                                .ToList()));

            this.CreateMap<ProjectionDto, Projection>();

            this.CreateMap<CustomerDto, Customer>();
            this.CreateMap<TicketDto, Ticket>();

        }
    }
}
