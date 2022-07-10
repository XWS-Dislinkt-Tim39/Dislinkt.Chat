using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcAddNotificationService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OpenTracing;
using OpenTracing.Mock;
using Public_Chat.Data;
using Public_Chat.Domain;
using Public_Chat.Hubs;
using Public_Chat.Interfaces.Repositories;
using Public_Chat.ReqDto;

namespace Public_Chat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IMessageRepository _messageRepository;
        private readonly ITracer _tracer;
        public ChatController(IHubContext<ChatHub> hubContext,IMessageRepository messageRepository, ITracer tracer)
        {
            _hubContext = hubContext;
            _messageRepository = messageRepository;
            _tracer = tracer;
        }

       

        /* [Route("send")]                                           //path looks like this: https://localhost:44379/api/chat/send
         [HttpPost]
         public IActionResult SendRequest([FromBody] MessageDto msg)
         {
             _hubContext.Clients.All.SendAsync("ReceiveOne", msg.User, msg.Text);
             return Ok();
         }*/

        [Route("send")]                                           //path looks like this: https://localhost:44379/api/chat/send
        [HttpPost]
        public IActionResult SendRequest([FromBody] MessageDto msg)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            _hubContext.Clients.Group(msg.Reciever).SendAsync("Receive", msg.User, msg.Text);
            return Ok();
        }

        [HttpGet]
        [Route("/get-by-from-to")]
        public async Task<Message> GetByFromAndTo(Guid from,Guid to)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            var result = await _messageRepository.GetBySenderAndReciever(from,to);

            return result;
        }

        [HttpPost]
        [Route("/create-chat")]
        public async Task CreateChat([FromBody] MessageData chat)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            await _messageRepository.CreateChatAsync(chat);

        }


        [HttpPost]
        [Route("/delete-chat")]
        public async Task DeleteChat([FromBody] MessageData chat)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            await _messageRepository.DeleteChat(chat.From,chat.To);

        }
        [HttpPost]
        [Route("/add-new-message")]
        public async Task<bool> AddNewInterest(NewMessageData newMessage)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            var existingChat = await _messageRepository.GetBySenderAndReciever(newMessage.To,newMessage.Sender);

            if (existingChat == null) return false;

            var updatedMessages = existingChat.Messages.Append(new Domain.MessageInfo(Guid.NewGuid(),newMessage.Sender,newMessage.Text,newMessage.Time)).ToArray();

            await _messageRepository.AddMessage(new Message(existingChat.Id, existingChat.From,existingChat.To, updatedMessages));


            var channel = GrpcChannel.ForAddress("https://localhost:5002/");
            var client = new addNotificationGreeter.addNotificationGreeterClient(channel);

         
                var reply = client.addNotification(new NotificationRequest { UserId = newMessage.To.ToString(), From = newMessage.Sender.ToString(), Type = "Message", Seen = false });

                if (!reply.Successful)
                {
                    Debug.WriteLine("Doslo je do greske prilikom kreiranja notifikacija za usera");
                    return false;
                }

                Debug.WriteLine("Uspesno prosledjen na registraciju u notifikacijama -- " + reply.Message);
          

            return true;

        }
    }
}
