using Application.Features.ProgrammingLanguages.Commands.CreateProgrammingLanguage;
using Application.Features.ProgrammingLanguages.Models;
using Core.ElasticSearch;
using Core.ElasticSearch.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElasticTestController : BaseController
    {
        private readonly IElasticSearch _elasticSearch;

        public ElasticTestController(IElasticSearch elasticSearch)
        {
            _elasticSearch = elasticSearch;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IElasticSearchResult result = await _elasticSearch.CreateNewIndexAsync(new IndexModel
            {
                IndexName = "models",
                AliasName = "amodels",
                NumberOfReplicas = 1,
                NumberOfShards = 1
            });

            ElasticSearchInsertUpdateModel model = new()
            {
                IndexName = "models",
                Item = new CreateProgrammingLanguageCommand
                { Name = "Fortran" }
            };

            IElasticSearchResult result2 = await _elasticSearch.InsertAsync(model);

            IEnumerable<IndexName> result3 = _elasticSearch.GetIndexList().Keys;

            List<ElasticSearchGetModel<ProgrammingLanguageListModel>> result4 = await
                                                                    _elasticSearch.GetSearchByField<ProgrammingLanguageListModel>(
                                                                        new SearchByFieldParameters
                                                                        {
                                                                            IndexName = "programminglanguages",
                                                                            FieldName = "Name",
                                                                            Value = "C#"
                                                                        });

            return Ok(result4);
        }

    }
}
