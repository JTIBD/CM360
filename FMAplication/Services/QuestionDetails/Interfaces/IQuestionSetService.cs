using FMAplication.Models.Questions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace FMAplication.Services.QuestionDetails.Interfaces
{
    public interface IQuestionSetService
    {
        #region Survey
        Task<IEnumerable<SurveyQuestionSetModel>> GetAllSurveyQuestionSetsWithCorrespondingQsCollectionAsync();
        //Task<IEnumerable<SurveyModel>> GetSurveysAsync();

        Task<IPagedList<SurveyQuestionSetModel>> GetPagedSurveysAsync(int pageNumber, int pageSize);

        Task<SurveyQuestionSetModel> GetQuestionSetAsync(int sId);
        //Task<SurveyModel> GetSurveyAsync(int id);

        Task<SurveyQuestionSetModel> CreateSurveyAndQsCollectionAsync(SurveyQuestionSetModel model);
        //Task<SurveyModel> CreateSurveyAsync(SurveyModel model);

        Task<SurveyQuestionSetModel> UpdateSurveyWithQsCollectionAsync(SurveyQuestionSetModel model);
        //Task<SurveyModel> UpdateSurveyAsync(SurveyModel model);

        Task<int> CascadeDeleteSurveyAndQsCollectionAsync(int surveyId);
        //Task<int> DeleteSurveyAsync(int id);

        Task<bool> IsQuestionSetNameExistAsync(string code, int id);
        #endregion

        #region Survey Question Collection
        Task<List<SurveyQuestionCollectionModel>> UpdateSQsCollectionAsync(SurveyQuestionMap model);
        Task<int> DeleteSurveyQsCollectionAsync(List<SurveyQuestionCollectionModel> id);
        Task<IEnumerable<SurveyQuestionCollectionModel>> GetQsCollectionBySurveyIdAsync(int surveyId);
        Task<IEnumerable<IGrouping<int, SurveyQuestionCollectionModel>>> GetAllQsCollectionGrpBySurveyIdAsync();
        Task<List<SurveyQuestionCollectionModel>> GetQuestionsBySurveyIdsAsync(List<int> ids);
        #endregion
    }
}
