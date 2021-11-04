using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ZaloDotNetSDK;

namespace webhook_zalo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZaloController : ControllerBase
    {
        ZaloClient client = new ZaloClient("VZfZReE8uJf_F69fX87WCtmoP6I2c9TY7NHpDEU1hnvpH5CDZfIg5mneA1JvdymxSbKgB8Res3eaArzjslgsEa4wLIpqXgTsQKP3K_6bmsjgLn0nyeBmPN8nU6N9sQqnCMzA8gw1jYKLTr4qjv6Z9q9JU3ssvPq-3nby5OxdeqGLBq9_hzE-U50JF6gfzBmw201X8BBguY4gLn0sh9VI6J9mKHARjuuZ66Hf4B6whG0a2tKrZgAi9o5oKJp0Zj8GEsyc8iA1yZTxGH00_uV4EtbJB3Voaj0SRqDC9CMbjHPJ63T4p8InOe6GvZq");
        [HttpPost]
        [Route("webhook")]
        public async Task<IActionResult> Webhook([FromBody] object request)
        {
            var event_name = GetPropertyString(request, "event_name");
            var user_id = GetPropertyString(GetPropertyObject(request, "sender"), "id");
            var text = GetPropertyString(GetPropertyObject(request, "message"), "text");
            if (event_name == "user_send_text")
            {
                JObject result1 = client.getProfileOfFollower(user_id);
                var profile = GetPropertyObject(result1, "data");
                var display_name = GetPropertyString(profile, "display_name");
                JObject result = client.sendTextMessageToUserId(user_id, display_name + ", Bạn đã gửi tin nhắn có nội dung : " + text);
            }
            return Ok("Send message is successful !");
        }
        public static string GetPropertyString(object request, string strProperty)
        {
            var body = JsonDocument.Parse(request.ToString());
            var root = body.RootElement;
            return root.GetProperty(strProperty).GetString();
        }
        public static object GetPropertyObject(object request, string strProperty)
        {
            var body = JsonDocument.Parse(request.ToString());
            var root = body.RootElement;
            return root.GetProperty(strProperty);
        }
    }
}
