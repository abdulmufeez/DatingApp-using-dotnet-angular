using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Extensions;
using DatingApp.Helpers;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DatingApp.Controllers
{
    [Authorize]
    public class MessagesController : BaseController
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessagesController(IUserProfileRepository userProfileRepository, 
            IMessageRepository messageRepository,
            IMapper mapper)
        {
            _mapper = mapper;
            _userProfileRepository = userProfileRepository;
            _messageRepository = messageRepository;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var currentUserProfileId = User.GetAppUserId(); 
            if (currentUserProfileId == createMessageDto.RecipientId) 
                return BadRequest("You cannot send messages to yourself");

            var sender = await _userProfileRepository.GetUserByAppIdAsync(currentUserProfileId);
            var recipient = await _userProfileRepository.GetUserByAppIdAsync(createMessageDto.RecipientId);

            if (recipient is null) return NotFound();

            var message = new Message 
            {
              Sender = sender,
              SenderId = sender.Id,
              SenderName = sender.KnownAs,
              Recipient = recipient,
              RecipientId = recipient.Id,
              RecipientName = recipient.KnownAs,              
              Content = createMessageDto.Content  
            };

            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery]MessageParams messageParams)
        {
            messageParams.UserId = (await _userProfileRepository.GetUserByAppIdAsync(User.GetAppUserId())).Id;
            var messages = await _messageRepository.GetMessagesForUser(messageParams);
            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);
            return messages;
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(int recipientId)
        {
            var currentUserProfileId = (await _userProfileRepository.GetUserByAppIdAsync(User.GetAppUserId())).Id;
            return Ok(await _messageRepository.GetMessagesThread(currentUserProfileId,recipientId));
        }
    }
}