namespace MusicHub
{
    using AutoMapper;
    using Data.Models;
    using DataProcessor.ExportDtos;
    using DataProcessor.ImportDtos;
    using System;
    using System.Globalization;

    public class MusicHubProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public MusicHubProfile()
        {
            this.CreateMap<WriterDto, Writer>();

            this.CreateMap<ProducerDto, Producer>()
                .ForMember(p => p.Albums, x => x.MapFrom(pd => pd.Albums));

            this.CreateMap<AlbumDto, Album>()
                .ForMember(a => a.ReleaseDate,
                    x => x.MapFrom(ad =>
                        DateTime.ParseExact(ad.ReleaseDate, @"dd/MM/yyyy", CultureInfo.InvariantCulture)));

            this.CreateMap<PerformerDto, Performer>();

            this.CreateMap<SongPerformerDto, SongPerformer>()
                .ForMember(sp => sp.SongId, x => x.MapFrom(spd => spd.Id));
            

            this.CreateMap<Song, SongAboveDurationDto>();
        }
    }
}
