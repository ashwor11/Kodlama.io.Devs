using Application.Features.ProgrammingLanguages.Commands.CreateProgrammingLanguage;
using Application.Features.ProgrammingLanguages.Dtos;
using Core.ElasticSearch;
using Core.ElasticSearch.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElasticSearchTestController : BaseController
    {
        private readonly IElasticSearch _elasticSearch;

        public ElasticSearchTestController(IElasticSearch elasticSearch)
        {
            _elasticSearch = elasticSearch;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IElasticSearchResult result = await _elasticSearch.CreateNewIndexAsync(new IndexModel
            {
                IndexName = "programminglanguages",
                AliasName = "aprogramminglanguages",
                NumberOfReplicas = 1,
                NumberOfShards = 1
            });

            

            ElasticSearchInsertUpdateModel model = new()
            {
                IndexName = "programminglanguages",
                Item = new CreateProgrammingLanguageCommand
                { Name = "C" }
            };
            ElasticSearchInsertUpdateModel model1 = new()
            {
                IndexName = "programminglanguages",
                Item = new CreateProgrammingLanguageCommand
                { Name = "C++" }
            };
            ElasticSearchInsertUpdateModel model2 = new()
            {
                IndexName = "programminglanguages",
                Item = new CreateProgrammingLanguageCommand
                { Name = "C#" }
            };

            IElasticSearchResult result2 = await _elasticSearch.InsertAsync(model);
            IElasticSearchResult result2X = await _elasticSearch.InsertAsync(model2);
            IElasticSearchResult result2XD = await _elasticSearch.InsertAsync(model1);


            IEnumerable<IndexName> result3 = _elasticSearch.GetIndexList().Keys;

            List<ElasticSearchGetModel<ProgrammingLanguageListDto>> result4 = await
                                                                    _elasticSearch.GetSearchByField<ProgrammingLanguageListDto>(
                                                                        new SearchByFieldParameters
                                                                        {
                                                                            IndexName = "programminglanguages",
                                                                            FieldName = "Name",
                                                                            Value = "C"
                                                                        });

            return Ok(result4);
        }
    }
}
