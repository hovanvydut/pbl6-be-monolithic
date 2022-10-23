using static Monolithic.Constants.PermissionPolicy;
using Microsoft.AspNetCore.Authorization;
using Monolithic.Services.Interface;
using Monolithic.Models.ReqParams;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;

namespace Monolithic.Controllers;

public class BookmarkController : BaseController
{
    private readonly IBookmarkService _bookmarkService;

    public BookmarkController(IBookmarkService bookmarkService)
    {
        _bookmarkService = bookmarkService;
    }

    [HttpGet]
    public async Task<BaseResponse<PagedList<BookmarkDTO>>> GetWithParams([FromQuery] BookmarkParams bookmarkParams)
    {
        var reqUser = HttpContext.Items["reqUser"] as ReqUser;
        var bookmarks = await _bookmarkService.GetBookmarks(reqUser.Id, bookmarkParams);
        return new BaseResponse<PagedList<BookmarkDTO>>(bookmarks);
    }

    [HttpPost]
    public async Task<BaseResponse<bool>> CreateBookmark([FromBody] CreateBookmarkDTO createBookmarkDTO)
    {
        if (ModelState.IsValid)
        {
            var reqUser = HttpContext.Items["reqUser"] as ReqUser;
            var bookmarkCreated = await _bookmarkService.CreateBookmark(reqUser.Id, createBookmarkDTO);
            if (bookmarkCreated)
                return new BaseResponse<bool>(bookmarkCreated, HttpCode.CREATED);
            else
                return new BaseResponse<bool>(bookmarkCreated, HttpCode.BAD_REQUEST, "", false);
        }
        return new BaseResponse<bool>(false, HttpCode.BAD_REQUEST, "Model state is not valid", false);
    }

    [HttpDelete("{bookmarkId}")]
    public async Task<BaseResponse<bool>> RemoveBookmark(int bookmarkId)
    {
        var reqUser = HttpContext.Items["reqUser"] as ReqUser;
        var bookmarkDeleted = await _bookmarkService.RemoveBookmark(reqUser.Id, bookmarkId);
        if (bookmarkDeleted)
            return new BaseResponse<bool>(bookmarkDeleted, HttpCode.NO_CONTENT);
        else
            return new BaseResponse<bool>(bookmarkDeleted, HttpCode.BAD_REQUEST, "", false);
    }
}