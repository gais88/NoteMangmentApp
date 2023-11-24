﻿

using AutoMapper;
using Core.Models;
using NoteManagmentApp.UI.ViewModels;

namespace NoteManagmentApp.UI.Profiles
{
    public class Noteprofile: Profile
    {
        public Noteprofile()
        {
            CreateMap<Note, NoteVM>()
                .ReverseMap();

            CreateMap<Note,CreateNoteVM>()
                .ForMember(src => src.Image, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Note,UpdateNoteVM>()
                .ForMember(src => src.Image, opt => opt.Ignore())
                .ForMember(viewModel => viewModel.CurrentImage, model => model.MapFrom(model => model.ImageUrl))

                .ReverseMap();
        }
    }
}
