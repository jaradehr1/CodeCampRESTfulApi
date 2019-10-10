using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CodeCampRESTfulApi.Data.Entities;
using CodeCampRESTfulApi.Models;

namespace CodeCampRESTfulApi.Data.Profiles
{
    public class CampProfile : Profile
    {
        public CampProfile() {
            this.CreateMap<Camp, CampModel>()
                .ForMember(c => c.Venue, o => o.MapFrom(m => m.Location.VenueName))
                .ReverseMap();

            this.CreateMap<Talk, TalkModel>()
                .ReverseMap()
                .ForMember(t => t.Camp, opt => opt.Ignore())
                .ForMember(t => t.Speaker, opt => opt.Ignore());

            this.CreateMap<Speaker, SpeakerModel>()
                .ReverseMap();
        }
    }
}
