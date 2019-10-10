using AutoMapper;
using CodeCampRESTfulApi.Data.Entities;
using CodeCampRESTfulApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeCampRESTfulApi.Data.Profiles {
    public class UserProfile : Profile {
        public UserProfile() {
            CreateMap<User, UserModel>()
                .ForMember(u => u.ConfirmPassword, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
