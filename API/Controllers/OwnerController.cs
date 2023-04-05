using AutoMapper;
using Contracts;
using Entities.DTO;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers;

[Route("api/owner")]
[ApiController]
public class OwnerController : ControllerBase
{
    private readonly IRepositoryWrapper _repository;
    private readonly IMapper _mapper;
    public OwnerController(IRepositoryWrapper repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    [HttpGet]
    public IActionResult GetAllOwners([FromQuery] OwnerParameters ownerParameters)
    {
        try
        {
            var owners = _repository.OwnerRepository.GetAllOwners(ownerParameters);

            var metadata = new
            {
                owners.TotalCount,
                owners.PageSize,
                owners.CurrentPage,
                owners.TotalPages,
                owners.HasNext,
                owners.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var OwnersResult = _mapper.Map<IEnumerable<OwnerDto>>(owners);
            return Ok(OwnersResult);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    [HttpGet("{id}", Name = "OwnerById")]
    public IActionResult GetOwnerById(Guid id)
    {
        try
        {
            var owner = _repository.OwnerRepository.GetOwnerById(id);
            if (owner == null)
            {
                return NotFound();
            }

            var ownerResult = _mapper.Map<OwnerDto>(owner);
            return Ok(ownerResult);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    [HttpGet("{id}/account")]
    public IActionResult GetOwnerWithDetails(Guid id)
    {
        try
        {
            var owner = _repository.OwnerRepository.GetOwnerWithDetails(id);
            if (owner == null)
            {
                return NotFound();
            }

            var ownerResult = (_mapper.Map<OwnerDto>(owner));
            return Ok(ownerResult);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    [HttpPost]
    public IActionResult CreateOwner([FromBody] OwnerForCreationDto owner)
    {
        try
        {
            if (owner == null)
            {
                return BadRequest("Owner object is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model object");
            }
            var ownerEntity = _mapper.Map<Owner>(owner);
            _repository.OwnerRepository.CreateOwner(ownerEntity);
            _repository.Save();

            var createdOwner = _mapper.Map<OwnerDto>(ownerEntity);

            return CreatedAtRoute("OwnerById", new { id = createdOwner.OwnerId }, createdOwner);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    public IActionResult UpdateOwner(Guid id, [FromBody] OwnerForUpdateDto owner)
    {
        try
        {
            if (owner == null)
            {
                return BadRequest("Owner object is null");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model object");
            }

            var ownerEntity = _repository.OwnerRepository.GetOwnerById(id);
            if (ownerEntity == null)
            {
                return NotFound();
            }
            _mapper.Map(owner, ownerEntity);
            _repository.OwnerRepository.UpdateOwner(ownerEntity);
            _repository.Save();

            return NoContent();

        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete]
    public IActionResult DeleteOwner(Guid id)
    {
        try
        {
            var owner = _repository.OwnerRepository.GetOwnerById(id);
            if (owner == null)
            {
                return NotFound();
            }
            if (_repository.AccountRepository.AccountsByOwner(id).Any())
            {
                return BadRequest("Can not delete owner. It has related accounts. Delete those accounts first");
            }
            _repository.OwnerRepository.DeleteOwner(owner);
            _repository.Save();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
