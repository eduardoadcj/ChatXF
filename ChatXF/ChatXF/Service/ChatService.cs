using ChatXF.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ChatXF.Service {
    public class ChatService {

        private const string URL = "http://ws.spacedu.com.br/xf2018/rest/api";
        private HttpClient _HttpClient;

        public ChatService() {
            _HttpClient = new HttpClient();
        }

        public Usuario GetUsuario(Usuario usuario) {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("nome", usuario.nome),
                new KeyValuePair<string, string>("password", usuario.password)
            });
            HttpResponseMessage response = _HttpClient.PostAsync(URL + "/usuario", content)
                .GetAwaiter().GetResult();
            if(response.StatusCode == HttpStatusCode.OK) {
                var contentResponse = response.Content.ReadAsStringAsync()
                    .GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<Usuario>(contentResponse);
            }
            return null;
        }

        public List<Chat> GetChats() {
            HttpResponseMessage response = _HttpClient.GetAsync(URL + "/chats")
                .GetAwaiter().GetResult();
            if(response.StatusCode == HttpStatusCode.OK) {
                var responseContent = response.Content.ReadAsStringAsync()
                    .GetAwaiter().GetResult();
                if(responseContent.Length > 2) {
                    return JsonConvert.DeserializeObject<List<Chat>>(responseContent);
                } else {
                    return null;
                }
            }
            return new List<Chat>();
        }

        public bool InsertChat(Chat chat) {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("nome", chat.nome)
            });
            HttpResponseMessage response = _HttpClient.PostAsync(URL + "/chat", content)
                .GetAwaiter().GetResult();
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

        public List<Mensagem> GetMensagensChat(Chat chat) {
            HttpResponseMessage response = _HttpClient.GetAsync(URL + "/chat/" + chat.id + "/msg")
                .GetAwaiter().GetResult();
            if(response.StatusCode == HttpStatusCode.OK) {
                var responseContent = response.Content.ReadAsStringAsync()
                    .GetAwaiter().GetResult();
                if(responseContent.Length > 2) {
                    return JsonConvert.DeserializeObject<List<Mensagem>>(responseContent);
                } else {
                    return null;
                }
            }
            return null;
        }

        public bool InsertMensagem(Mensagem mensagem) {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("mensagem", mensagem.mensagem),
                new KeyValuePair<string, string>("id_usuario", mensagem.id_usuario.ToString())
            });
            HttpResponseMessage response = _HttpClient.PostAsync(URL + "/chat/" + mensagem.id_chat + "/msg", content)
                .GetAwaiter().GetResult();
            return response.StatusCode == HttpStatusCode.OK;
        }

        public bool DeleteMensagem(Mensagem mensagem) {
            HttpResponseMessage response = _HttpClient.DeleteAsync(URL + "/chat/" + mensagem.id_chat + "/delete/" + mensagem.id)
                .GetAwaiter().GetResult();
            return response.StatusCode == HttpStatusCode.OK;
        }

    }
}
