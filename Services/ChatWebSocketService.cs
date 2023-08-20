using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using tcc_mypet_back.Data.Context;
using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Models;

namespace tcc_mypet_back.Services
{
    public class ChatWebSocketService
    {
        private readonly RequestDelegate _next;

        public ChatWebSocketService(RequestDelegate next)
        {
            _next = next;
        }

        public static class WebSocketManager
        {
            // Key é o UserID e Value é o WebSocket associado
            public static Dictionary<int, WebSocket> ActiveConnections = new Dictionary<int, WebSocket>();
        }
        private bool IsValid(ChatMessageDTO message)
        {
            // Exemplo básico de validação. Você pode expandir isso.
            return message != null 
                && message.User1Id > 0 
                && message.User2Id > 0 
                && (message.PetId.HasValue || message.ProductId.HasValue) 
                && (!string.IsNullOrEmpty(message.Text) || !string.IsNullOrEmpty(message.Image64));
        }


        public async Task Invoke(HttpContext context)
        {
            var dataContext = context.RequestServices.GetRequiredService<DataContext>();

            if (context.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await HandleWebSocket(context, webSocket, dataContext);
            }
            else
            {
                await _next.Invoke(context);
            }
        }

        private async Task HandleWebSocket(HttpContext context, WebSocket webSocket, DataContext dataContext)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            int userId = 0;  // Definindo fora do escopo condicional

            // Ler a mensagem inicial para identificar o UserId
            var initialMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var user = JsonSerializer.Deserialize<Dictionary<string, int>>(initialMessage);
            if (user != null && user.ContainsKey("UserId"))
            {
                userId = user["UserId"];
                WebSocketManager.ActiveConnections[userId] = webSocket;  // Registrar a conexão

                // Ir diretamente para a próxima iteração após o registro
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }


            while (!result.CloseStatus.HasValue)
            {
                var messageJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var chatMessageDto = JsonSerializer.Deserialize<ChatMessageDTO>(messageJson);

                if(IsValid(chatMessageDto))
                {
                    await SaveMessage(chatMessageDto, dataContext);

                    if (WebSocketManager.ActiveConnections.TryGetValue(chatMessageDto.User2Id, out var receiverWebSocket))
                    {
                        var responseData = Encoding.UTF8.GetBytes(messageJson); 
                        await receiverWebSocket.SendAsync(new ArraySegment<byte>(responseData), result.MessageType, result.EndOfMessage, CancellationToken.None);
                    }

                    // Enviar a mensagem também de volta ao remetente para que ele possa vê-la no chat.
                    if (WebSocketManager.ActiveConnections.TryGetValue(chatMessageDto.User1Id, out var senderWebSocket))
                    {
                        var responseData = Encoding.UTF8.GetBytes(messageJson); 
                        await senderWebSocket.SendAsync(new ArraySegment<byte>(responseData), result.MessageType, result.EndOfMessage, CancellationToken.None);
                    }
                }

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            if (userId != 0)  // Verificando se temos um userId válido antes de tentar remover
            {
                WebSocketManager.ActiveConnections.Remove(userId);
            }
        }

        private int GetUserIdFromRequest(HttpRequest request)
        {
            // Implementação exemplo: extrair ID do usuário do cabeçalho ou parâmetro da solicitação
            // Ajuste conforme necessário
            return int.Parse(request.Headers["UserId"].ToString());
        }
        public async Task SaveMessage(ChatMessageDTO message, DataContext dataContext)
        {
            try
            {
                // Identifique a sessão de chat existente ou crie uma nova
                if (message.PetId.HasValue)
                {
                    var petChatSession = await dataContext.UserPetChatSessions
                        .FirstOrDefaultAsync(cs => cs.Id == message.SessionId);

                    if (petChatSession == null)
                    {
                        petChatSession = new UserPetChatSession
                        {
                            User1Id = message.User1Id,
                            User2Id = message.User1Id,
                            PetId = message.PetId.Value,
                            CreatedAt = DateTime.Now
                        };
                        dataContext.UserPetChatSessions.Add(petChatSession);
                        await dataContext.SaveChangesAsync();  // Salve a sessão antes de usá-la
                    }

                    var petChatMessage = new UserPetChat
                    {
                        UserPetChatSessionId = petChatSession.Id,
                        SenderUserId = message.User1Id,
                        Text = message.Text,
                        Image64 = message.Image64
                    };
                    dataContext.UserPetChats.Add(petChatMessage);
                }
                else if (message.ProductId.HasValue)
                {
                    var productChatSession = await dataContext.UserProductChatSessions
                        .FirstOrDefaultAsync(cs => cs.Id == message.SessionId);

                    if (productChatSession == null)
                    {
                        productChatSession = new UserProductChatSession
                        {
                            User1Id = message.User1Id,
                            User2Id = message.User2Id,
                            ProductId = message.ProductId.Value,
                            CreatedAt = DateTime.Now
                        };
                        dataContext.UserProductChatSessions.Add(productChatSession);
                        await dataContext.SaveChangesAsync();  // Salve a sessão antes de usá-la
                    }

                    var productChatMessage = new UserProductChat
                    {
                        UserProductChatSessionId = productChatSession.Id,
                        SenderUserId = message.User1Id,
                        Text = message.Text,
                        Image64 = message.Image64
                    };
                    dataContext.UserProductChats.Add(productChatMessage);
                }

                await dataContext.SaveChangesAsync();
            }
            catch (System.Exception error)
            {
                // Aqui você pode lidar com exceções específicas se necessário
                // Por exemplo, logar o erro, retornar uma mensagem amigável, etc.
                throw;
            }
        }
    }
}
