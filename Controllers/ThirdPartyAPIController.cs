using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIsDemo.Entities;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace WebAPIsDemo.Controllers
{
    [Authorize]
    [Route("api/read-external-data")]
    [ApiController]
    public class ThirdPartyAPIController : ControllerBase
    {
        

        [HttpGet]
        public List<Collection> Get(){
            
            List<Collection> collections = new List<Collection>();
            var userName = "username";
            var password = "password";
            var authToken = Encoding.ASCII.GetBytes($"{userName}:{password}");
            
            var base64Token = Convert.ToBase64String(authToken);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Token);

            var response = Task.Run(async()=> await client.GetAsync("Uri")).Result;
            var finalResult = response.Content.ReadAsStringAsync();
            var resultObj = JObject.Parse(finalResult.Result.ToString());
            collections.Add(new Collection
            {
                id = (int)resultObj.GetValue("id"),
                name = (string)resultObj.GetValue("FindingNumber")
            });
            return collections;
}

       

    }
}
