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
using System.Threading.Tasks;

namespace CodeCampRESTfulApi.Controllers {

    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [ApiController]
    public class CampsController : ControllerBase {

        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkgenerator;

        public CampsController(ICampRepository repository, IMapper mapper, LinkGenerator linkGenerator) {
            _repository = repository;
            _mapper = mapper;
            _linkgenerator = linkGenerator;
        }

        // Get all camps
        [HttpGet]
        public async Task<ActionResult<CampModel[]>> Get(bool includeTalks = true) {
            try {
                var results = await _repository.GetAllCampsAsync(includeTalks);
                return _mapper.Map<CampModel[]>(results);
            } catch (Exception e) {

                // This is just an example to catch an error 
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // Get all speakers
        [HttpGet("speakers")]
        public async Task<ActionResult<SpeakerModel[]>> GetAllSpeakers() {
            try {
                var results = await _repository.GetAllSpeakersAsync();
                return _mapper.Map<SpeakerModel[]>(results);
            } catch (Exception e) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // Get all talks
        [HttpGet("talks")]
        public async Task<ActionResult<TalkModel[]>> GetAllTalks() {
            try {
                var results = await _repository.GetAllTalksAsync();
                return _mapper.Map<TalkModel[]>(results);
            } catch (Exception e) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // Get all speakers of a camp
        [HttpGet("{moniker}/speakers")]
        public async Task<ActionResult<SpeakerModel[]>> GetAllSpeakersOfACamp(string moniker) {
            try {
                var results = await _repository.GetSpeakersByMonikerAsync(moniker);
                return _mapper.Map<SpeakerModel[]>(results);
            } catch (Exception e) {

                // This is just an example to catch an error 
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // Get a camp v1.0 without talks
        [HttpGet("{moniker}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<CampModel>> Get(string moniker, bool includeTalks = false) {
            try {
                var result = await _repository.GetCampAsync(moniker, includeTalks);
                if (result != null)
                    return _mapper.Map<CampModel>(result);
                else
                    return NotFound();
            } catch (Exception e) {

                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        // Get a camp v1.1 with talks
        [HttpGet("{moniker}")]
        [MapToApiVersion("1.1")]
        public async Task<ActionResult<CampModel>> Get11(string moniker, bool includeTalks = true) {
            try {
                var result = await _repository.GetCampAsync(moniker, includeTalks);
                if (result != null)
                    return _mapper.Map<CampModel>(result);
                else
                    return NotFound();
            } catch (Exception e) {

                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        // Search for camps by eventDate ?querystring
        [HttpGet("search")]
        public async Task<ActionResult<CampModel[]>> SearchByDate(DateTime theDate, bool includeTalks = false) {
            try {
                var results = await _repository.GetAllCampsByEventDate(theDate, includeTalks);
                if (!results.Any())
                    return NotFound();
                return _mapper.Map<CampModel[]>(results);
            } catch (Exception e) {

                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        // Add a new camp
        [HttpPost]
        public async Task<ActionResult<CampModel>> Post(CampModel model) {
            try {
                // Check duplicates moniker
                var existing = await _repository.GetCampAsync(model.Moniker);
                if(existing != null) {
                    return BadRequest("Duplicate Moniker");
                }

                // generate a location ( Core 2.2 -> )
                var location = _linkgenerator.GetPathByAction("Get", 
                    "Camps",
                    new { moniker = model.Moniker });

                if (string.IsNullOrWhiteSpace(location)) {
                    return BadRequest("Could no use current moniker");
                }
                // create a new camp
                var camp = _mapper.Map<Camp>(model);
                _repository.Add(camp);
                if (await _repository.SaveChangesAsync()) {
                    return Created(location.ToString(), _mapper.Map<CampModel>(camp));
                }
            } catch (Exception e) {

                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return BadRequest();
        }

        // Update a camp
        [HttpPut("{moniker}")]
        public async Task<ActionResult<CampModel>> Put(string moniker, CampModel model) {
            try {
                var oldCamp = await _repository.GetCampAsync(moniker);
                if(oldCamp == null) {
                    return NotFound($"Could not find camp with moniker of {moniker}");
                }
                _mapper.Map(model, oldCamp);
                if (await _repository.SaveChangesAsync()) {
                    // I am returning the oldCamp but it is now the newCamp after the mapping update
                    return _mapper.Map<CampModel>(oldCamp);
                }
            } catch (Exception e) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return BadRequest();
        }

        // Delete a Camp
        [HttpDelete("{moniker}")]
        public async Task<IActionResult> Delete(string moniker) {
            try {
                var oldCamp = await _repository.GetCampAsync(moniker);
                if (oldCamp == null) {
                    return NotFound($"Could not find camp with moniker of {moniker}");
                }

                _repository.Delete(oldCamp);
                if(await _repository.SaveChangesAsync()) {
                    return Ok();
                }
            } catch (Exception e) {

                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return BadRequest();
        }
    }
}
