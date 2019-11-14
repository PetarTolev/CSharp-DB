namespace CarDealer
{
    using AutoMapper;
    using DTO.Import;
    using Models;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<CarImportDto, Car>();
        }
    }
}
