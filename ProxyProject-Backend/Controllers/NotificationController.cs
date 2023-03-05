using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.Models.RequestModels;
using ProxyProject_Backend.Models.Response;

namespace ProxyProject_Backend.Controllers
{
    [Route("api/notification")]
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
        [Route("GetNotifications")]
        public async Task<IActionResult> GetNotifications([FromQuery] GetListPagingModel model)
        {
            var notifications = await _unitOfWork.NotificationRepository
                     .GetAsync(null, x => x.OrderByDescending(p => p.CreatedDate), "", model.PageIndex, model.PageSize);

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

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("AddNotification")]
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

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("UpdateNotification")]
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
        [Authorize(Roles = UserRoles.Admin)]
        [Route("DeleteNotification")]
        public async Task<IActionResult> DeleteNotification(DeleteNotificationModel model)
        {
            var notification = await _unitOfWork.NotificationRepository.GetByIDAsync(model.Id);

            if (notification != null)
            {
                _unitOfWork.NotificationRepository.Delete(notification);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Message = "Delete notification successfully!"
                });
            }

            return BadRequest("Notification not found");
        }
    }
}
