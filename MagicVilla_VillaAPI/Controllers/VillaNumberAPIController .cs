using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IMapper _mapper;
        private readonly IVillaNumberRepository _dbVillaNumber;
        //private readonly ILogger<VillaAPIController> _logger;
        //private readonly ILogging _logger;
        //public VillaAPIController(ILogging logger)
        //{
        //    _logger = logger;
        //}

        public VillaNumberAPIController(IVillaNumberRepository dbVillaNumber, IMapper mapper, IVillaRepository dbVilla)
        {
            _dbVillaNumber = dbVillaNumber;
            _mapper = mapper;
            this._response = new();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            //_logger.Log("Get all villas", "");
            try
            {
                IEnumerable<VillaNumber> villaList = await _dbVillaNumber.GetAllAsync();
                _response.IsSuccess = true;
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage
                        = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    //_logger.Log("Get Villa Error with Id " + id, "error");
                    return BadRequest(_response);
                }

                //var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
                //var villa = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);
                var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);
                if (villaNumber == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.IsSuccess = true;
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage
                        = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createDTO)
        {
            try
            {
                if (await _dbVillaNumber.GetAsync(u => u.VillaNo == createDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("CustomError", "VillaNumber already Exist!");
                    _response.IsSuccess = false;
                    return BadRequest(ModelState);
                }


                if (createDTO == null)
                {
                    _response.IsSuccess = false;
                    return BadRequest(createDTO);
                }

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(createDTO);

                //villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
                await _dbVillaNumber.CreateAsync(villaNumber);
                //VillaStore.villaList.AddAsync(villaDTO);
                _response.IsSuccess = true;
                _response.Result = _mapper.Map<VillaNumberCreateDTO>(villaNumber);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVillaNumber", new { id = villaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage
                        = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);
                if (villaNumber == null)
                {
                    _response.IsSuccess = false;
                    return NotFound();
                }
                await _dbVillaNumber.RemoveAsync(villaNumber);


                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage
                        = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                var villaNumber = _mapper.Map<VillaNumber>(updateDTO);

                await _dbVillaNumber.UpdateAsync(villaNumber);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);

            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage
                        = new List<string> { ex.ToString() };
            }
            return _response;

        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaNumberUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var villa = await _dbVillaNumber.GetAsync(u => u.VillaNo == id, tracked: false);

            if (villa == null)
            {
                return BadRequest();
            }

            var villaDTO = _mapper.Map<VillaNumberUpdateDTO>(villa);
            patchDTO.ApplyTo(villaDTO, ModelState);
            var model = _mapper.Map<VillaNumber>(villaDTO);

            await _dbVillaNumber.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();

        }

    }
}
