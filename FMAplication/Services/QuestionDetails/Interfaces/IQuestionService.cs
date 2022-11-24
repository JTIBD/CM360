using FMAplication.Domain.Questions;
using FMAplication.Models.Questions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace FMAplication.Services.QuestionDetails.Interfaces
{
    public interface IQuestionService
    {
        #region Question
        Task<IEnumerable<QuestionModel>> GetQuestionsAsync();
        Task<IEnumerable<QuestionModel>> GetActiveQuestionsAsync();
        Task<IPagedList<QuestionModel>> GetPagedQuestionsAsync(int pageNumber, int pageSize);
        Task<IEnumerable<QuestionModel>> GetQueryQuestionsAsync();
        Task<QuestionModel> GetQuestionAsync(int id);
        Task<QuestionModel> SaveAsync(QuestionModel model);
        Task<QuestionModel> CreateAsync(QuestionModel model);
        Task<QuestionModel> UpdateAsync(QuestionModel model, QuestionModel existingQuestion);
        Task<int> DeleteAsync(int id);
        Task<bool> IsQuestionExistAsync(string code, int id);
        #endregion

        #region QuestionOption
        Task<QuestionOptionModel> CreateQuestionOptionAsync(QuestionOptionModel model);
        
        Task<IEnumerable<QuestionOptionModel>> GetQuestionOptionsAsync(int questionId);
        
        Task<int> DeleteQuestionOptionAsync(int id);

        #endregion

    }
}
