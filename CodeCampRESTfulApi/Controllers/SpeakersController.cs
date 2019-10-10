using AutoMapper;
using CodeCampRESTfulApi.Data.Repository;
using CodeCampRESTfulApi.Data.Entities;
using CodeCampRESTfulApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeCampRESTfulApi.Controllers { 

    [Route("api/camps/{moniker}/talks/{id:int}/speaker")]
    [ApiController]
    public class SpeakersController : ControllerBase {

        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkgenerator;

        public SpeakersController(ICampRepository repository, IMapper mapper, LinkGenerator linkGenerator) {
            _repository = repository;
            _mapper = mapper;
            _linkgenerator = linkGenerator;
        }

        // Get the speaker of a talk in a camp
        [HttpGet]
        public async Task<ActionResult<SpeakerModel>> Get(string moniker, int id) {
            try {
                var talk = await _repository.GetTalkByMonikerAsync(moniker, id, true);
                if (talk == null) {
                    return NotFound("Couldn't find the talk");
                }
                if (talk.Speaker == null) {
                    return Ok("There is no speaker for this talk");
                }
                return _mapper.Map<SpeakerModel>(talk.Speaker);
            } catch (Exception e) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        
    }
}
