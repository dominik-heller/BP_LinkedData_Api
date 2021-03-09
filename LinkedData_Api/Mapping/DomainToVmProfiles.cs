using System.Collections.Generic;
using AutoMapper;

namespace LinkedData_Api.Mapping
{
    public class DomainToVmProfile : Profile
    {
        public DomainToVmProfile()
        {
            CreateMap<Model.Domain.Namespace, Model.ViewModels.Namespace>()
                .ForMember(x => x.Prefix,
                    y => y.MapFrom(z => z.Prefix))
                .ForMember(x => x.Uri,
                    y => y.MapFrom(z => z.Uri)).ReverseMap();
            CreateMap<Model.Domain.EntryResource, Model.ViewModels.EntryResource>()
                .ForMember(x => x.Command,
                    y => y.MapFrom(z => z.Command))
                .ForMember(x => x.GraphName,
                    y => y.MapFrom(z => z.GraphName)).ReverseMap();
            CreateMap<Model.Domain.EntryClass, Model.ViewModels.EntryClass>()
                .ForMember(x => x.Command,
                    y => y.MapFrom(z => z.Command))
                .ForMember(x => x.GraphName,
                    y => y.MapFrom(z => z.GraphName)).ReverseMap();
            CreateMap<Model.Domain.SupportedMethods, Model.ViewModels.SupportedMethods>()
                .ForMember(x => x.Sparql10,
                    y => y.MapFrom(z => z.Sparql10))
                .ForMember(x => x.Sparql11,
                    y => y.MapFrom(z => z.Sparql11)).ReverseMap();
            CreateMap<Model.Domain.NamedGraph, Model.ViewModels.NamedGraph>()
                .ForMember(x => x.Uri,
                    y => y.MapFrom(z => z.Uri))
                .ForMember(x => x.GraphName,
                    y => y.MapFrom(z => z.GraphName)).ReverseMap();
            CreateMap<Model.Domain.Endpoint, Model.ViewModels.EndpointVm>().ReverseMap();
        }
    }
}