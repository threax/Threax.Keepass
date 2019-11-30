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
        public EntryEntity MapEntry(EntryInput src, EntryEntity dest)
        {
            return mapper.Map(src, dest);
        }

        public Entry MapEntry(EntryEntity src, Entry dest)
        {
            return mapper.Map(src, dest);
        }

        public IQueryable<Entry> ProjectEntry(IQueryable<EntryEntity> query)
        {
            return mapper.ProjectTo<Entry>(query);
        }
    }

    public partial class EntryProfile : Profile
    {
        public EntryProfile()
        {
            //Map the input model to the entity
            MapInputToEntity(CreateMap<EntryInput, EntryEntity>());

            //Map the entity to the view model.
            MapEntityToView(CreateMap<EntryEntity, Entry>());
        }

        void MapInputToEntity(IMappingExpression<EntryInput, EntryEntity> mapExpr)
        {
            mapExpr.ForMember(d => d.ItemId, opt => opt.Ignore())
                .ForMember(d => d.Created, opt => opt.MapFrom<ICreatedResolver>())
                .ForMember(d => d.Modified, opt => opt.MapFrom<IModifiedResolver>());
        }

        void MapEntityToView(IMappingExpression<EntryEntity, Entry> mapExpr)
        {
            
        }
    }
}