using AutoMapper;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Extensions;
using DatingApp.Helpers;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [Authorize]
    public class MessagesController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MessagesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var currentUserId = User.GetAppUserId();
            if (currentUserId == createMessageDto.RecipientId)
                return BadRequest("You cannot send messages to yourself");

            var sender = await _unitOfWork.UserProfileRepository.GetUserByAppIdAsync(currentUserId);
            var recipient = await _unitOfWork.UserProfileRepository.GetUserByIdAsync(createMessageDto.RecipientId);

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

            _unitOfWork.MessageRepository.AddMessage(message);
            if (await _unitOfWork.Complete()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.UserId = (await _unitOfWork.UserProfileRepository.GetUserByAppIdAsync(User.GetAppUserId())).Id;
            var messages = await _unitOfWork.MessageRepository.GetMessagesForUser(messageParams);
            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);
            return messages;
        }       

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id, [FromQuery] string deletedBoth)
        {
            var userProfileId = (await _unitOfWork.UserProfileRepository.GetUserByAppIdAsync(User.GetAppUserId())).Id;
            var message = await _unitOfWork.MessageRepository.GetMessage(id);

            if (message.SenderId != userProfileId && message.RecipientId != userProfileId)
                return Unauthorized();

            if (deletedBoth == "false" || deletedBoth == "False")
            {
                if (message.SenderId == userProfileId) message.SenderDeleted = true;

                if (message.RecipientId == userProfileId) message.RecipientDeleted = true;

                if (message.SenderDeleted && message.RecipientDeleted)
                    _unitOfWork.MessageRepository.DeleteMessage(message);
            }

            if (deletedBoth == "true" || deletedBoth == "True")
                _unitOfWork.MessageRepository.DeleteMessage(message);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Problem deleting message");
        }



        // this is now implememnt in signalr 
                
        // [HttpGet("thread/{recipientId}")]
        // public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(int recipientId)
        // {
        //     var currentUserProfileId = (await _unitOfWork.UserProfileRepository.GetUserByAppIdAsync(User.GetAppUserId())).Id;
        //     return Ok(await _unitOfWork.MessageRepository.GetMessagesThread(currentUserProfileId, recipientId));
        // }
    }
}