using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using URF.Core.EF.Trackable.Enums;

namespace URF.Core.Services
{
    public class HttpClientEx : IHttpClientEx
    {
        public async Task<string> Delete(string url, string id, string token = default, string xRealIp = default)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (xRealIp != null && xRealIp != string.Empty)
                    client.DefaultRequestHeaders.Add("x-real-ip", xRealIp);
                if (token != null && token.Trim() != string.Empty)
                {
                    if (token.StartsWith("Bearer"))
                    {
                        token = token.Replace("Bearer", string.Empty).Trim();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    else if (token.Contains(":"))
                    {
                        var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

                    }
                    else client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                }

                url = url.Contains(id)
                    ? url
                    : url + "/" + id;
                var response = await client.DeleteAsync(url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return jsonContent;
                }
                else
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return jsonContent;
                }
            }
        }
        public async Task<string> Delete(string url, object obj, string token = default, string xRealIp = default)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (xRealIp != null && xRealIp != string.Empty)
                    client.DefaultRequestHeaders.Add("x-real-ip", xRealIp);
                if (token != null && token.Trim() != string.Empty)
                {
                    if (token.StartsWith("Bearer"))
                    {
                        token = token.Replace("Bearer", string.Empty).Trim();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    else if (token.Contains(":"))
                    {
                        var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

                    }
                    else client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                }
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url),
                    Content = new StringContent(ToJson(obj), Encoding.UTF8, "application/json")
                };
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return jsonContent;
                }
                else
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return jsonContent;
                }
            }
        }

        public async Task<string> GetRequest(string url, Dictionary<string, object> keyValues, string token)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", token);
            if (keyValues != null && keyValues.Count > 0)
            {
                var content = new StringContent(JsonConvert.SerializeObject(keyValues), null, "application/json");
                request.Content = content;
            }
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> Get(string url, string token = default, string xRealIp = default)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (xRealIp != null && xRealIp != string.Empty)
                    client.DefaultRequestHeaders.Add("x-real-ip", xRealIp);
                if (token != null && token.Trim() != string.Empty)
                {
                    if (token.ToLower().Contains("[basic]"))
                    {
                        var items = token.Split("]_[", StringSplitOptions.RemoveEmptyEntries)
                            .Select(c => c.Trim(new[] { '[', ']', '_', ' ' }))
                            .Where(c => c.Trim() != string.Empty)
                            .Where(c => c.ToLower() != "basic")
                            .ToList();
                        if (items.Count >= 2)
                        {
                            var user = items[0].Split(":").LastOrDefault();
                            var password = items[1].Split(":").LastOrDefault();

                            var authenticationString = $"{user}:{password}";
                            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(authenticationString));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                        }
                    }
                    else
                    {
                        if (token.StartsWith("Bearer"))
                        {
                            token = token.Replace("Bearer", string.Empty).Trim();
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        }
                        else if (token.Contains(":"))
                        {
                            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

                        }
                        else client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                    }
                }
                try
                {
                    var response = await client.GetAsync(url).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonContent = await response.Content.ReadAsStringAsync();
                        return jsonContent;
                    }
                    else
                    {
                        var jsonContent = await response.Content.ReadAsStringAsync();
                        return jsonContent;
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        public async Task<string> Get(string url, string xApiKey, string xClientId, string xRealIp = default, string token = default)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-api-key", xApiKey);
                if (xRealIp != null && xRealIp != string.Empty)
                    client.DefaultRequestHeaders.Add("x-real-ip", xRealIp);
                client.DefaultRequestHeaders.Add("x-client-id", xClientId);
                if (token != null && token.Trim() != string.Empty)
                {
                    if (token.ToLower().Contains("[basic]"))
                    {
                        var items = token.Split("]_[", StringSplitOptions.RemoveEmptyEntries)
                            .Select(c => c.Trim(new[] { '[', ']', '_', ' ' }))
                            .Where(c => c.Trim() != string.Empty)
                            .Where(c => c.ToLower() != "basic")
                            .ToList();
                        if (items.Count >= 2)
                        {
                            var user = items[0].Split(":").LastOrDefault();
                            var password = items[0].Split(":").LastOrDefault();

                            var authenticationString = $"{user}:{password}";
                            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                        }
                    }
                    else
                    {
                        if (token.StartsWith("Bearer"))
                        {
                            token = token.Replace("Bearer", string.Empty).Trim();
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        }
                        else client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                    }
                }
                try
                {
                    var response = await client.GetAsync(url).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonContent = await response.Content.ReadAsStringAsync();
                        return jsonContent;
                    }
                    else
                    {
                        var jsonContent = await response.Content.ReadAsStringAsync();
                        return jsonContent;
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public async Task<Stream> GetFile(string url, string token = default, string xRealIp = default)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (xRealIp != null && xRealIp != string.Empty)
                    client.DefaultRequestHeaders.Add("x-real-ip", xRealIp);
                if (token != null && token.Trim() != string.Empty)
                {
                    if (token.ToLower().Contains("[basic]"))
                    {
                        var items = token.Split("]_[", StringSplitOptions.RemoveEmptyEntries)
                            .Select(c => c.Trim(new[] { '[', ']', '_', ' ' }))
                            .Where(c => c.Trim() != string.Empty)
                            .Where(c => c.ToLower() != "basic")
                            .ToList();
                        if (items.Count >= 2)
                        {
                            var user = items[0].Split(":").LastOrDefault();
                            var password = items[1].Split(":").LastOrDefault();

                            var authenticationString = $"{user}:{password}";
                            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(authenticationString));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                        }
                    }
                    else
                    {
                        if (token.StartsWith("Bearer"))
                        {
                            token = token.Replace("Bearer", string.Empty).Trim();
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        }
                        else if (token.Contains(":"))
                        {
                            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

                        }
                        else client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                    }
                }
                try
                {
                    var response = await client.GetAsync(url).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        var stramContent = await response.Content.ReadAsStreamAsync();
                        return stramContent;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        public async Task<string> Put(string url, object obj, string token = default, string xRealIp = default)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (xRealIp != null && xRealIp != string.Empty)
                    client.DefaultRequestHeaders.Add("x-real-ip", xRealIp);
                if (token != null && token.Trim() != string.Empty)
                {
                    if (token.StartsWith("Bearer"))
                    {
                        token = token.Replace("Bearer", string.Empty).Trim();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    else if (token.Contains(":"))
                    {
                        var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

                    }
                    else client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                }
                var content = new StringContent(ToJson(obj), Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url, content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return jsonContent;
                }
                else
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return jsonContent;
                }
            }
        }
        public async Task<string> Put(string url, object obj, string xApiKey, string xClientId, string xRealIp = default, string token = default)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-api-key", xApiKey);
                if (xRealIp != null && xRealIp != string.Empty)
                    client.DefaultRequestHeaders.Add("x-real-ip", xRealIp);
                client.DefaultRequestHeaders.Add("x-client-id", xClientId);
                if (token != null && token.Trim() != string.Empty)
                {
                    if (token.StartsWith("Bearer"))
                    {
                        token = token.Replace("Bearer", string.Empty).Trim();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    else if (token.Contains(":"))
                    {
                        var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

                    }
                    else client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                }
                var content = new StringContent(ToJson(obj), Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url, content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return jsonContent;
                }
                else
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return jsonContent;
                }
            }
        }

        public async Task<string> Patch(string url, object obj, string token = default, string xRealIp = default)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (xRealIp != null && xRealIp != string.Empty)
                    client.DefaultRequestHeaders.Add("x-real-ip", xRealIp);
                if (token != null && token.Trim() != string.Empty)
                {
                    if (token.StartsWith("Bearer"))
                    {
                        token = token.Replace("Bearer", string.Empty).Trim();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    else if (token.Contains(":"))
                    {
                        var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

                    }
                    else client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                }
                var content = new StringContent(ToJson(obj), Encoding.UTF8, "application/json");
                var response = await client.PatchAsync(url, content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return jsonContent;
                }
                else
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return jsonContent;
                }
            }
        }
        public async Task<string> Patch(string url, object obj, string xApiKey, string xClientId, string xRealIp = default, string token = default)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-api-key", xApiKey);
                if (xRealIp != null && xRealIp != string.Empty)
                    client.DefaultRequestHeaders.Add("x-real-ip", xRealIp);
                client.DefaultRequestHeaders.Add("x-client-id", xClientId);
                if (token != null && token.Trim() != string.Empty)
                {
                    if (token.StartsWith("Bearer"))
                    {
                        token = token.Replace("Bearer", string.Empty).Trim();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    else if (token.Contains(":"))
                    {
                        var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

                    }
                    else client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                }
                var content = new StringContent(ToJson(obj), Encoding.UTF8, "application/json");
                var response = await client.PatchAsync(url, content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return jsonContent;
                }
                else
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return jsonContent;
                }
            }
        }

        public async Task<string> Post(string url, object obj, string token = default, string xRealIp = default)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (xRealIp != null && xRealIp != string.Empty)
                    client.DefaultRequestHeaders.Add("x-real-ip", xRealIp);
                if (token != null && token.Trim() != string.Empty)
                {
                    if (token.StartsWith("Bearer"))
                    {
                        token = token.Replace("Bearer", string.Empty).Trim();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    else if (token.Contains(":"))
                    {
                        var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

                    }
                    else client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                }
                var content = new StringContent(ToJson(obj), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return jsonContent;
                }
                else
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return jsonContent;
                }
            }
        }
        public async Task<string> Post(string url, object obj, string xApiKey, string xClientId, string xRealIp = default, string token = default)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-api-key", xApiKey);
                if (xRealIp != null && xRealIp != string.Empty)
                    client.DefaultRequestHeaders.Add("x-real-ip", xRealIp);
                client.DefaultRequestHeaders.Add("x-client-id", xClientId);
                client.DefaultRequestHeaders.Add("client-key", xClientId);
                if (token != null && token.Trim() != string.Empty)
                {
                    if (token.StartsWith("Bearer"))
                    {
                        token = token.Replace("Bearer", string.Empty).Trim();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    else if (token.Contains(":"))
                    {
                        var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

                    }
                    else client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                }
                var content = new StringContent(ToJson(obj), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return jsonContent;
                }
                else
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return jsonContent;
                }
            }
        }
        public async Task<string> PostXForm(string url, Dictionary<string, string> obj, string token = default, string xRealIp = default)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                if (xRealIp != null && xRealIp != string.Empty)
                    client.DefaultRequestHeaders.Add("x-real-ip", xRealIp);
                if (xRealIp != null && xRealIp != string.Empty)
                    if (token != null && token.Trim() != string.Empty)
                    {
                        if (token.StartsWith("Bearer"))
                        {
                            token = token.Replace("Bearer", string.Empty).Trim();
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        }
                        else if (token.Contains(":"))
                        {
                            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

                        }
                        else client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                    }
                using (var content = new FormUrlEncodedContent(obj))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    HttpResponseMessage response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonContent = await response.Content.ReadAsStringAsync();
                        return jsonContent;
                    }
                }
            }
            return string.Empty;
        }

        public async Task<string> CallApi(string url, object obj, Dictionary<string, object> headers = null, MethodType type = MethodType.Get)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (headers != null && headers.Count > 0)
                {
                    foreach (var item in headers)
                    {
                        if (item.Key == "token")
                        {
                            var token = item.Value?.ToString();
                            if (token.StartsWith("Bearer"))
                            {
                                token = token.Replace("Bearer", string.Empty).Trim();
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            }
                            else if (token.Contains(":"))
                            {
                                var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

                            }
                            else client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                        }
                        else client.DefaultRequestHeaders.Add(item.Key, item.Value?.ToString());
                    }
                }
                var content = new StringContent(ToJson(obj), Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                switch (type)
                {
                    case MethodType.Get:
                        {
                            if (obj != null)
                            {
                                var request = new HttpRequestMessage
                                {
                                    Method = HttpMethod.Get,
                                    RequestUri = new Uri(url),
                                    Content = new StringContent(ToJson(obj), Encoding.UTF8, "application/json")
                                };
                                response = await client.SendAsync(request).ConfigureAwait(false);
                            }
                            else response = await client.GetAsync(url).ConfigureAwait(false);
                        }
                        break;
                    case MethodType.Put:
                        response = await client.PutAsync(url, content).ConfigureAwait(false);
                        break;
                    case MethodType.Post:
                        response = await client.PostAsync(url, content).ConfigureAwait(false);
                        break;
                    case MethodType.Delete:
                        {
                            if (obj != null)
                            {
                                var request = new HttpRequestMessage
                                {
                                    RequestUri = new Uri(url),
                                    Method = HttpMethod.Delete,
                                    Content = new StringContent(ToJson(obj), Encoding.UTF8, "application/json")
                                };
                                response = await client.SendAsync(request).ConfigureAwait(false);
                            }
                            else response = await client.DeleteAsync(url).ConfigureAwait(false);
                        }
                        break;
                    case MethodType.Patch:
                        response = await client.PatchAsync(url, content).ConfigureAwait(false);
                        break;
                    default:
                        break;
                }

                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonContent = await response.Content.ReadAsStringAsync();
                        return jsonContent;
                    }
                    else
                    {
                        var jsonContent = await response.Content.ReadAsStringAsync();
                        return jsonContent;
                    }
                } return string.Empty;
            }
        }

        private string ToJson(object obj)
        {
            return obj == null ? string.Empty : JsonConvert.SerializeObject(obj);
        }

    }

    public partial interface IHttpClientEx
    {
        public Task<string> Get(string url, string token = default, string xRealIp = default);
        public Task<Stream> GetFile(string url, string token = default, string xRealIp = default);
        public Task<string> Put(string url, object obj, string token = default, string xRealIp = default);
        public Task<string> Patch(string url, object obj, string token = default, string xRealIp = default);
        public Task<string> Post(string url, object obj, string token = default, string xRealIp = default);
        public Task<string> Delete(string url, string id, string token = default, string xRealIp = default);
    }
}
