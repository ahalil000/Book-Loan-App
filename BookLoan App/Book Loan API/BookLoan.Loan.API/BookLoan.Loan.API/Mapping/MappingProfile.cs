using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLoan.Models;


namespace BookLoan.Loan.API.Mapping
{
    public class BookLoanMappingProfile : Profile
    {
		public BookLoanMappingProfile()
		{
			CreateMap<BookStatusViewModel, LoanReportDto>()
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
				.ForMember(dest => dest.Borrower, opt => opt.MapFrom(src => src.Borrower))
				.ForMember(dest => dest.DateLoaned, opt => opt.MapFrom(src => src.DateLoaned))
				.ForMember(dest => dest.DateReturn, opt => opt.MapFrom(src => src.DateReturn))
				.ForMember(dest => dest.DateDue, opt => opt.MapFrom(src => src.DateDue));
		}
	}
}
