using FMAplication.common;
using FMAplication.Controllers.Common;
using FMAplication.Enumerations;
using FMAplication.Filters;
using FMAplication.Models.Common;
using FMAplication.Models.Survey;
using FMAplication.Services.Surveys.interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Controllers.Survey
{
    [ApiController]
    [JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class SurveyController:BaseController
    {
        private readonly ISurveyService _survey;

        public SurveyController(ISurveyService survey)
        {
            _survey = survey;
        }

        [HttpPost]
        public async Task<ActionResult<List<SurveyModel>>> CreateSurvey(CustomObject<List<SurveyModel>> payload)
        {          
            var surveys = await _survey.Create(payload.Data);
            return Ok(surveys);          
        }

        [HttpPut]
        public async Task<ActionResult<List<SurveyModel>>> UpdateSurvey(SurveyModel payload)
        {
            if (! await _survey.IsSurveyActive(payload)) throw new ApplicationException("Survey Set Up is Already InActive");

            SurveyModel survey = await _survey.UpdateSurvey(payload);
            return Ok(survey);                        
        }

        [JwtAuthorize]
        [HttpGet("getSurveys")]
        public async Task<ActionResult<Pagination<SurveyModel>>> GetSurveys([FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] string search, [FromQuery] DateTime fromDateTime, [FromQuery] DateTime toDateTime,int salesPointId)
        {           
            fromDateTime = fromDateTime.ToUniversalTime();
            toDateTime = toDateTime.ToUniversalTime();
            Pagination<SurveyModel> data = await _survey.GetSurveys(pageSize, pageIndex, fromDateTime, toDateTime, search, salesPointId);
            return Ok(data);           
        }

        [JwtAuthorize]
        [HttpGet("getSurvey/{id}")]
        public async Task<ActionResult<SurveyModel>> GetSurvey([FromRoute] int id)
        {            
            SurveyModel data = await _survey.GetSurveyById(id);
            return Ok(data);         
        }

        [JwtAuthorize]
        [HttpPost("getExistingSurveys")]
        public async Task<ActionResult<List<SurveyModel>>> GetExistingSurveys(CustomObject<List<SurveyModel>> payload)
        {           
            List<SurveyModel> data = await _survey.GetExistingSurveys(payload.Data);
            return Ok(data);                      
        }
    }
}
