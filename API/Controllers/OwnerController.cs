using API.Extensions;
using AutoMapper;
using Contracts;
using Entities.DTOs.Owner;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

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
    [ProducesResponseType(typeof(IEnumerable<OwnerDto>), 200)]
    public IActionResult GetAllOwners([FromQuery] OwnerParameters ownerParameters)
    {
        if (!ownerParameters.ValidYearRange)
        {
            return BadRequest("Max year of birth cannot be less than min year of birth");
        }

        try
        {
            var owners = _repository.OwnerRepository.GetAllOwners(ownerParameters);

            Response.AddPaginationHeader(owners);

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
