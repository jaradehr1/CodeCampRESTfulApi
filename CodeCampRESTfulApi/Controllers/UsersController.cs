using AutoMapper;
using CodeCampRESTfulApi.Data.Entities;
using CodeCampRESTfulApi.Data.Repositories;
using CodeCampRESTfulApi.Helpers;
using CodeCampRESTfulApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CodeCampRESTfulApi.Controllers {

    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase {
        private IUserRepository _userRepository;
        private IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        private readonly AppSettings _appSettings;


        public UsersController(IUserRepository userRepository, IMapper mapper, IOptions<AppSettings> appSettings, LinkGenerator linkGenerator) {
            _userRepository = userRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateModel model) {
            var user = _userRepository.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(UserModel model) {
            if (ModelState.IsValid) {
                // map model to entity
                var user = _mapper.Map<User>(model);

                try {
                    // save 
                    var RegisteredUser = _userRepository.Create(user, model.Password);
                    if (RegisteredUser != null) {
                        var url = _linkGenerator.GetPathByAction("GetById",
                            "users",
                            values: new {  });
                        return Created(url, _mapper.Map<UserModel>(user));
                    } else {
                        return BadRequest("Failed to register!");
                    }
                    
                } catch (AppException ex) {
                    // return error message if there was an exception
                    return BadRequest(new { message = ex.Message });
                }
            } else {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            
        }

        [HttpGet]
        public IActionResult GetAll() {
            var users = _userRepository.GetAll();
            return Ok(_mapper.Map<IList<UserModel>>(users));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id) {
            var user = _userRepository.GetById(id);
            return (Ok(_mapper.Map<UserModel>(user)));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]UserModel model) {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = id;

            try {
                // save 
                _userRepository.Update(user, model.Password);
                return Ok();
            } catch (AppException ex) {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            _userRepository.Delete(id);
            return Ok();
        }
    }
}
