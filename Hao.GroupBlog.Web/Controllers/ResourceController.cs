using Hao.GroupBlog.Common.Enums;
using Hao.GroupBlog.Domain.Consts;
using Hao.GroupBlog.Domain.Interfaces;
using Hao.GroupBlog.Domain.Models;
using Hao.GroupBlog.Domain.Paging;
using Hao.GroupBlog.Manager.Basic;
using Hao.GroupBlog.Manager.Implements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Net.Http.Headers;

namespace Hao.GroupBlog.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceManager _manager;
        private readonly INoteManager _note;
        protected readonly IConfiguration _configuration;
        protected readonly PrivilegeManager _privilege;

        public ResourceController(IResourceManager manager,
            IConfiguration configuration,
            PrivilegeManager privilege,
            INoteManager note)
        {
            _manager = manager;
            _configuration = configuration;
            _privilege = privilege;
            _note = note;
        }

        [HttpPost("Save")]
        public async Task<ResponseResult<string>> Save(string ownerId, FileCategory category)
        {
            var res = new ResponseResult<string>();
            try
            {
                if (string.IsNullOrEmpty(ownerId)) throw new MyCustomException("ownerId is null!");
                if (category == FileCategory.other) throw new MyCustomException("category is null!");
                var invalid = !Request.HasFormContentType || Request.Form.Files == null || !Request.Form.Files.Any();
                if (invalid) throw new MyCustomException("No file found in the form!");
                var file = Request?.Form?.Files?.FirstOrDefault();
                if (file == null) throw new MyCustomException("No file found in the form!");

                string rootPath = _configuration[CfgConsts.FILE_RESOURCE_DIRECTORY];
                var directory = new DirectoryInfo(rootPath);
                if (!directory.Exists) { directory.Create(); }

                var code = _manager.GetNewCode();
                var suffix = Path.GetExtension(file.FileName);
                var newName = file.FileName;
                var newFileName = code + suffix;
                var fp = @$"{rootPath}\{newFileName}";

                using (var fs = new FileStream(fp, FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(fs);
                }

                var model = new FileM()
                {
                    Code = code,
                    Name = file.FileName,
                    OwnId = ownerId,
                    FileName = newFileName,
                    Type = file.ContentType,
                    Suffix = suffix,
                    Size = file.Length,
                    Category = category
                };

                var result = await _manager.Save(model);
                if (result.Success) res.Data = model.FileName;
                else new MyCustomException(result.AllMessages);
            }
            catch (Exception e)
            {
                res.AddError(e);
            }
            return res;
        }

        [HttpGet("GetByCode")]
        public async Task<ResponseResult<FileM>> GetByCode(string code)
        {
            return await _manager.GetByCode(code);
        }

        [AllowAnonymous]
        [HttpGet("GetFileByName")]
        public async Task<IActionResult> GetFileByName(string name, string key)
        {
            try
            {
                try { await _privilege.GetLoginRecord(key); }
                catch (Exception e) { return Forbid(e.Message); }

                return GetLocalFileByName(name);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("GetNoteFileByName")]
        public async Task<IActionResult> GetNoteOpenedFileByName(string name, string noteId)
        {
            try
            {
                try {
                    var open = await _note.IsOpen(noteId);
                    if (!open) throw new Exception("note is not opened!");
                }
                catch (Exception e) { return Forbid(e.Message); }

                return GetLocalFileByName(name);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("GetList")]
        public async Task<ResponsePagingResult<ResourceM>> GetList(PagingParameter<ResourceFilter> parameter)
        {
            return await _manager.GetList(parameter);
        }

        private IActionResult GetLocalFileByName(string name)
        {
            string rootPath = _configuration[CfgConsts.FILE_RESOURCE_DIRECTORY];
            string path = @$"{rootPath}\{name}";
            var file = new FileInfo(path);
            if (!file.Exists) return NotFound();

            var suffix = Path.GetExtension(name);
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[suffix]; // 获取文件类型
            string? type = new MediaTypeHeaderValue(memi).MediaType;
            type = type == null ? "" : type;
            FileStream fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
            return File(fs, contentType: type, file.Name, enableRangeProcessing: true);
        }
    }
}
