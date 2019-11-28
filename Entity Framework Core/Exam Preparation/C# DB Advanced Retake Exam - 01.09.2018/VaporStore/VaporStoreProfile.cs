namespace VaporStore
{
    using AutoMapper;
    using Data.Models;
    using DataProcessor.Dto.Import;

    public class VaporStoreProfile : Profile
	{
		// Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
		public VaporStoreProfile()
        {
            this.CreateMap<GameImportDto, Game>();

            this.CreateMap<Game, GameImportDto>();
        }
	}
}