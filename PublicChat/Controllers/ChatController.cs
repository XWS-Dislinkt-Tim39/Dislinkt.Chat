﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcAddNotificationService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        public ChatController(IHubContext<ChatHub> hubContext,IMessageRepository messageRepository)
        {
            _hubContext = hubContext;
            _messageRepository = messageRepository;
        }

   
     

        [HttpGet]
        [Route("/get-by-from-to")]
        public async Task<Message> GetByFromAndTo(Guid from,Guid to)
        {
            var result = await _messageRepository.GetBySenderAndReciever(from,to);

            return result;
        }

        [HttpPost]
        [Route("/create-chat")]
        public async Task CreateChat([FromBody] MessageData chat)
        {
            await _messageRepository.CreateChatAsync(chat);

        }


        [HttpPost]
        [Route("/delete-chat")]
        public async Task DeleteChat([FromBody] MessageData chat)
        {
            await _messageRepository.DeleteChat(chat.From,chat.To);

        }
        [HttpPost]
        [Route("/add-new-message")]
        public async Task<bool> AddNewInterest(NewMessageData newMessage)
        {
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
