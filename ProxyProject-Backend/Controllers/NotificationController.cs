using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.Models.RequestModels;
using ProxyProject_Backend.Models.Response;
using System.Linq.Expressions;

namespace ProxyProject_Backend.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    [Authorize]
    public class NotificationController : ApiBaseController
    {
        public NotificationController(
            ApplicationDbContext context,
            UserManager<UserEntity> userManager
            ) : base(context, userManager)
        {
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetNotifications([FromQuery] GetListPagingModel model)
        {
            Expression<Func<NotificationEntity, bool>> filter = null;

            if (!string.IsNullOrWhiteSpace(model.Keyword))
            {
                filter = (x) => x.Message.Contains(model.Keyword);
            }

            var notifications = await _unitOfWork.NotificationRepository
                     .GetAsync(filter, x => x.OrderByDescending(p => p.CreatedDate), "", model.PageIndex, model.PageSize);

            return Ok(new ResponseModel
            {
                Status = "Success",
                Data = notifications.Select(x => new NotificationModel
                {
                    Id = x.Id,
                    CreatedDate = x.CreatedDate,
                    Message = x.Message,
                    NotificationType = x.NotificationType
                }),
                Total = await _unitOfWork.NotificationRepository.CountByFilterAsync()
            });
        }

        [HttpGet]
        [Authorize(Roles = UserRolesConstant.Admin)]
        [Route("GetNotification")]
        public async Task<IActionResult> GetNotification(RequestNotificationModel model)
        {
            var notification = await _unitOfWork.NotificationRepository.GetByIDAsync(model.Id);

            if (notification != null)
            {
                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = new NotificationModel
                    {
                        Id = notification.Id,
                        CreatedDate = notification.CreatedDate,
                        Message = notification.Message,
                        NotificationType = notification.NotificationType
                    }
                });
            }

            return BadRequest("Notification not found");
        }

        [HttpPost]
        [Authorize(Roles = UserRolesConstant.Admin)]
        [Route("")]
        public async Task<IActionResult> AddNotification(AddNotificationModel model)
        {
            var notification = await _unitOfWork.NotificationRepository.InsertAsync(new NotificationEntity
            {
                Message = model.Message,
                NotificationType = model.NotificationType,
                CreatedDate = DateTime.UtcNow
            });

            await _unitOfWork.SaveChangesAsync();

            if(notification != null)
            {
                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = new NotificationModel
                    {
                        Id = notification.Id,
                        CreatedDate = notification.CreatedDate,
                        Message = notification.Message,
                        NotificationType = notification.NotificationType
                    }
                });
            }

            return BadRequest("Error when adding notification");

        }

        [HttpPatch]
        [Authorize(Roles = UserRolesConstant.Admin)]
        [Route("")]
        public async Task<IActionResult> UpdateNotification(UpdateNotificationModel model)
        {
            var notification = await _unitOfWork.NotificationRepository.GetByIDAsync(model.Id);

            if(notification != null)
            {
                notification.NotificationType = model.NotificationType;
                notification.Message = model.Message;

                _unitOfWork.NotificationRepository.Update(notification);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = new NotificationModel
                    {
                        Id = notification.Id,
                        CreatedDate = notification.CreatedDate,
                        Message = notification.Message,
                        NotificationType = notification.NotificationType
                    }
                });
            }

            return BadRequest("Notification not found");
        }

        [HttpDelete]
        [Authorize(Roles = UserRolesConstant.Admin)]
        [Route("")]
        public async Task<IActionResult> DeleteNotification(DeleteNotificationModel model)
        {
            //var notification = await _unitOfWork.NotificationRepository.GetByIDAsync(model.Id);

            _unitOfWork.NotificationRepository.DeleteList(model.Ids);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new ResponseModel
            {
                Status = "Success",
                Message = "Delete notification successfully!"
            });
        }
    }
}
