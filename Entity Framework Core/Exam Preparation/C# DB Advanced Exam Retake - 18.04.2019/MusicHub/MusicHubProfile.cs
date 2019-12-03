using System;
using System.Globalization;
using MusicHub.Data.Models;
using MusicHub.DataProcessor.ExportDtos;
using MusicHub.DataProcessor.ImportDtos;

namespace MusicHub
{
    using AutoMapper;

    public class MusicHubProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public MusicHubProfile()
        {
            //Import
            //ImportProducersAlbums
            this.CreateMap<AlbumDto, Album>()
                .ForMember(a => a.ReleaseDate,
                    x => x.MapFrom(ad =>
                        DateTime.ParseExact(ad.ReleaseDate, @"dd/MM/yyyy", CultureInfo.InvariantCulture)));
            //ImportSongPerformers
            this.CreateMap<PerformerDto, Performer>();
            this.CreateMap<SongPerformerDto, SongPerformer>()
                .ForMember(sp => sp.SongId, x => x.MapFrom(spd => spd.Id));

            //export
            this.CreateMap<Song, SongAboveDurationDto>();
        }
    }
}
