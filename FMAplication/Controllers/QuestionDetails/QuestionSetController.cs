using FMAplication.Controllers.Common;
using FMAplication.Filters;
using FMAplication.Models.Questions;
using FMAplication.Services.QuestionDetails.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FMAplication.Controllers.QuestionDetails
{
    [ApiController]
    [JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class QuestionSetController : BaseController
    {
        private readonly ILogger<QuestionController> _logger;
        private readonly IQuestionSetService _questionSet;

        public QuestionSetController(ILogger<QuestionController> logger, IQuestionSetService questionSet)
        {
            _logger = logger;
            _questionSet = questionSet;
        }

        #region Survey
        /// <summary>
        /// Return a list of Survey Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet()]
        [Route("get-surveys")]
        public async Task<IActionResult> GetSurveys()
        {
            try
            {
                var result = await _questionSet.GetAllSurveyQuestionSetsWithCorrespondingQsCollectionAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// return a single survey object by surveyId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ApiResponse</returns>
        [HttpGet()]
        [Route("get-survey/{id}")]
        public async Task<IActionResult> GetQuestionSet(int id)
        {
            try
            {
                var result = await _questionSet.GetQuestionSetAsync(id);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }

        }

        /// <summary>
        /// create Survey object and Return a single of Survey Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[HttpPost()]
        [AcceptVerbs("POST")]
        [Route("create-survey")]
        public async Task<IActionResult> CreateSurvey([FromBody] SurveyQuestionSetModel model)
        {
            try
            {
                var isExist = await _questionSet.IsQuestionSetNameExistAsync(model.Name, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Name), "A survey with similar name already exist");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _questionSet.CreateSurveyAndQsCollectionAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }

        }

        /// <summary>
        /// update Survey object and Return a single of Survey Model objects
        /// </summary>
        /// <param name="model">SurveyModel</param>
        /// <returns></returns>
        //[HttpPut()]
        [AcceptVerbs("PUT")]
        [Route("update-survey")]
        public async Task<IActionResult> UpdateSurvey([FromBody] SurveyQuestionSetModel model)
        {
            var isExist = await _questionSet.IsQuestionSetNameExistAsync(model.Name, model.Id);
            if (isExist)
            {
                ModelState.AddModelError(nameof(model.Name), "A survey with similar name already exist");
            }
            if (!ModelState.IsValid)
            {
                return ValidationResult(ModelState);
            }
            else
            {
                var result = await _questionSet.UpdateSurveyWithQsCollectionAsync(model);
                return OkResult(result);
            }

        }

        /// <summary>
        /// delete a single survey object by surveyId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AcceptVerbs("DELETE")]
        [Route("delete-survey/{id}")]
        public async Task<IActionResult> DeleteSruvey(int id)
        {
            var result = await _questionSet.CascadeDeleteSurveyAndQsCollectionAsync(id);
            return OkResult(result);
        }
        #endregion
    }

}