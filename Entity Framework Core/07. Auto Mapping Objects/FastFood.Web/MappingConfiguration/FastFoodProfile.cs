namespace FastFood.Web.MappingConfiguration
{
    using AutoMapper;
    using Models;
    using ViewModels.Categories;
    using ViewModels.Employees;
    using ViewModels.Items;
    using ViewModels.Orders;
    using ViewModels.Positions;

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            #region Positions configurations

            this.CreateMap<CreatePositionInputModel, Position>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.PositionName));

            this.CreateMap<Position, PositionsAllViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.Name));

            #endregion

            #region Employee configurations

            this.CreateMap<Position, RegisterEmployeeViewModel>()
                .ForMember(re => re.PositionName, y => y.MapFrom(p => p.Name));

            this.CreateMap<RegisterEmployeeInputModel, Employee>();

            this.CreateMap<Employee, EmployeesAllViewModel>()
                .ForMember(ea => ea.Position, y => y.MapFrom(e => e.Position.Name));

            #endregion

            #region Category configurations

            this.CreateMap<CreateCategoryInputModel, Category>()
                .ForMember(c => c.Name, y => y.MapFrom(cc => cc.CategoryName));

            #endregion

            #region Item configurations

            this.CreateMap<Category, CreateItemViewModel>()
                .ForMember(ci => ci.CategoryName, y => y.MapFrom(c => c.Name));

            this.CreateMap<CreateItemInputModel, Item>();

            this.CreateMap<Item, ItemsAllViewModels>()
                .ForMember(ia => ia.Category, y => y.MapFrom(i => i.Category.Name));

            #endregion

            #region Order configurations

            this.CreateMap<Order, CreateOrderInputModel>()
                .ForMember(co => co.EmployeeName, y => y.MapFrom(o => o.Employee.Name));

            this.CreateMap<CreateOrderInputModel, Order>();

            this.CreateMap<Order, OrderAllViewModel>()
                .ForMember(oa => oa.Employee, y => y.MapFrom(o => o.Employee.Name))
                .ForMember(oa => oa.OrderId, y => y.MapFrom(o => o.Id))
                .ForMember(oa => oa.DateTime, y => y.MapFrom(o => o.DateTime.ToString("g")));

            #endregion
        }
    }
}
