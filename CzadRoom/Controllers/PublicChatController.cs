using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CzadRoom.Models;
using CzadRoom.Services.Interfaces;
using CzadRoom.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CzadRoom.Controllers {
    public class PublicChatController : Controller {
        private readonly IPublicRoomService _publicRoomService;
        private readonly IMapper _mapper;
        private readonly IConnectionService _connectionService;

        public PublicChatController(IPublicRoomService publicRoomService, IMapper mapper, IConnectionService connectionService) {
            _publicRoomService = publicRoomService;
            _mapper = mapper;
            _connectionService = connectionService;
        }

        public async Task<IActionResult> Index() {
            await NicknameMiddleware();
            var rooms = await _publicRoomService.GetAll();
            var roomsVM = rooms.Select(x => _mapper.Map<PublicRoom, PublicRoomViewModel>(x, opt =>
            opt.AfterMap((src, dest) => dest.ClientCount = _connectionService.ConnectedUsersCount(src.ID))));
            return View(roomsVM);
        }


        public async Task<IActionResult> Room(string roomId) {
            await NicknameMiddleware();
            var roomDB = await _publicRoomService.GetRoom(roomId);
            var room = _mapper.Map<PublicRoom, PublicRoomViewModel>(roomDB, opt =>
             opt.AfterMap((src, dest) =>
                 dest.ClientsName = _connectionService.ConnectedUsersID(roomId)));
            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> GetUsername() {
            return Json(await NicknameMiddleware());
        }

        private async Task<string> NicknameMiddleware() {
            const string sessionKey = "Nickname";
            string Nickname;
            var value = HttpContext.Session.GetString(sessionKey);
            if (string.IsNullOrEmpty(value)) {
                Nickname = await GetRandomNickname();
                HttpContext.Session.SetString(sessionKey, JsonConvert.SerializeObject(Nickname));
            }
            else {
                Nickname = JsonConvert.DeserializeObject<string>(value);
            }
            return Nickname;
        }

        class NicknameResponse {
            public bool Success { get; set; }
            public string Nickname { get; set; }
        }


        private async Task<string> GetRandomNickname() {
            var url = "https://api.codetunnel.net/random-nick";
            var postContent = JsonConvert.SerializeObject(new { sizeLimit = 15 });
            using (var httpClient = new HttpClient()) {
                using (var httpResponse = await httpClient.PostAsync(url, new StringContent(postContent, Encoding.UTF8, "application/json"))) {
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK) {
                        var response = JsonConvert.DeserializeObject<NicknameResponse>(await httpResponse.Content.ReadAsStringAsync());
                        if (response.Success)
                            return response.Nickname;
                    }
                }
            }
            return string.Empty;
        }
    }
}