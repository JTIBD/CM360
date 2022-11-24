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
    public class QuestionController : BaseController
    {
        private readonly ILogger<QuestionController> _logger;
        private readonly IQuestionService _question;

        public QuestionController(ILogger<QuestionController> logger, IQuestionService question)
        {
            _logger = logger;
            _question = question;
        }

        #region Question 
        /// <summary>
        /// Return a list of Question Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetQuestions()
        {
            try
            {
                var result = await _question.GetQuestionsAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Return a list of Active Question Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet()]
        [Route("get-active-questions")]
        public async Task<IActionResult> GetActiveQuestions()
        {
            try
            {
                var result = await _question.GetActiveQuestionsAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// return a single question object by questionId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestion(int id)
        {
            try
            {
                var result = await _question.GetQuestionAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        /// <summary>
        /// create or update Question object and Return a single of Question Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<IActionResult> SaveQuestion([FromBody]QuestionModel model)
        {
            try
            {
                var isExist = await _question.IsQuestionExistAsync(model.QuestionTitle, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.QuestionTitle), "Question Already Exist");
                }

                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _question.SaveAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        /// <summary>
        /// create Question object and Return a single of Question Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateQuestion([FromBody]QuestionModel model)
        {
            
            var isExist = await _question.IsQuestionExistAsync(model.QuestionTitle, model.Id);
            if (isExist)
            {
                ModelState.AddModelError(nameof(model.QuestionTitle), "Question Already Exist");
            }
            if (!ModelState.IsValid)
            {
                return ValidationResult(ModelState);
            }
            else
            {
                var result = await _question.CreateAsync(model);
                return OkResult(result);
            }
            
        }
        /// <summary>
        /// update Question object and Return a single of Question Model objects
        /// </summary>
        /// <param name="model">QuestionModel</param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateQuestion([FromBody]QuestionModel model)
        {            
           
            var isExist = await _question.IsQuestionExistAsync(model.QuestionTitle, model.Id);
            if (isExist)
            {
                ModelState.AddModelError(nameof(model.QuestionTitle), "Question Already Exist");
            }                
            
            if (!ModelState.IsValid)
            {
                return ValidationResult(ModelState);
            }
            else
            {
                var existingQuestion = await _question.GetQuestionAsync(model.Id);
                var result = await _question.UpdateAsync(model, existingQuestion);
                return OkResult(result);
            }
        }

        /// <summary>
        /// delete a single question object by questionId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var result = await _question.DeleteAsync(id);
            return OkResult(result);
        }
        #endregion
    }

}