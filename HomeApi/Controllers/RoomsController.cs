using System;
using System.Threading.Tasks;
using AutoMapper;
using HomeApi.Contracts.Models.Devices;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers
{
    /// <summary>
    /// Контроллер комнат
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomRepository _repository;
        private IMapper _mapper;
        private IRoomRepository _rooms;

        public RoomsController(IRoomRepository repository, IMapper mapper, IRoomRepository rooms)
        {
            _repository = repository;
            _mapper = mapper;
            _rooms = rooms;
        }

        //TODO: Задание - добавить метод на получение всех существующих комнат

        /// <summary>
        /// Просмотр списка подключенных устройств
        /// </summary>
        [HttpGet]
        [Route("GetRooms")]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _rooms.GetRooms();

            var resp = new GetRoomsResponse
            {
                RoomAmount = rooms.Length,
                Rooms = _mapper.Map<Room[], RoomView[]>(rooms)
            };

            return StatusCode(200, resp);
        }

        /// <summary>
        /// Добавление комнаты
        /// </summary>
        [HttpPost] 
        [Route("AddRoom")] 
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            var existingRoom = await _repository.GetRoomByName(request.Name);
            if (existingRoom == null)
            {
                var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
                await _repository.AddRoom(newRoom);
                return StatusCode(201, $"Комната {request.Name} добавлена! Id = {newRoom.Id} ");
            }           

            return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
        }


        /// <summary>
        /// Обновление существующей комнаты
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(
            [FromRoute] Guid id,
            [FromBody] EditRoomRequest request)
        {
            
            var room = await _rooms.GetRoomById(id);
            if (room == null)
                return StatusCode(400, $"Ошибка: Комната с идентификатором {id} не существует.");

            await _rooms.UpdateRoom(room, new Data.Queries.UpdateRoomQuery(request.NewName)
            );

            return StatusCode(200, $"Комната обновлена! Имя - {room.Name}");
        }

    }
}