using API.Extensions;
using AutoMapper;
using Contracts;
using Entities.DTOs.Account;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public AccountController(IRepositoryWrapper repo, IMapper mapper)
        {
            _repository = repo;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AccountDto>), 200)]
        public IActionResult GetAllAccounts([FromQuery] AccountParameters accountParameters)
        {
            try
            {
                var accounts = _repository.AccountRepository.GetAllAccounts(accountParameters);

                Response.AddPaginationHeader(accounts);

                var accountResult = _mapper.Map<IEnumerable<AccountDto>>(accounts);
                return Ok(accountResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}", Name = "accountById")]
        public IActionResult GetAccountById(Guid id)
        {
            try
            {
                var account = _repository.AccountRepository.GetAccountById(id);

                if (account == null)
                {
                    return NotFound();
                }

                var accountResult = _mapper.Map<AccountDto>(account);
                return Ok(accountResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public IActionResult CreateAcount([FromBody] AccountForCreationDto account)
        {
            try
            {
                if (account == null)
                {
                    return BadRequest("Account is empty");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("");
                }

                var accountEntity = _mapper.Map<Account>(account);

                _repository.AccountRepository.CreateAccount(accountEntity);
                _repository.Save();

                var createdAccount = _mapper.Map<AccountDto>(accountEntity);

                return CreatedAtRoute("accountById", new { id = createdAccount.AccountId }, createdAccount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
