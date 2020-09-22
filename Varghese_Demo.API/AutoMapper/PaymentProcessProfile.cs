﻿using AutoMapper; 
using System; 
using CodeDemo.API.AppDatabase.Entities;
using CodeDemo.API.Models;

namespace CodeDemo.API.AutoMapper
{
    /// <summary>
    /// Automapper
    /// </summary>
    public class PaymentProcessProfile : Profile
    {
        /// <summary>
        /// Create Mappings
        /// </summary>
        public PaymentProcessProfile()
        {
            CreateMap<PaymentData, PaymentProcess>()
                .ForMember(
                dest => dest.PaymentDate,
                opt => opt.MapFrom(src => $"{DateTime.Now}"));

            CreateMap<PaymentProcess, PaymentHistory>()
                .ForMember(
                    dest => dest.CardNumber,
                    opt => opt.MapFrom(src => $"{new string('X', src.CardNumber.Length - 4) + src.CardNumber.Substring(src.CardNumber.Length - 4, 4)}"));

            CreateMap<LoginUsers, User>().ForMember(
                    dest => dest.Username,
                    opt => opt.MapFrom(src => $"{ src.UserName }"));

            CreateMap<ProcessCardData, ProcessData>();
        }

    }
}
