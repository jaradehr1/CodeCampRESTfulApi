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

namespace CodeCampRESTfulApi.Controllers
{
    [Route("api/camps/{moniker}/[controller]")]
    [ApiController]
    public class TalksController : ControllerBase
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public TalksController(ICampRepository repository, IMapper mapper, LinkGenerator linkGenerator) {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        // Get all the talks of a camp
        [HttpGet]
        public async Task<ActionResult<TalkModel[]>> Get(string moniker, bool includeSpeakers = true) {
            try {
                var talks = await _repository.GetTalksByMonikerAsync(moniker, includeSpeakers);
                return _mapper.Map<TalkModel[]>(talks);
            } catch (Exception e) {

                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // Get a talk by id in a camp
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TalkModel>> Get(string moniker, int id, bool includeSpeakers = true) {
            try {
                var talk = await _repository.GetTalkByMonikerAsync(moniker, id, includeSpeakers);
                if (talk == null) {
                    return NotFound("Couldn't find the talk");
                }
                return _mapper.Map<TalkModel>(talk);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // Search for a topic in a camp
        [HttpGet("topic/{subject}")]
        public async Task<ActionResult<TalkModel[]>> SearchByTopic(string moniker, string subject, bool includeSpeakers = true) {
            try {
                var talks = await _repository.GetTalksByTopicMonikerAsync(moniker, subject, includeSpeakers);
                if (talks.Length == 0) {
                    return NotFound("Couldn't find a talk with the subject: {subject}");
                }
                return _mapper.Map<TalkModel[]>(talks);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        
        // Add a talk under a camp
        [HttpPost]
        public async Task<ActionResult<TalkModel>> Post(string moniker, TalkModel model) {
            try {
                if (ModelState.IsValid) {
                    var camp = await _repository.GetCampAsync(moniker);
                    if (camp == null)
                        return BadRequest("Camp does not exist");

                    var talk = _mapper.Map<Talk>(model);
                    talk.Camp = camp;

                    // Check the speaker in the request body
                    if (model.Speaker == null) {
                        return BadRequest("Speaker ID is required");
                    }

                    // Check to see if the speaker in the database (speaker must be in the database first)
                    // the model cares about the speaker id only
                    var speaker = await _repository.GetSpeakerAsync(model.Speaker.SpeakerId);
                    if (speaker == null) {
                        // This means we're adding a new speaker to the talk
                        
                        // search by name
                        if(string.IsNullOrWhiteSpace(model.Speaker.FirstName) || string.IsNullOrWhiteSpace(model.Speaker.LastName)) {
                            return StatusCode(StatusCodes.Status400BadRequest, "First name and Last name of the speaker are required");
                        }

                        // Check if the speaker that we want to create is already in the DB
                        var speakerResult = await _repository.GetSpeakerByNameAsync(model.Speaker.FirstName,
                            model.Speaker.LastName, model.Speaker.MiddleName);

                        if (speakerResult == null) {
                            // create a new speaker
                            var newSpeaker = _mapper.Map<Speaker>(model.Speaker);
                            _repository.Add(newSpeaker);
                            await _repository.SaveChangesAsync();
                            speaker = newSpeaker;
                        } else {
                            return StatusCode(StatusCodes.Status400BadRequest, "Failed to add a speaker");
                        }
                    }
                    talk.Speaker = speaker;

                    _repository.Add(talk);
                    if (await _repository.SaveChangesAsync()) {
                        var url = _linkGenerator.GetPathByAction("Get",
                            "talks",
                            values: new { moniker, id = talk.TalkId });

                        return Created(url, _mapper.Map<TalkModel>(talk));
                    } else {
                        return BadRequest("Failed to save a new Talk");
                    }
                } else {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState);
                }
            } catch (Exception e) {

                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // Update a talk under a camp
        [HttpPut("{id:int}")]
        public async Task<ActionResult<TalkModel>> Put(string moniker, int id, TalkModel model) {
            try {
                var talk = await _repository.GetTalkByMonikerAsync(moniker, id, true);
                if (talk == null) {
                    return NotFound("Couldn't find the talk");
                }

                _mapper.Map(model, talk);

                if (model.Speaker != null) {
                    var speaker = await _repository.GetSpeakerAsync(model.Speaker.SpeakerId);
                    if(speaker != null) {
                        talk.Speaker = speaker;
                    }
                }

                if(await _repository.SaveChangesAsync()) {
                    return _mapper.Map<TalkModel>(talk);
                } else {
                    return BadRequest("Failed to do update database");
                }
            } catch (Exception e) {

                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // Delete a talk under a camp
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(string moniker, int id) {
            try {
                var talk = await _repository.GetTalkByMonikerAsync(moniker, id, true);
                if(talk == null) {
                    return NotFound("Couldn't find the talk");
                } else {
                    _repository.Delete(talk);
                }

                if (await _repository.SaveChangesAsync()) {
                    return Ok("Talk has been deleted successfully!");
                } else {
                    return BadRequest("Failed to do delete talk");

                }
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
