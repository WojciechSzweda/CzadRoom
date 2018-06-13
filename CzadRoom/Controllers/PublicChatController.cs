using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CzadRoom.Controllers {
    public class PublicChatController : Controller {
        public async Task<IActionResult> Index() {
            var Nickname = await NicknameMiddleware();
            return Json(Nickname);
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
                using (var httpResponse = await httpClient.PostAsync(url,new StringContent(postContent,Encoding.UTF8, "application/json"))) {
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