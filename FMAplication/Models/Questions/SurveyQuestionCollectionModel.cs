using AutoMapper;
using FMAplication.Domain.Questions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Questions
{
    public class SurveyQuestionCollectionModel
    {
        public int Id { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Required]
        public int SurveyId { get; set; }

        public bool IsActive { get; set; }
        public QuestionModel Question { get; set; }
    }

    public class SurveyQuestionMap
    {
        public List<int> QuestionsId { get; set; }
        public int SurveyId { get; set; }
    }

    public static class SurveyQuestionCollectionExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<SurveyQuestionCollection, SurveyQuestionCollectionModel>();
            cfg.CreateMap<Question, QuestionModel>();            
        }).CreateMapper();

        public static SurveyQuestionCollectionModel MapToModel(this SurveyQuestionCollection source)
        {
            var result = Mapper.Map<SurveyQuestionCollectionModel>(source);
            return result;
        }

        public static List<SurveyQuestionCollectionModel> MapToModel(this IEnumerable<SurveyQuestionCollection> source)
        {
            var result = Mapper.Map<List<SurveyQuestionCollectionModel>>(source);
            return result;
        }

    }

}
