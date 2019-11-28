using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tracking;
using Threax.Keepass.InputModels;
using Threax.Keepass.Database;
using Threax.Keepass.ViewModels;
using System.Linq;

namespace Threax.Keepass.Mappers
{
    public partial class AppMapper
    {
        public ItemEntity MapItem(ItemInput src, ItemEntity dest)
        {
            return mapper.Map(src, dest);
        }

        public Item MapItem(ItemEntity src, Item dest)
        {
            return mapper.Map(src, dest);
        }

        public IEnumerable<Item> ProjectItem(IEnumerable<ItemEntity> query)
        {
            return query.Select(i => mapper.Map<Item>(i));
        }
    }

    public partial class ItemProfile : Profile
    {
        public ItemProfile()
        {
            //Map the input model to the entity
            MapInputToEntity(CreateMap<ItemInput, ItemEntity>());

            //Map the entity to the view model.
            MapEntityToView(CreateMap<ItemEntity, Item>());
        }

        void MapInputToEntity(IMappingExpression<ItemInput, ItemEntity> mapExpr)
        {
            mapExpr.ForMember(d => d.ItemId, opt => opt.Ignore())
                .ForMember(d => d.Created, opt => opt.MapFrom<ICreatedResolver>())
                .ForMember(d => d.Modified, opt => opt.MapFrom<IModifiedResolver>());
        }

        void MapEntityToView(IMappingExpression<ItemEntity, Item> mapExpr)
        {
            
        }
    }
}