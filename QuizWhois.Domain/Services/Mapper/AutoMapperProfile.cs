using System.Linq;
using AutoMapper;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Entity;

namespace QuizWhois.Domain.Services.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PackModelRequest, Pack>();
            CreateMap<Pack, PackModelResponse>();

            CreateMap<QuestionModelRequest, Question>()
                .ForMember(dest => dest.CorrectAnswers, opt => opt.MapFrom(src => src.CorrectAnswers.Select(x => new CorrectAnswer(x))));
            CreateMap<Question, QuestionModelResponse>()
                .ForMember(dest => dest.CorrectAnswers, opt => opt.MapFrom(src => src.CorrectAnswers.Select(x => x.AnswerText.ToString())));
        }
    }
}
