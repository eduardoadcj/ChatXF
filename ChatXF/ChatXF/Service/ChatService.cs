using ChatXF.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChatXF.Service {
    public class ChatService {

        private const string URL = "http://ws.spacedu.com.br/xf2018/rest/api";
        private HttpClient _HttpClient;

        public ChatService() {
            _HttpClient = new HttpClient();
        }

        public async Task<Usuario> GetUsuario(Usuario usuario) {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("nome", usuario.nome),
                new KeyValuePair<string, string>("password", usuario.password)
            });
            HttpResponseMessage response = await _HttpClient.PostAsync(URL + "/usuario", content);
            if(response.StatusCode == HttpStatusCode.OK) {
                var contentResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Usuario>(contentResponse);
            }
            return null;
        }

        public async Task<List<Chat>> GetChats() {
            HttpResponseMessage response = await _HttpClient.GetAsync(URL + "/chats");
            if(response.StatusCode == HttpStatusCode.OK) {
                var responseContent = await response.Content.ReadAsStringAsync();
                if(responseContent.Length > 2) {
                    return JsonConvert.DeserializeObject<List<Chat>>(responseContent);
                } else {
                    return null;
                }
            }
            return new List<Chat>();
        }

        public async Task<bool> InsertChat(Chat chat) {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("nome", chat.nome)
            });
            HttpResponseMessage response = await _HttpClient.PostAsync(URL + "/chat", content);
            return response.StatusCode == HttpStatusCode.OK;
        }

        public bool UpdateChat(Chat chat) {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("nome", chat.nome)
            });
            HttpResponseMessage response = _HttpClient.PutAsync(URL + "/chat" +chat.id, content)
                .GetAwaiter().GetResult();
            return response.StatusCode == HttpStatusCode.OK;
        }

        public bool DeleteChat(Chat chat) {
            HttpResponseMessage response = _HttpClient.DeleteAsync(URL + "/chat/delete/" + chat.id)
                .GetAwaiter().GetResult();
            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<List<Mensagem>> GetMensagensChat(Chat chat) {
            HttpResponseMessage response = await _HttpClient.GetAsync(URL + "/chat/" + chat.id + "/msg");
            if(response.StatusCode == HttpStatusCode.OK) {
                var responseContent = await response.Content.ReadAsStringAsync();
                if(responseContent.Length > 2) {
                    return JsonConvert.DeserializeObject<List<Mensagem>>(responseContent);
                } else {
                    return null;
                }
            }
            return new List<Mensagem>();
        }

        public async Task<bool> InsertMensagem(Mensagem mensagem) {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("mensagem", mensagem.mensagem),
                new KeyValuePair<string, string>("id_usuario", mensagem.id_usuario.ToString())
            });
            HttpResponseMessage response = await _HttpClient.PostAsync(URL + "/chat/" + mensagem.id_chat + "/msg", content);

            Debug.WriteLine("Retorno envio da mensagem: " + response.StatusCode);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public bool DeleteMensagem(Mensagem mensagem) {
            HttpResponseMessage response = _HttpClient.DeleteAsync(URL + "/chat/" + mensagem.id_chat + "/delete/" + mensagem.id)
                .GetAwaiter().GetResult();
            return response.StatusCode == HttpStatusCode.OK;
        }

    }
}
