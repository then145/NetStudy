using API_Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetStudy.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

[Authorize]
[ApiController]
[Route("api/singlechat")]
public class SingleChatController : ControllerBase
{
    private readonly SingleChatService _chatService;
    private readonly JwtService _jwtService;
    private readonly UserService _userService;
    private readonly RsaService _rsaService;
    private readonly AesService _aesService;

    public SingleChatController(SingleChatService chatService, RsaService rsaServic, AesService aesService, JwtService jwtService, UserService userService)
    {
        _chatService = chatService;
        _jwtService = jwtService;
        _userService = userService;
        _rsaService = rsaServic;
        _aesService = aesService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SingleChat chat)
    {
        await _chatService.SendMessage(chat);
        return Ok(new { message = "Message sent successfully!" });
    }

    [HttpGet("history/{user1}/{user2}")]
    public async Task<ActionResult<List<SingleChat>>> GetChatHistory(string user1, string user2)
    {
        var userReq = await _userService.GetUserByUserName(user1);
        var userEnd = await _userService.GetUserByUserName(user2); 
        var key = await _jwtService.GetKeyChat(user1, user2); 
        var userKey = _rsaService.Encrypt(key, userReq.PublicKey);

        var chats = await _chatService.GetChatHistory(user1, user2);
        return Ok(new
        {
            data = chats,
            key = userKey,
        });
    }

    [HttpGet("get-friend-list/{username}")]
    public async Task<ActionResult<List<string>>> GetListFriend(string username)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        if (!_jwtService.IsValidate(authorizationHeader))
        {
            return Unauthorized(new { message = "Invalid request!" });
        }
        try
        {
            var friends = await _userService.GetListFriendIdByUsername(username);
            return Ok(new { total = friends.Count, data = friends });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal Server Error", details = ex.Message });
        }
    }
}